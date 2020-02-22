using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager
{
    public int hp;
    public int stamina;
    public int strength;

    public StatsManager() {

    }

    public StatsManager(int hp, int stamina, int strength) {
        this.hp = hp;
        this.stamina = stamina;
        this.strength = strength;
    }

    public StatsManager clone() {
        return new StatsManager(hp, stamina, strength);
    }
}
