using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (roomCount == 10) room.relic = Relic.tempRelicOne();

        //Generate Obstacle
        Obstacle o = Obstacle.tempPackage()[Random.Range(0, Obstacle.tempPackage().Length)];
        room.obstacle = o; //A bit jank that we need to set the obstacle before generating the options
        

        //Generate Options
        room.options = generateOptions(Option.tempPackage());

        //Generate Background Color
        room.backgroundColor = Random.ColorHSV(0f, .5f, .5f, .5f, 0.5f, 1f);  // I've limited the background colors to the lighter half of the spectrum.

        uim.nextRoom();


    }

    public void takeAction() {
        consequences = "";
        StatsManager oldStats = player.stats.clone();
        int obstacleHp = room.obstacle.health;
        processOption(); 

        //Jank
        int deltaStr = player.stats.strength - oldStats.strength;
        string dStr = (deltaStr >= 0 ? "+" : "") + deltaStr;
        int deltaStam = player.stats.stamina - oldStats.stamina;
        string dStam = (deltaStam >= 0 ? "+" : "") + deltaStam;
        int deltaHp = player.stats.hp - oldStats.hp;
        string dHp = (deltaHp >= 0 ? "+" : "") + deltaHp;
        int deltaObsHp = room.obstacle.health - obstacleHp;
        string dObsHp = (deltaObsHp >= 0 ? "+" : "") + deltaObsHp;

        consequences += "Obstacle " + dObsHp + " health. Player " + dStr + " strength, " + dStam + " stamina, " + dHp + " hp.";

        uim.displayConsequence();

        StartCoroutine(waitForNextRoom());
    }

    IEnumerator waitForNextRoom() {
        yield return new WaitForSeconds(3);
        nextRoom();
    }

    public void processOption() {
        player.stats.stamina--;
        int numSelected = 0;
        for (int i = 0; i < room.options.Length; i++) {
            if (uim.opm.optionsSelected[i]) {
                room.options[i].onpress();
                numSelected++; 
            }
        }
        if (numSelected > 0) player.stats.stamina -= (numSelected - 1);
    }

    Option[] generateOptions(Option[] possibleOptions) {
        if (possibleOptions.Length < 2) {
            print("Uh Oh");
            Application.Quit();
        }
        int option1 = Random.Range(0, possibleOptions.Length);
        int option2 = Random.Range(0, possibleOptions.Length - 1);
        if (option2 == option1) option2++;
        //return new Option[] { possibleOptions[option1], possibleOptions[option2] };
        return new Option[] { possibleOptions[option1], possibleOptions[option2], possibleOptions[option1] };
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
