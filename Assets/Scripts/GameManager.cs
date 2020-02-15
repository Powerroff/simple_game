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
        nextRoom();
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public void nextRoom() {
        if (room)
            if (room.obstacle.health > 0) {
                room.obstacle.unCleared.Invoke();
            } else {
                room.obstacle.cleared.Invoke();
            }

        Room tempRoom = gameObject.AddComponent<Room>() as Room;
        if(room) Destroy(room);
        room = tempRoom;


        
        room.init(Option.tempPackage(player), Obstacle.tempPackage(player));

        
        room.newButtons(uim.leftOption, uim.rightOption);
        uim.updateBackground(room);
        uim.UpdateRoomText(room);

    }





}
