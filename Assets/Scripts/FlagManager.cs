using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class FlagManager
{
    //Want to make these event actions but w/e 
    public List<Flag> afterNewRoom;
    public List<Flag> onGenerateOption;
    public List<Flag> onProcessOption;
    public List<Flag> onNewRoom;

    public Option currentlyGenerating { get; set; }
    public Option currentlyEvaluating { get; set; }


    public class Flag
    {
        static int flagid = 0;

        public int id;
        public event Action todo;
        public bool toDelete;
        /*
        public Flag(Action todo) {
            id = flagid++; // yeet
            this.todo = todo;
        }
        */

        public Flag() {
            id = flagid++; // yeet
            //Debug.Log("Created Flag " + id);
            toDelete = false;
        }

        public void invoke() {
            if (!toDelete && todo != null) todo();
        }



    }


    public FlagManager() {
        onGenerateOption = new List<Flag>();
        onProcessOption = new List<Flag>();
        onNewRoom = new List<Flag>();
        afterNewRoom = new List<Flag>();
    }

    public void invoke(List<Flag> list) {
        list.RemoveAll(flag => flag.toDelete); //Crazy syntax
        for (int i = 0; i < list.Count; i++) { //Not using foreach because we may alter list during processing
            if (list[i] != null) {
                //Debug.Log("Action " + list[i].id);
                list[i].invoke();
            }
        }
    
    }

    public int findById(List<Flag> list, int id) {
        return list.FindIndex(flag => flag.id == id); //More syntax
    }

    public void destroyById(List<Flag> list, int id) {
        int ind = findById(list, id);
        if (ind >= 0) list[ind].toDelete = true;
        Debug.Log("Destroyed Flag " + id);
    }




    //Useful flag creation methods. Using these will hopefully prevent memory leaks caused by buildup of uncleared flags **********

    //Creates a flag that lasts for one room
    public Flag oneRoomFlag(List<Flag> list) {
        Flag destroy;
        return oneRoomFlag(list, out destroy);
    }

    //Creates a flag to last for one room, also giving access to destruction flag
    //TODO special flag when list = onNewRoom
    public Flag oneRoomFlag(List<Flag> list, out Flag destroy) {
        Flag flag = new Flag();
        destroy = new Flag();
        int id = destroy.id;
        destroy.todo += () => destroyById(list, flag.id);
        destroy.todo += () => destroyById(onNewRoom, id);

        list.Add(flag);
        onNewRoom.Add(destroy);

        return flag;
    }

    //Maybe would rather have modification action work on the consequence rather than the option?
    public Flag modifyNextOptionIf(Predicate<Option> match, Action<Option> modification, bool single_use, bool single_room) {
        if (!single_room && !single_use) Debug.Log("CAUTION: Perpetual Flag created.");
        Flag flag;
        if (single_room) flag = oneRoomFlag(onProcessOption);
        else flag = new Flag();

        //Debug.Log("Made Modification Flag " + flag.id);
        flag.todo += () => {
            if (match(currentlyEvaluating)) {
                Debug.Log("Action " + flag.id);
                modification(currentlyEvaluating);
                if (single_use) flag.toDelete = true;
            }
        };
        return flag;
    }

    public Flag modifyNextObstacleIf(Predicate<Obstacle> match, Action<Obstacle> modification, bool single_use) {
        if (!single_use) Debug.Log("CAUTION: Perpetual Flag created.");

        //Debug.Log("Made Modification Flag " + flag.id);
        Flag flag = new Flag();
        flag.todo += () => {
            if (match(GameManager.instance.room.obstacle)) {                
                modification(GameManager.instance.room.obstacle);
                if (single_use) flag.toDelete = true;
            }
        };

        afterNewRoom.Add(flag);

        return flag;
    }

}
