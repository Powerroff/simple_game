using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Relic 
{ 

    public static bool tempRelicOneFlag = false; //may move this later

    public string name;
    public string description;
    public UnityAction onPickup;  //Use a different delegate class maybe?

    public Relic() {
 
    }

    public static Relic tempRelicOne() {
        Relic r = new Relic();
        r.name = "Cognition Augment";
        r.description = "Allows player to select both options, at the cost of -1 stamina";
        r.onPickup = () => { tempRelicOneFlag = true; };
        return r;
    }


}
