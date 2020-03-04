using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlagManager
{

    public List<Flag> onGenerateOption;
    public List<Flag> onProcessOption;
    public List<Flag> onNewRoom;
    public Option option = null;

    List<Flag> toPurge;

    public FlagManager() {
        onGenerateOption = new List<Flag>();
        onProcessOption = new List<Flag>();
        onNewRoom = new List<Flag>();
    }


    public void evaluate(List<Flag> list) {
        toPurge = new List<Flag>();
        foreach (Flag flag in list)
            flag.invoke();
        foreach (Flag flag in toPurge) list.Remove(flag);
        toPurge = new List<Flag>();
    }

    //Adds a flag to a room and returns its destruction flag
    public Flag addForRoom(Flag flag, List<Flag> list) {
        list.Add(flag);

        Flag destroyFlag = destroyOnNewRoom();
        destroyFlag.addAction(() => toPurge.Add(flag));

        return destroyFlag;
        
    }

    public class Flag
    {
        UnityAction todo;

        public Flag() {
            todo = () => {; };
        }

        public Flag(UnityAction action) {
            todo = action;
        }

            public void addAction(UnityAction toAdd) {
            todo += toAdd;
        }

        public void invoke() {
            todo();
        }
    }





    //Useful flags + actions
    /*
    UnityAction removeFlag(Flag flag, List<Flag> list) {
        return () => { list.Remove(flag); };
    }
    */

    Flag destroyOnNewRoom() {
        Flag flag = new Flag(); 
        flag.addAction( () => { toPurge.Add(flag); });
        onNewRoom.Add(flag);
        return flag;
    }

}
