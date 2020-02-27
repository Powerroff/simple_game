using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager
{
    public int maxHp;
    int _hp;
    public int hp {
        get {
            return this._hp;
        }
        set {
            this._hp = System.Math.Min(value, maxHp);
        }
    }
    public int stamina;

    public StatsManager() {

    }

    public StatsManager(int maxHp,  int hp, int stamina) {
        this.maxHp = maxHp;
        this._hp = hp;
        this.stamina = stamina;
    }

    public StatsManager clone() {
        return new StatsManager(maxHp, hp, stamina);
    }
}
