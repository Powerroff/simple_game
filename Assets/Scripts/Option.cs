using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public partial class Option
{
    //These (global) variables are looked up from GameManager.instance upon creation
    GameManager gm;
    FlagManager fm;

    //Other variables
    public string description, descriptionPow, shortened;
    public Action onpress, onPoweredPress; //Capitalization?
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
        onpress = (() => {if (isReceivingPower()) reinforce(true);});
        onPoweredPress = () => {; };
        gm = GameManager.instance;
        fm = gm.fm;
        rarity = 0;
        rewards = new Option[0] { };
    }

    //Lazy evaluation so that option tree can be static even when room is updating
    //Makes use of { get; set; } syntax in GameManager to have implicit get and set functions rather than accessing properties

    void setupStats(ref Action action, int monsterDmg, int natureDmg, int hpChange, int stamChange) {
        action += () => { gm.player.updateStats(hpChange, stamChange); };
        action += () => { gm.room.obstacle.assignDamage(monsterDmg, natureDmg); };
    }

    void setupStats(int monsterDmg, int natureDmg, int hpChange, int stamChange) {
        setupStats(ref onpress, monsterDmg, natureDmg, hpChange, stamChange);
        setupStats(ref onPoweredPress, monsterDmg, natureDmg, hpChange, stamChange);
    }

    public List<Option> randomRewards() {
        List<Option> onKill = new List<Option>();
        for (int i = 0; i < rewards.Length; i++) {
            if (UnityEngine.Random.Range(0f, 1f) < rewardProbs[i])
                onKill.Add(rewards[i]);
        }
        return onKill;
    }

    public void reinforce(bool selected) {
        if (conduit == null) return;
        if (!selected) conduit.reinforcement--;
        else {
            if (conduit.reinforcement == conduit.max_reinforcement - 1) 
                foreach (Option option in gm.room.options)
                    if (option.isPowered()) option.conduit.resetPower(); //Resets all other powered conduit options. This will eventually need to be reworked, I think. Want to power connectors instead of conduits maybe?
            conduit.reinforcement++;
        }

        for (int i = 0; i < 3; i++)
            if (getChildren()[i] != null)
                getChildren()[i].conduit.incomingPower[2 - i] = conduit.getOutput(i); //Some redundancy here with multiple function calls.
            
        
    }

    bool isPowered() {
        if (conduit != null)
            return conduit.isPowered();
        return false;
    }

    bool isReceivingPower() {
        if (conduit != null)
            return conduit.isReceivingPower();
        return false;
    }

    public string getDescription() {
        if (isPowered() && descriptionPow != null) return descriptionPow;
        else return description;
    }

    public void onPress() {
        if (!isPowered()) onpress();
        else onPoweredPress();
    }
    

     

}
