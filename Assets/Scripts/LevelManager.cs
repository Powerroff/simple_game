using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager
{

    class roomNode
    {
        public roomNode northExit;
        public roomNode eastExit;
        public roomNode southExit;
        public roomNode westExit;
        public Room room;

        public roomNode() {
            northExit = null;
            eastExit = null;
            southExit = null;
            westExit = null;
            room = null;
        }

        public void init() {
            room.init(northExit.room, eastExit.room, southExit.room, westExit.room);
        }
            

    }

    public LevelManager() {

    }

    public Room initRooms(int numRooms) {
        roomNode oldR = null;
        for (int i = 0; i < numRooms; i++) {
            roomNode r = new roomNode();
            r.room = new Room();
            if (i > 0) {
                r.westExit = oldR;
                oldR.eastExit = r;
            }
            
            oldR = r;
        }
        return oldR.room;
    }

    
}
