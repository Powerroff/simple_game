using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public partial class Obstacle
{
    //Useful Macros for the class
    public enum ObstacleClass { Nature, Monster };
    readonly string[] monsterPrefixes = { "", "Feral ", "Cunning ", "Big ", "Warped ", "Gigantic " };
    readonly string[] naturePrefixes = { "", "Overgrown ", "Sinister ", "Evolved ", "Carnivorous ", "Possessed " };

    //These (global) variables will be looked up from GameManager.instance upon creation
    GameManager gm;

    //These variables will be specific to the obstacle
    int _health;
    int _maxHealth;
    public int maxHealth {
        get {
            return _maxHealth;
        }
        set {
            _maxHealth = value;
            _health = value;
        }
    }
    public int health {
        get {
            return _health;
        }
        set {
            _health = Math.Min(value, _maxHealth);
        }
    }
    public ObstacleClass obstacleClass;
    public Action unCleared, cleared;
    public string name, description;
    public Option uniqueOption;
    public bool chases;


    //Constructor and methods
    public Obstacle() {
        chases = false;
        uniqueOption = null;
        unCleared = () => {; };
        cleared = () => {; };

    }

    /*
    string getPrefix() {
        if (obstacleClass == ObstacleClass.Nature) return naturePrefixes[Math.Min(naturePrefixes.Length - 1, roomCount / 10)];
        if (obstacleClass == ObstacleClass.Monster) return monsterPrefixes[Math.Min(monsterPrefixes.Length - 1, roomCount / 10)];
        return "";
    } */

    public Action changeHp(int amount) {
        return (() => { health = Math.Max(health + amount, 0); });
    }

    public void assignDamage(int monsterDmg, int natureDmg) {
        if (obstacleClass == ObstacleClass.Monster)
            health = Math.Max(health + monsterDmg, 0);
        if (obstacleClass == ObstacleClass.Nature)
            health = Math.Max(health + natureDmg, 0);
    }

    public float hpFraction() {
        return (float)health / (float)maxHealth;
    }


}
