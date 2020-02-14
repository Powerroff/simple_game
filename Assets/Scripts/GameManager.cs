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
        room.init();
        uim.updateBackground(room);

    }



}
