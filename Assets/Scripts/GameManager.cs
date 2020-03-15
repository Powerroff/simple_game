using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    
    //Static instance of GameManager which allows it to be accessed by any other script.
    public static GameManager instance = null;
    void Awake() {
        Debug.Log("awoke");

        //Check if instance already exists
        if (instance == null) {
            Debug.Log("Instantiating gm");
            //if not, set instance to this
            instance = this;

            fm = new FlagManager(); //Make a component maybe? Also don't want this here.
        }

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, 
            // meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene / Switching scenes
        DontDestroyOnLoad(gameObject); // VERY IMPORTANT

    }
    //^^ Above section creates a static GameManager

    public Room room { get; set; }
    public UIManager uim { get; set; }
    public FlagManager fm { get; set; }
    public Player player { get; set; }
    public int roomCount { get; set; }
    int bossRoom;
    public bool[] optionsSelected { get; set; }
    public string consequences { get; set; }

    public GameObject endTextPrefab; //Refactor this
    public bool didBoss1; //Redo this

    // Start is called before the first frame update
    void Start()
    {
        roomCount = 0;
        bossRoom = -15;

        uim = GameObject.Find("UIManager").GetComponent<UIManager>();
        player = GameObject.Find("Player").GetComponent<Player>();

        uim.init();
        nextRoom();

    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public void nextRoom() {

        if (player.stats.stamina <= 0 || player.stats.hp <= 0) {
            onGameEnd(false);
            return;
        }

        //Process old room
        int distance = 0;
        if (room) {
            if (roomCount > 50 || room.distance > 50) {
                onGameEnd(true);
                return;
            }

            if (processObstacle()) {
                repeatRoom(); //If obstacle chases you, repeat.
                return;
            }

            //Pickup relic
            if (room.relic != null) room.relic.onPickup.Invoke();

            //Process Flags
            fm.invoke(fm.onNewRoom);
            distance = room.distance;
        }

        Room tempRoom = gameObject.AddComponent<Room>() as Room;
        if(room) Destroy(room);
        room = tempRoom;

        //Set distance
        room.distance = distance + 1;
        bossRoom += 1 + (roomCount % 2);

        //Increment roomCount
        roomCount++;

        //Generate Relic **TEMP**
        //if (roomCount == 10) room.relic = Relic.tempRelicOne();

        //Generate Obstacle
        if (bossDistance() == 0 && !didBoss1) {
            room.obstacle = Obstacle.boss1();
            bossRoom = roomCount-15;
        } else if (bossDistance() == 0 && didBoss1) room.obstacle = Obstacle.boss2();
        else {
            Obstacle o = Obstacle.defaultPackage()[Random.Range(0, 4)];
            room.obstacle = o;
        }
        

        //Generate Options
        room.options = player.optionTree.getFirstLayer();
        initOptionCons();

        //Generate Background Color
        room.backgroundColor = Random.ColorHSV(0f, .5f, .5f, .5f, 0.5f, 1f);  // I've limited the background colors to the lighter half of the spectrum.

        fm.invoke(fm.afterNewRoom);

        uim.nextRoom();
        Debug.Log("Room Count " + roomCount);


    }

    public int bossDistance() {
        return System.Math.Max(room.distance - bossRoom, 0);
    }

    public void repeatRoom() {
        fm.invoke(fm.onNewRoom);
        roomCount++; 
        room.options = player.optionTree.getFirstLayer();
        initOptionCons();
        room.backgroundColor = Random.ColorHSV(0f, .5f, .5f, .5f, 0.5f, 1f);  // I've limited the background colors to the lighter half of the spectrum.
        uim.nextRoom();
    }



    public void takeAction() {

        if (room.options.Count == 0) {
            Invoke("nextRoom", .5f);
            return;
        }

        StatsManager oldStats = player.stats.clone();
        int obstacleHp = room.obstacle.health;
        nextOptions(processOption()); //hmm
        generateConsequenceStr(oldStats, obstacleHp);
        uim.displayConsequence();

        if (room.options.Count == 0)
            uim.endRoom();
        else
            uim.Invoke("newOPM", .5f);

    }

    //Jank
    public void generateConsequenceStr(StatsManager oldStats, int obstacleHp) {
        consequences = "";

        int deltaStam = player.stats.stamina - oldStats.stamina;
        string dStam = (deltaStam >= 0 ? "+" : "") + deltaStam;
        int deltaHp = player.stats.hp - oldStats.hp;
        string dHp = (deltaHp >= 0 ? "+" : "") + deltaHp;
        int deltaObsHp = room.obstacle.health - obstacleHp;
        string dObsHp = (deltaObsHp >= 0 ? "+" : "") + deltaObsHp;

        consequences += "Obstacle " + dObsHp + " health. Player "  + dStam + " stamina, " + dHp + " hp.";
    }


    public List<Option> processOption() {
        List<Option> selected = new List<Option>();
        for (int i = 0; i < room.options.Count; i++) {
            if (uim.opm.optionsSelected[i]) {
                fm.currentlyEvaluating = room.options[i];
                fm.invoke(fm.onProcessOption);
                room.options[i].onPress();
                selected.Add(room.options[i]);
            }
            room.options[i].reinforce(uim.opm.optionsSelected[i]);
        }
        player.stats.stamina -= System.Math.Max(selected.Count - 1, 0);
        return selected;
    }

    public void nextOptions(List<Option> selected) {
        room.options = new List<Option>();
        foreach (Option option in selected) {
            if (room.obstacle.health > 0) {
                room.options.AddRange(option.getChildrenList());
            } else {
                room.options.AddRange(option.randomRewards());
                }
                
            }
        initOptionCons();
        if (room.options.Count > 0 && room.obstacle.uniqueOption != null) 
            room.options.Add(room.obstacle.uniqueOption);
            
    }


    bool processObstacle() {
        if (room)
            if (room.obstacle.health > 0) {
                room.obstacle.unCleared.Invoke();
                return room.obstacle.chases;
            } else {
                room.obstacle.cleared.Invoke();
                return false;
            }
        return false;
    }

    void initOptionCons() {
        foreach (Option option in room.options) {
            option.generateConsequence();
            fm.currentlyGenerating = option;
            fm.invoke(fm.onGenerateOption);
        }
    }

    //this could be refactored to be more general
    public void redoObstacle() {
        if (room.obstacle.obstacleClass == Obstacle.ObstacleClass.Nature)
            room.obstacle = Obstacle.allOfClass(Obstacle.ObstacleClass.Monster)[Random.Range(0, 2)];
        else
            room.obstacle = Obstacle.allOfClass(Obstacle.ObstacleClass.Nature)[Random.Range(0, 2)];
    }


    //Refactor this
    public void onGameEnd(bool victory) {
        //Destroy(uim.gameObject);
        //SceneManager.LoadScene("GameEndScene");
        Text vText = Instantiate(endTextPrefab, uim.canvas.transform).GetComponent<Text>();
        if (victory) vText.text = "Victory";
        else vText.text = "Defeat";
        Debug.Log(vText.text);
    }


}
