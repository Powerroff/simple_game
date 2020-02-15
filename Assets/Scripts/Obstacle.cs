﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Obstacle
{
    public static Player player;

    public enum ObstacleClass
    {
        Nature = 0,
        Monster = 1
    }

    public int health;
    public ObstacleClass obstacleClass;
    public UnityAction unCleared;
    public UnityAction cleared;

    public string name;
    public string description;

    public Obstacle() {

    }

    public UnityAction changeHp(int amount) {
        return (() => { health += amount; });
    }








    // Prebuilt Options



    public static Obstacle tempA() {
        Obstacle o = new Obstacle();
        o.name = "Underbrush";
        o.description = "Natural. \n Health 1 \n If not cleared, -1 stamina.";
        o.health = 1;
        o.obstacleClass = ObstacleClass.Nature;
        o.unCleared = (() => { player.stats.stamina--; });
        o.cleared = (() => {; }) ;
        return o;
    }

    public static Obstacle tempB() {
        Obstacle o = new Obstacle();
        o.name = "Overgrown Tree";
        o.description = "Natural.  \n Health 2 \n If cleared, +1 stamina.";
        o.health = 2;
        o.obstacleClass = ObstacleClass.Nature;
        o.unCleared = (() => {; });
        o.cleared = (() => { player.stats.stamina++; });
        return o;
    }

    public static Obstacle tempC() {
        Obstacle o = new Obstacle();
        o.name = "Feral Hound";
        o.description = "Monster.  \n Health 1 \n If not killed, -1 health.";
        o.health = 1;
        o.obstacleClass = ObstacleClass.Monster;
        o.unCleared = (() => { player.stats.hp--; });
        o.cleared = (() => {; });
        return o;
    }

    public static Obstacle tempD() {
        Obstacle o = new Obstacle();
        o.name = "Monkey";
        o.description = "Monster.  \n Health 2 \n If cleared, +1 health.";
        o.health = 2;
        o.obstacleClass = ObstacleClass.Monster;
        o.unCleared = (() => {; });
        o.cleared = (() => { player.stats.hp++; });
        return o;
    }

    public static Obstacle[] tempPackage() {
        return new Obstacle[] { tempA(), tempB(), tempC(), tempD() };
    }




}