using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public bool[] optionsSelected { get; set; }
    public string consequences { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        roomCount = 0;

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
        //Process old room
        if (room) {
            processObstacle();

            //Pickup relic
            if (room.relic != null) room.relic.onPickup.Invoke();

            //Process Flags
            fm.invoke(fm.onNewRoom);
        }

        Room tempRoom = gameObject.AddComponent<Room>() as Room;
        if(room) Destroy(room);
        room = tempRoom;

        //Increment roomCount
        roomCount++;

        //Generate Relic **TEMP**
        //if (roomCount == 10) room.relic = Relic.tempRelicOne();

        //Generate Obstacle
        Obstacle o = Obstacle.defaultPackage()[Random.Range(0, 4)];
        room.obstacle = o;
        

        //Generate Options
        room.options = player.optionTree.getFirstLayer();
        initOptionCons();

        //Generate Background Color
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


    void processObstacle() {
        if (room)
            if (room.obstacle.health > 0) {
                room.obstacle.unCleared.Invoke();
            } else {
                room.obstacle.cleared.Invoke();
            }
    }

    void initOptionCons() {
        foreach (Option option in room.options) {
            option.generateConsequence();
            fm.currentlyGenerating = option;
            fm.invoke(fm.onGenerateOption);
        }
    }



}
