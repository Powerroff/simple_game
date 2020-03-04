using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Obstacle
{
    //Useful Macros for the class
    public enum ObstacleClass { Nature, Monster };
    readonly string[] monsterPrefixes = { "", "Feral ", "Cunning ", "Big ", "Warped ", "Gigantic " };
    readonly string[] naturePrefixes = { "", "Overgrown ", "Sinister ", "Evolved ", "Carnivorous ", "Possessed " };

    //These (global) variables will be looked up from GameManager.instance upon creation
    GameManager gm;

    //These variables will be specific to the obstacle
    public int health;
    public ObstacleClass obstacleClass;
    public Action unCleared, cleared;
    public string name, description;
    public Option uniqueOption;


    //Constructor and methods
    public Obstacle() {
        
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








    // Prebuilt Options
    public static Obstacle underbrush() {
        Obstacle o = new Obstacle();
        o.health = UnityEngine.Random.Range(4, 8);
        o.obstacleClass = ObstacleClass.Nature;
        o.unCleared = () => GameManager.instance.player.updateStats(0, -o.health);        
        o.description = string.Format("Natural.  \n If not cleared, deals remaining health as stamina damage.");        
        o.name = "Underbrush";
        return o;
    }

    public static Obstacle tree() {
        Obstacle o = new Obstacle();
        o.health = UnityEngine.Random.Range(6, 12);
        o.obstacleClass = ObstacleClass.Nature;
        o.unCleared = () => GameManager.instance.player.updateStats(0, -o.health);
        o.uniqueOption = Option.climbTree();
        o.description = string.Format("Natural.  \n If not cleared, deals remaining health as stamina damage.");
        o.name = "Tree";
        return o;
    }

    public static Obstacle hound() {
        Obstacle o = new Obstacle();
        o.health = UnityEngine.Random.Range(4, 10);
        o.obstacleClass = ObstacleClass.Monster;
        o.unCleared = () => GameManager.instance.player.updateStats(-o.health, 0);
        o.description = string.Format("Monster.  \n If not cleared, deals remaining health as damage.");
        o.name = "Hound";
        return o;
    }

    public static Obstacle monkey() {
        Obstacle o = new Obstacle();
        o.health = UnityEngine.Random.Range(7, 13);
        o.obstacleClass = ObstacleClass.Monster;
        o.unCleared = () => GameManager.instance.player.updateStats( -o.health, 0);
        o.description = string.Format("Monster.  \n If not cleared, deals remaining health as damage.");
        o.name = "Monkey";
        return o;
    }

    public static Obstacle[] defaultPackage() {
        return new Obstacle[] { underbrush(), tree(), hound(), monkey() };
    }
    
    /*
    public static Obstacle tempA() {
        Obstacle o = new Obstacle();
        int level = o.roomCount / 5;
        o.health = 1 + level;
        o.obstacleClass = ObstacleClass.Nature;
        //o.unCleared = o.player.changeStam(-level);
        //o.cleared = (() => {; });
        o.description = string.Format("Natural.  \n If not cleared, -{0} stamina.", level + 1);
        o.name = o.getPrefix() + "Underbrush";
        return o;
    }
    */




}
