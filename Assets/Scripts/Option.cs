using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Option
{
    //These (global) variables are looked up from GameManager.instance upon creation
    Player player;
    Obstacle obstacle;

    //Other variables
    public string description;
    public UnityAction onpress;  //Capitalization?


    public Option() {
        onpress = (() => {; });
        player = GameManager.instance.player;
        obstacle = GameManager.instance.room.obstacle;
    }







    // Prebuilt Options **********************

    public static Option tempA() {
        Option o = new Option();
        o.description = "Train\n\nChance to gain strength.";
        if (Random.Range(0f, 1f) < .75) {
            o.onpress += o.player.changeStr(1);
            o.onpress += (() => Debug.Log("Success!"));
        }

        return o;
    }

    public static Option tempB() {
        Option o = new Option();
        o.description = "Recover\n\nChance to gain heal and regain stamina.";

        if (Random.Range(0f, 1f) < .75)
            o.onpress += o.player.changeHp(2);
        if (Random.Range(0f, 1f) < .75)
            o.onpress += o.player.changeStam(2);

        return o;
    }

    public static Option tempC() {
        Option o = new Option();
        o.description = string.Format("Wing it\n\n{0}-{1} Damage to all types",o.player.stats.strength, o.player.stats.strength + 2);
        int dmg = Random.Range(o.player.stats.strength, o.player.stats.strength + 3);
        o.onpress += o.obstacle.changeHp(-dmg);
        o.onpress += (() => Debug.Log("Damage " + dmg));
        return o;
    }

    public static Option tempD() {
        Option o = new Option();
        o.description = string.Format("Harvest\n\n{0} Damage to nature.\n -1 Stamina", 2 + o.player.stats.strength);

        if (o.obstacle.obstacleClass == Obstacle.ObstacleClass.Nature) {            //Maybe could be reworked a bit
            o.onpress += o.obstacle.changeHp(-2-o.player.stats.strength);
        }
        o.onpress += o.player.changeStam(-1);
        return o;
    }

    public static Option tempE() {
        Option o = new Option();
        o.description = string.Format("Hack and Slash\n\n{0} Damage to monsters.\n -1 Stamina", 2+o.player.stats.strength);

        if (o.obstacle.obstacleClass == Obstacle.ObstacleClass.Monster) {            //Maybe could be reworked a bit
            o.onpress += o.obstacle.changeHp(-2-o.player.stats.strength);
        }
        o.onpress += o.player.changeStam(-1);
        return o;
    }

    public static Option[] tempPackage() {
        return new Option[] { tempA(), tempB(), tempC(), tempD(), tempE() };
    }
    

    
}
