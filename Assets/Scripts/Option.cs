﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public partial class Option
{
    //These (global) variables are looked up from GameManager.instance upon creation
    public GameManager gm;

    //Other variables
    public string description, shortened;
    public UnityAction onpress;  //Capitalization?
    public int rarity;
    public Conduit conduit;

    public OptionTree.OptionNode node;

    Option[] rewards;
    float[] rewardProbs;

    Option[] getChildren() {
        return node.getChildren();
    }

    public List<Option> getChildrenList() {
        if (node == null) return new List<Option>();
        else return node.getChildrenList();
    }

    public Option() {
        onpress = (() => {reinforce(true); });
        gm = GameManager.instance;
        rarity = 0;
        rewards = new Option[0] { };
    }

    //Lazy evaluation so that option tree can be static even when room is updating
    //Makes use of { get; set; } syntax in GameManager to have implicit get and set functions rather than accessing properties
    void setupStats(int monsterDmg, int natureDmg, int hpChange, int stamChange) {
        onpress += () => gm.player.updateStats(hpChange,  stamChange);
        onpress += () => gm.room.obstacle.assignDamage(monsterDmg, natureDmg);
    }

    public List<Option> randomRewards() {
        List<Option> onKill = new List<Option>();
        for (int i = 0; i < rewards.Length; i++) {
            if (Random.Range(0f, 1f) < rewardProbs[i])
                onKill.Add(rewards[i]);
        }
        return onKill;
    }

    public void reinforce(bool selected) {
        if (conduit == null) return;
        if (selected) conduit.reinforcement++;
        else conduit.reinforcement--;

        for (int i = 0; i < 3; i++)
            if (getChildren()[i] != null)
                getChildren()[i].conduit.incomingPower[2 - i] = conduit.getOutput(i); //Some redundancy here with multiple function calls.
            
        
    }


    

     

}
