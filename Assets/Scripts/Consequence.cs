using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Consequence
{
    public int playerHpChange, playerStamChange, monsterDmg, natureDmg;
    public Action specialAction;
    public string description;

    public Consequence() {
       
    }

    public Consequence(int playerHpChange, int playerStamChange, int monsterDmg, int natureDmg, Action specialAction, string description) {
        this.playerHpChange = playerHpChange;
        this.playerStamChange = playerStamChange;
        this.monsterDmg = monsterDmg;
        this.natureDmg = natureDmg;
        this.specialAction = specialAction;
        this.description = description;
    }

    public void evaluate() {
        GameManager.instance.player.updateStats(playerHpChange, playerStamChange);
        GameManager.instance.room.obstacle.assignDamage(monsterDmg, natureDmg);
        if (specialAction != null) specialAction();
    }

    public Consequence clone() {
        return new Consequence(playerHpChange, playerStamChange, monsterDmg, natureDmg, specialAction, description);
    }


}
