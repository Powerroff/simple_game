using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager
{
    public int hp;
    public int stamina;

    public StatsManager() {

    }

    public StatsManager(int hp, int stamina) {
        this.hp = hp;
        this.stamina = stamina;
    }

    public StatsManager clone() {
        return new StatsManager(hp, stamina);
    }
}
