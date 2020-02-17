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
    public Option optionSelected = null;
    // Start is called before the first frame update
    void Start()
    {
        roomCount = 0;

        uim.init();
        uim.nextRoom();//A bit weird to do it this way...

    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public void nextRoom() {
        //Process old room
        if (room) player.stats.stamina--;

        if (optionSelected != null) optionSelected.onpress.Invoke();
        optionSelected = null;

        processObstacle();
        Room tempRoom = gameObject.AddComponent<Room>() as Room;
        if(room) Destroy(room);
        room = tempRoom;

        //Increment roomCount
        roomCount++;

        //Generate Obstacle
        Obstacle o = Obstacle.tempPackage()[Random.Range(0, Obstacle.tempPackage().Length)];
        room.obstacle = o; //A bit jank that we need to set the obstacle before generating the options
        

        //Generate Options
        room.options = generateOptions(Option.tempPackage());

        //Generate Background Color
        room.backgroundColor = Random.ColorHSV(0f, .5f, .5f, .5f, 0.5f, 1f);  // I've limited the background colors to the lighter half of the spectrum.


    }

    Option[] generateOptions(Option[] possibleOptions) {
        if (possibleOptions.Length < 2) {
            print("Uh Oh");
            Application.Quit();
        }
        int option1 = Random.Range(0, possibleOptions.Length);
        int option2 = Random.Range(0, possibleOptions.Length - 1);
        if (option2 == option1) option2++;
        return new Option[] { possibleOptions[option1], possibleOptions[option2] };
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
