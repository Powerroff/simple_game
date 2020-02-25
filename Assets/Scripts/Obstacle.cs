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
    Player player;
    int roomCount;

    //These variables will be specific to the obstacle
    public int health;
    public ObstacleClass obstacleClass;
    public UnityAction unCleared, cleared;
    public string name, description;
    public Option uniqueOption; //JANK. Can't invoke the method before the obstacle is generated so...


    //Constructor and methods
    public Obstacle() {
        //Look up the global variables
        player = GameManager.instance.player;
        roomCount = GameManager.instance.roomCount;
        uniqueOption = null;
    }

    string getPrefix() {
        if (obstacleClass == ObstacleClass.Nature) return naturePrefixes[Math.Min(naturePrefixes.Length - 1, roomCount / 10)];
        if (obstacleClass == ObstacleClass.Monster) return monsterPrefixes[Math.Min(monsterPrefixes.Length - 1, roomCount / 10)];
        return "";
    }

    public UnityAction changeHp(int amount) {
        return (() => { health = Math.Max(health + amount, 0); });
    }







    // Prebuilt Options



    public static Obstacle tempA() {
        Obstacle o = new Obstacle();
        int level = o.roomCount / 5;
        o.health = 1 + level;
        o.obstacleClass = ObstacleClass.Nature;
        o.unCleared = o.player.changeStam(-level);
        o.cleared = (() => {; });
        o.description = string.Format("Natural.  \n If not cleared, -{0} stamina.", level + 1);
        o.name = o.getPrefix() + "Underbrush";
        return o;
    }

    public static Obstacle tempB() {
        Obstacle o = new Obstacle();
        o.health = 2 + o.roomCount / 5;
        o.obstacleClass = ObstacleClass.Nature;
        o.unCleared = (() => {; });
        o.cleared = o.player.changeStam(1);
        o.uniqueOption = Option.climbTree();
        o.description = string.Format("Natural.  \n If cleared, +1 stamina.");
        o.name = o.getPrefix() + "Tree";
        return o;
    }

    public static Obstacle tempC() {
        Obstacle o = new Obstacle();
        int level = o.roomCount / 5;
        o.health = 1 + level;
        o.obstacleClass = ObstacleClass.Monster;
        o.unCleared = o.player.changeHp(-level-1);
        o.cleared = (() => {; });
        o.description = string.Format("Monster.  \n If not killed, -{0} health.", 1+level);
        o.name = o.getPrefix() + "Hound";
        return o;
    }

    public static Obstacle tempD() {
        Obstacle o = new Obstacle();
        int level = o.roomCount / 5;
        o.health = 2 + level;
        o.obstacleClass = ObstacleClass.Monster;
        o.unCleared = (() => {; });
        o.cleared = o.player.changeHp(1);
        o.description = string.Format("Monster.  \n If killed, +1 health.");
        o.name = o.getPrefix() + "Monkey";
        return o;
    }

    public static Obstacle[] tempPackage() {
        return new Obstacle[] { tempA(), tempB(), tempC(), tempD() };
    }




}
