using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private LevelManager level;
    // Start is called before the first frame update
    void Start()
    {
        level = new LevelManager();
        Room startRoom = level.initRooms(5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
