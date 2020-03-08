using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Consequence
{
    //default properties
    public int playerHpChange, playerStamChange, monsterDmg, natureDmg;
    public Action specialAction;
    public string description;

    //extra properties
    public int distanceChange;



    public Consequence() {
       
    }

    public Consequence(int playerHpChange, int playerStamChange, int monsterDmg, int natureDmg, Action specialAction, string description) {
        this.playerHpChange = playerHpChange;
        this.playerStamChange = playerStamChange;
        this.monsterDmg = monsterDmg;
        this.natureDmg = natureDmg;
        this.specialAction = specialAction;
        this.description = description;
        this.distanceChange = 0;
    }

    public Consequence(int playerHpChange, int playerStamChange, int monsterDmg, int natureDmg, Action specialAction, string description, int distanceChange ) {
        this.playerHpChange = playerHpChange;
        this.playerStamChange = playerStamChange;
        this.monsterDmg = monsterDmg;
        this.natureDmg = natureDmg;
        this.specialAction = specialAction;
        this.description = description;
        this.distanceChange = distanceChange;
    }

    public void evaluate() {
        GameManager gm = GameManager.instance;
        if (specialAction != null) specialAction();
        gm.player.updateStats(playerHpChange, playerStamChange);
        gm.room.obstacle.assignDamage(monsterDmg, natureDmg);
        gm.room.distance += distanceChange;
        Debug.Log(distanceChange);
    }

    public Consequence clone() {
        return new Consequence(playerHpChange, playerStamChange, monsterDmg, natureDmg, specialAction, description, distanceChange);
    }

    //Useful Special Actions

    public void spendLessStamina(int stamSave) {
        specialAction += () => {
            if (playerStamChange < 0) playerStamChange = Math.Min(0, playerStamChange + stamSave);
        };
    }

    public void dealMoreDamage(int monsterDmg, int natureDmg) {
        this.monsterDmg -= monsterDmg;
        this.natureDmg -= natureDmg;
    }

    public void flee(float threshold) { //would like this to be after damage rather than before
        specialAction += () => {
            Obstacle obs = GameManager.instance.room.obstacle;
            if (obs.hpFraction() <= threshold) {
                obs.unCleared = () => {; };
                obs.chases = false;
                Debug.Log("Fled");
            }
            
        };
    }



}
