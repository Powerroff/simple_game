using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Option
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

    public List<Option> getChildren() {
        return node.getChildren();
    }

    public Option() {
        onpress = (() => {; });
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


    // Prebuilt Options **********************
    
    /* Basic Option Tree
     * 
     *       O O
     *      /\ /\
     *     O  O  O
     *    /\ /|\ /\
     *   O  O O O  O
     */

    //Option 0-0
    public static Option hackSlash() {
        int monsterDmg = 2;
        int natureDmg = 1;

        Option o = new Option();
        o.description = string.Format("Hack and Slash\n\n{0} Damage to monsters\n {1} damage to nature", monsterDmg, natureDmg);
        o.shortened = "Hack n Slash";
        o.setupStats(-monsterDmg, -natureDmg, 0, 0);

        o.rewards = new Option[] { treatWounds(), takeShelter() };
        o.rewardProbs = new float[] { 0.75f, 0.5f };

        o.conduit = new Conduit.Generator(o, Conduit.newConnectors((int)Conduit.powerColors.red, (int)Conduit.powerColors.red, (int)Conduit.powerColors.red), (int)Conduit.powerColors.red);

        return o;
    }

    //Option 0-1
    public static Option harvest() {
        int monsterDmg = 1;
        int natureDmg = 2;

        Option o = new Option();
        o.description = string.Format("Harvest\n\n{0} Damage to monsters\n {1} damage to nature", monsterDmg, natureDmg);
        o.shortened = "Harvest";
        o.setupStats(-monsterDmg, -natureDmg, 0, 0);

        o.rewards = new Option[] { treatWounds(), takeShelter() };
        o.rewardProbs = new float[] { 0.5f, 0.75f };

        o.conduit = new Conduit.Generator(o, Conduit.newConnectors((int)Conduit.powerColors.green, (int)Conduit.powerColors.green, (int)Conduit.powerColors.green), (int)Conduit.powerColors.green);

        return o;
    }

    //Option 1-0
    public static Option savageSlash() {
        int monsterDmg = 3;
        int natureDmg = 1;

        Option o = new Option();
        o.description = string.Format("Savage Slash\n\n{0} Damage to monsters\n {1} damage to nature", monsterDmg, natureDmg);
        o.shortened = "Savage Slash";
        o.setupStats(-monsterDmg, -natureDmg, 0, 0);

        o.rewards = new Option[] { treatWounds(), takeShelter() };
        o.rewardProbs = new float[] { 0.75f, 0.5f };
        
        o.conduit = new Conduit(o, Conduit.newConnectors(-1, -1, (int)Conduit.powerColors.red), Conduit.newConnectors((int)Conduit.powerColors.red, (int)Conduit.powerColors.red, (int)Conduit.powerColors.red));

        return o;
    }

    //Option 1-1
    public static Option clearPath() {
        int monsterDmg = 2;
        int natureDmg = 2;

        Option o = new Option();
        o.description = string.Format("Clear a Path\n\n{0} Damage to monsters\n {1} damage to nature", monsterDmg, natureDmg);
        o.shortened = "Clear a Path";
        o.setupStats(-monsterDmg, -natureDmg, 0, 0);

        o.rewards = new Option[] { treatWounds(), takeShelter() };
        o.rewardProbs = new float[] { 0.75f, 0.75f };

        return o;
    }

    //Option 1-2
    public static Option skilledExploration() {
        int monsterDmg = 1;
        int natureDmg = 3;

        Option o = new Option();
        o.description = string.Format("Skilled Exploration\n\n{0} Damage to monsters\n {1} damage to nature", monsterDmg, natureDmg);
        o.shortened = "Exploration";
        o.setupStats(-monsterDmg, -natureDmg, 0, 0);

        o.rewards = new Option[] { treatWounds(), takeShelter() };
        o.rewardProbs = new float[] { 0.5f, 0.75f };

        return o;
    }

    //Option 2-0
    public static Option recklessAssault() {
        int monsterDmg = 5;
        int stamLoss = 1;

        Option o = new Option();
        o.description = string.Format("Reckless Assault\n\n{0} Damage to monsters\n Spend {1} extra stamina", monsterDmg, stamLoss);
        o.shortened = "Assault";
        o.setupStats(-monsterDmg, 0, 0, -stamLoss);

        o.rewards = new Option[] { treatWounds(), takeShelter() };
        o.rewardProbs = new float[] { 0.75f, 0.25f };

        return o;
    }

    //Option 2-1
    public static Option swiftKill() {
        int monsterDmg = 3;
        int natureDmg = 1;

        Option o = new Option();
        o.description = string.Format("Swift Kill\n\n{0} Damage to monsters\n {1} damage to nature", monsterDmg, natureDmg);
        o.shortened = "Swift Kill";
        o.setupStats(-monsterDmg, -natureDmg, 0, 0);

        o.rewards = new Option[] { treatWounds(), takeShelter() };
        o.rewardProbs = new float[] { 0.75f, 0.5f };

        return o;
    }

    //Option 2-2
    public static Option layLand() {
        int monsterDmg = 2;
        int natureDmg = 2;

        Option o = new Option();
        o.description = string.Format("Lay of the Land\n\n{0} Damage to monsters\n {1} damage to nature", monsterDmg, natureDmg);
        o.shortened = "Lay of the Land";
        o.setupStats(-monsterDmg, -natureDmg, 0, 0);

        o.rewards = new Option[] { treatWounds(), takeShelter(), investigateSurroundings() };
        o.rewardProbs = new float[] { 0.75f, 0.75f, .25f };

        return o;
    }

    //Option 2-3
    public static Option rangerTactics() {
        int monsterDmg = 1;
        int natureDmg = 3;

        Option o = new Option();
        o.description = string.Format("Ranger Tactics\n\n{0} Damage to monsters\n {1} damage to nature", monsterDmg, natureDmg);
        o.shortened = "Ranger Tactics";
        o.setupStats(-monsterDmg, -natureDmg, 0, 0);

        o.rewards = new Option[] { treatWounds(), takeShelter() };
        o.rewardProbs = new float[] { 0.5f, 0.75f };

        return o;
    }

    //Option 2-4
    public static Option conquerWilderness() {
        int natureDmg = 5;
        int stamLoss = 1;

        Option o = new Option();
        o.description = string.Format("Conquer the Wilderness\n\n{0} Damage to nature\n Spend {1} extra stamina", natureDmg, stamLoss);
        o.shortened = "Conquer";
        o.setupStats(0, -natureDmg, 0, -stamLoss);

        o.rewards = new Option[] { treatWounds(), takeShelter() };
        o.rewardProbs = new float[] { 0.25f, 0.75f };

        return o;
    }

    //Reward Options
    public static Option treatWounds() {
        int healthGain = 2;

        Option o = new Option();
        o.description = string.Format("Treat Wounds\n\nRecover {0} health", healthGain);
        o.setupStats(0, 0, healthGain, 0);
        return o;
    }

    public static Option takeShelter() {
        int stamGain = 2;

        Option o = new Option();
        o.description = string.Format("Take Shelter\n\nRecover {0} stamina", stamGain);
        o.setupStats(0, 0, 0, stamGain);
        return o;
    }

    public static Option investigateSurroundings() {
        Option o = new Option();
        o.description = string.Format("Investigate Surroundings");
        o.setupStats(0, 0, 0, 0);
        return o;
    }
    

    public static Option climbTree() {
        int stamGain = 5;

        Option o = new Option();
        o.description = string.Format("Climb Tree");

        o.setupStats(0, 0, 0, stamGain);
        o.rarity = 1;
        return o;
    }

    public static Option prepare() {
        int monsterDmg = 1;
        int natureDmg = 1;

        Option o = new Option();
        o.description = string.Format("Prepare \n\n Your next action does not spend stamina");

        o.rewards = new Option[] { treatWounds(), takeShelter(), investigateSurroundings() };
        o.rewardProbs = new float[] { 0.75f, 0.75f,  1f };
        o.setupStats(1, 1, 0, 0);
        o.rarity = 1;
        return o;
    }

    public static Option dualStrike() {
        int dmg = 4;

        Option o = new Option();
        o.description = string.Format("Dual Strike. Deal {0} to both types.", dmg);
        o.setupStats(-dmg, -dmg, 0, 0);

        o.rewards = new Option[] { treatWounds(), takeShelter() };
        o.rewardProbs = new float[] { 0.5f, 0.5f };

        return o;
    }

     

}
