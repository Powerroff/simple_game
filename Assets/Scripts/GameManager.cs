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
        room = gameObject.AddComponent<Room>() as Room;
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public void nextRoom() {
        Room tempRoom = gameObject.AddComponent<Room>() as Room;
        Destroy(room);
        room = tempRoom;


        Option tempA = new Option("Strength UP", () => { player.stats.strength++; });
        Option tempB = new Option("Recover Stamina", () => { player.stats.stamina++; });
        Option tempC = new Option("Increase HP", () => { player.stats.hp += 2; });
        Option tempD = new Option("Do nothing", () => {; });
        room.init(new Option[]{ tempA, tempB, tempC, tempD});

        
        room.newButtons(uim.leftOption, uim.rightOption);
        uim.updateBackground(room);

    }





}
