using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Option
{
    public string description;
    public UnityAction onpress;

    public static Player player;
    public static Obstacle obstacle;


    public Option(string s, UnityAction op) {
        description = s;
        onpress = op;
    }

    public Option() {
        onpress = (() => {; });
    }







    // Prebuilt Options **********************

    public static Option tempA() {
        Option o = new Option();
        o.description = "Train\n\nChance to gain strength.";

        if (Random.Range(0, 1) < .5) {
            o.onpress += player.changeStr(1);
            o.onpress += (() => Debug.Log("Success!"));
        }

        return o;
    }

    public static Option tempB() {
        Option o = new Option();
        o.description = "Recover\n\nChance to gain heal and regain stamina.";

        if (Random.Range(0, 1) < .75)
            o.onpress += player.changeHp(2);
        if (Random.Range(0, 1) < .75)
            o.onpress += player.changeStam(2);

        return o;
    }

    public static Option tempC() {
        Option o = new Option();
        o.description = "Wing it\n\n0-2 Damage to all types";

        int dmg = Random.Range(0, 3);
        o.onpress += obstacle.changeHp(-dmg);
        o.onpress += (() => Debug.Log("Damage " + dmg));
        return o;
    }

    public static Option tempD() {
        Option o = new Option();
        o.description = "Harvest\n\n2 Damage to nature.\n -1 Stamina";

        if (obstacle.obstacleClass == Obstacle.ObstacleClass.Nature) {            //Maybe could be reworked a bit
            o.onpress += obstacle.changeHp(-2);
        }
        o.onpress += player.changeStam(-1);
        return o;
    }

    public static Option tempE() {
        Option o = new Option();
        o.description = "Hack and Slash\n\n2 Damage to monsters.\n -1 Stamina";

        if (obstacle.obstacleClass == Obstacle.ObstacleClass.Monster) {            //Maybe could be reworked a bit
            o.onpress += obstacle.changeHp(-2);
        }
        o.onpress += player.changeStam(-1);
        return o;
    }

    public static Option[] tempPackage() {
        return new Option[] { tempA(), tempB(), tempC(), tempD(), tempE() };
    }
    

    
}
