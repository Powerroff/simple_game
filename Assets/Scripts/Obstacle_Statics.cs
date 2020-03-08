using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public partial class Obstacle
{
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
        o.unCleared = () => GameManager.instance.player.updateStats(-o.health, 0);
        o.description = string.Format("Monster.  \n If not cleared, deals remaining health as damage.");
        o.name = "Monkey";
        return o;
    }

    public static Obstacle[] defaultPackage() {
        return new Obstacle[] { underbrush(), tree(), hound(), monkey() };
    }

    public static Obstacle boss1() {
        Obstacle o = new Obstacle();
        o.health = UnityEngine.Random.Range(20, 25);
        o.obstacleClass = ObstacleClass.Monster;
        o.unCleared = () => GameManager.instance.player.updateStats(-o.health, 0);
        o.description = string.Format("Monster.  \n If not cleared, deals remaining health as damage.");
        o.name = "Boss Monkey";
        return o;
    }




}
