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

    public Room room;
    public UIManager uim;
    public Player player;
    public int roomCount;
    public bool[] optionsSelected;
    public string consequences;
    // Start is called before the first frame update
    void Start()
    {
        roomCount = 0;

        optionsSelected = new bool[2];

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
        }

        Room tempRoom = gameObject.AddComponent<Room>() as Room;
        if(room) Destroy(room);
        room = tempRoom;

        //Increment roomCount
        roomCount++;

        //Generate Relic **TEMP**
        //if (roomCount == 10) room.relic = Relic.tempRelicOne();

        //Generate Obstacle
        Obstacle o = Obstacle.tempPackage()[Random.Range(0, Obstacle.tempPackage().Length)];
        room.obstacle = o; //A bit jank that we need to set the obstacle before generating the options
        

        //Generate Options
        room.options = new List<Option>(new Option[] { Option.hackSlash(), Option.harvest() });

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

        int deltaStr = player.stats.strength - oldStats.strength;
        string dStr = (deltaStr >= 0 ? "+" : "") + deltaStr;
        int deltaStam = player.stats.stamina - oldStats.stamina;
        string dStam = (deltaStam >= 0 ? "+" : "") + deltaStam;
        int deltaHp = player.stats.hp - oldStats.hp;
        string dHp = (deltaHp >= 0 ? "+" : "") + deltaHp;
        int deltaObsHp = room.obstacle.health - obstacleHp;
        string dObsHp = (deltaObsHp >= 0 ? "+" : "") + deltaObsHp;

        consequences += "Obstacle " + dObsHp + " health. Player " + dStr + " strength, " + dStam + " stamina, " + dHp + " hp.";
    }


    public List<Option> processOption() {
        player.stats.stamina--;
        List<Option> selected = new List<Option>();
        for (int i = 0; i < room.options.Count; i++) {
            if (uim.opm.optionsSelected[i]) {
                room.options[i].onpress();
                selected.Add(room.options[i]);
            }
        }
        player.stats.stamina -= System.Math.Max(selected.Count - 1, 0);
        return selected;
    }

    public void nextOptions(List<Option> selected) {
        room.options = new List<Option>();
        foreach (Option option in selected) {
            if (room.obstacle.health > 0) {
                room.options.AddRange(option.nextOptions);
            } else {
                room.options.AddRange(option.onKill);
            }
        if (room.options.Count > 0 && room.obstacle.uniqueOption != null) {
                room.options.Add(room.obstacle.uniqueOption);
            }
        }
    }


    void processObstacle() {
        if (room)
            if (room.obstacle.health > 0) {
                room.obstacle.unCleared.Invoke();
            } else {
                room.obstacle.cleared.Invoke();
            }
    }





}
