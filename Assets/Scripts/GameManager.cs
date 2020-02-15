using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Room room;
    public UIManager uim;
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        Option.player = player;
        Obstacle.player = player;
        nextRoom();
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public void nextRoom() {
        //Process old room
        processObstacle();
        Room tempRoom = gameObject.AddComponent<Room>() as Room;
        if(room) Destroy(room);
        room = tempRoom;

        //Generate Obstacle
        Obstacle o = Obstacle.tempPackage()[Random.Range(0, Obstacle.tempPackage().Length)];
        Option.obstacle = o;
        room.obstacle = o;

        //Generate Options
        room.options = generateOptions(Option.tempPackage());

        //Generate Background Color
        room.backgroundColor = Random.ColorHSV(0f, .5f, .5f, .5f, 0.5f, 1f);  // I've limited the background colors to the lighter half of the spectrum.

        //Update UI
        uim.updateWithRoomInformation(room);

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
