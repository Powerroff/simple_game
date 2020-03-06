using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public partial class Option
{
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
        int bonusDmg = 2;

        Option o = new Option();
        o.description = string.Format("Hack and Slash\n\n{0} Damage to monsters\n {1} damage to nature", monsterDmg, natureDmg);
        o.descriptionPow = string.Format("Hack and Slash (Powered)\n\n{0} Damage to monsters\n {1} damage to nature\n Your next unpowered attack this room does {2} more damage", monsterDmg, natureDmg, bonusDmg) ;
        o.shortened = "Hack n Slash";

        //Base Action
        o.defaultCons = new Consequence(0, -1, -monsterDmg, -natureDmg, null, o.description);

        //Powered Action
        o.powerCons = o.defaultCons.clone();
        o.powerCons.specialAction += () => o.fm.modifyNextOptionIf(opt => !opt.isPowered(), opt => { opt.consequence.monsterDmg -= 2; opt.consequence.natureDmg -= 2; }, true);
         
        //Rewards
        o.rewards = new Option[] { treatWounds(), takeShelter() };
        o.rewardProbs = new float[] { 0.75f, 0.5f };

        //Conduit
        Conduit.connector[] bottomConnections = Conduit.newConnectors((int)Conduit.powerColors.red, (int)Conduit.powerColors.red, (int)Conduit.powerColors.red);
        int powerColor = (int)Conduit.powerColors.red;
        o.conduit = new Conduit.Generator(o, bottomConnections, powerColor);

        return o;
    }

    //Option 0-1
    public static Option harvest() {
        int monsterDmg = 1;
        int natureDmg = 2;

        Option o = new Option();
        o.description = string.Format("Harvest\n\n{0} Damage to monsters\n {1} damage to nature", monsterDmg, natureDmg);
        o.shortened = "Harvest";

        //Base Action
        o.defaultCons = new Consequence(0, -1, -monsterDmg, -natureDmg, null, o.description);

        //Powered Action
        o.powerCons = o.defaultCons.clone();

        o.rewards = new Option[] { treatWounds(), takeShelter() };
        o.rewardProbs = new float[] { 0.5f, 0.75f };

        Conduit.connector[] bottomConnections = Conduit.newConnectors((int)Conduit.powerColors.green, (int)Conduit.powerColors.green, (int)Conduit.powerColors.green);
        int powerColor = (int)Conduit.powerColors.green;
        o.conduit = new Conduit.Generator(o, bottomConnections, powerColor);

        return o;
    }

    //Option 1-0
    public static Option savageSlash() {
        int monsterDmg = 3;
        int natureDmg = 1;

        Option o = new Option();
        o.description = string.Format("Savage Slash\n\n{0} Damage to monsters\n {1} damage to nature", monsterDmg, natureDmg);
        o.shortened = "Savage Slash";

        //Base Action
        o.defaultCons = new Consequence(0, -1, -monsterDmg, -natureDmg, null, o.description);

        //Powered Action
        o.powerCons = o.defaultCons.clone(); 

         o.rewards = new Option[] { treatWounds(), takeShelter() };
        o.rewardProbs = new float[] { 0.75f, 0.5f };

        Conduit.connector[] topConnections = Conduit.newConnectors(-1, -1, (int)Conduit.powerColors.red);
        Conduit.connector[] bottomConnections = Conduit.newConnectors((int)Conduit.powerColors.red, (int)Conduit.powerColors.red, (int)Conduit.powerColors.red);
        o.conduit = new Conduit(o, topConnections, bottomConnections);

        return o;
    }

    //Option 1-1
    public static Option clearPath() {
        int monsterDmg = 2;
        int natureDmg = 2;

        Option o = new Option();
        o.description = string.Format("Clear a Path\n\n{0} Damage to monsters\n {1} damage to nature", monsterDmg, natureDmg);
        o.shortened = "Clear a Path";

        //Base Action
        o.defaultCons = new Consequence(0, -1, -monsterDmg, -natureDmg, null, o.description);

        //Powered Action
        o.powerCons = o.defaultCons.clone();

        o.rewards = new Option[] { treatWounds(), takeShelter() };
        o.rewardProbs = new float[] { 0.75f, 0.75f };

        //init conduit
        Conduit.connector[] topConnections = Conduit.newConnectors((int)Conduit.powerColors.rainbow, (int)Conduit.powerColors.rainbow, (int)Conduit.powerColors.rainbow);
        Conduit.connector[] bottomConnections = Conduit.newConnectors(-1, (int)Conduit.powerColors.yellow, -1);
        int outputColor = (int)Conduit.powerColors.yellow;
        int[] requriedInputs = new int[] { (int)Conduit.powerColors.red, (int)Conduit.powerColors.green };
        o.conduit = new Conduit.Reactor(o, topConnections, bottomConnections, outputColor, requriedInputs);

        return o;
    }

    //Option 1-2
    public static Option skilledExploration() {
        int monsterDmg = 1;
        int natureDmg = 3;

        Option o = new Option();
        o.description = string.Format("Skilled Exploration\n\n{0} Damage to monsters\n {1} damage to nature", monsterDmg, natureDmg);
        o.shortened = "Exploration";

        //Base Action
        o.defaultCons = new Consequence(0, -1, -monsterDmg, -natureDmg, null, o.description);

        //Powered Action
        o.powerCons = o.defaultCons.clone();

        o.rewards = new Option[] { treatWounds(), takeShelter() };
        o.rewardProbs = new float[] { 0.5f, 0.75f };

        Conduit.connector[] topConnections = Conduit.newConnectors((int)Conduit.powerColors.green, -1, -1);
        Conduit.connector[] bottomConnections = Conduit.newConnectors((int)Conduit.powerColors.green, (int)Conduit.powerColors.green, (int)Conduit.powerColors.green);
        o.conduit = new Conduit(o, topConnections, bottomConnections);

        return o;
    }

    //Option 2-0
    public static Option recklessAssault() {
        int monsterDmg = 5;
        int stamLoss = 1;

        Option o = new Option();
        o.description = string.Format("Reckless Assault\n\n{0} Damage to monsters\n Spend {1} extra stamina", monsterDmg, stamLoss);
        o.shortened = "Assault";

        //Base Action
        o.defaultCons = new Consequence(0, -1-stamLoss, -monsterDmg, 0, null, o.description);

        //Powered Action
        o.powerCons = o.defaultCons.clone();

        o.rewards = new Option[] { treatWounds(), takeShelter() };
        o.rewardProbs = new float[] { 0.75f, 0.25f };

        Conduit.connector[] topConnections = Conduit.newConnectors(-1, -1, (int)Conduit.powerColors.red);
        Conduit.connector[] bottomConnections = new Conduit.connector[3];
        o.conduit = new Conduit(o, topConnections, bottomConnections);

        return o;
    }

    //Option 2-1
    public static Option swiftKill() {
        int monsterDmg = 3;
        int natureDmg = 1;

        Option o = new Option();
        o.description = string.Format("Swift Kill\n\n{0} Damage to monsters\n {1} damage to nature", monsterDmg, natureDmg);
        o.shortened = "Swift Kill";

        //Base Action
        o.defaultCons = new Consequence(0, -1, -monsterDmg, -natureDmg, null, o.description);

        //Powered Action
        o.powerCons = o.defaultCons.clone();

        o.rewards = new Option[] { treatWounds(), takeShelter() };
        o.rewardProbs = new float[] { 0.75f, 0.5f };

        Conduit.connector[] topConnections = Conduit.newConnectors((int)Conduit.powerColors.red, -1, -1);
        Conduit.connector[] bottomConnections = new Conduit.connector[3];
        o.conduit = new Conduit(o, topConnections, bottomConnections);

        return o;
    }

    //Option 2-2
    public static Option layLand() {
        int monsterDmg = 2;
        int natureDmg = 2;

        Option o = new Option();
        o.description = string.Format("Lay of the Land\n\n{0} Damage to monsters\n {1} damage to nature", monsterDmg, natureDmg);
        o.shortened = "Lay of the Land";

        //Base Action
        o.defaultCons = new Consequence(0, -1, -monsterDmg, -natureDmg, null, o.description);

        //Powered Action
        o.powerCons = o.defaultCons.clone();

        o.rewards = new Option[] { treatWounds(), takeShelter(), investigateSurroundings() };
        o.rewardProbs = new float[] { 0.75f, 0.75f, .25f };

        Conduit.connector[] topConnections = Conduit.newConnectors(-1, (int)Conduit.powerColors.yellow, -1);
        Conduit.connector[] bottomConnections = new Conduit.connector[3];
        o.conduit = new Conduit(o, topConnections, bottomConnections);

        return o;
    }

    //Option 2-3
    public static Option rangerTactics() {
        int monsterDmg = 1;
        int natureDmg = 3;

        Option o = new Option();
        o.description = string.Format("Ranger Tactics\n\n{0} Damage to monsters\n {1} damage to nature", monsterDmg, natureDmg);
        o.shortened = "Ranger Tactics";

        //Base Action
        o.defaultCons = new Consequence(0, -1, -monsterDmg, -natureDmg, null, o.description);

        //Powered Action
        o.powerCons = o.defaultCons.clone();

        o.rewards = new Option[] { treatWounds(), takeShelter() };
        o.rewardProbs = new float[] { 0.5f, 0.75f };
        Conduit.connector[] topConnections = Conduit.newConnectors(-1, -1, (int)Conduit.powerColors.green);
        Conduit.connector[] bottomConnections = new Conduit.connector[3];
        o.conduit = new Conduit(o, topConnections, bottomConnections);


        return o;
    }

    //Option 2-4
    public static Option conquerWilderness() {
        int natureDmg = 5;
        int stamLoss = 1;

        Option o = new Option();
        o.description = string.Format("Conquer the Wilderness\n\n{0} Damage to nature\n Spend {1} extra stamina", natureDmg, stamLoss);
        o.shortened = "Conquer";

        //Base Action
        o.defaultCons = new Consequence(0, -1-stamLoss, 0, -natureDmg, null, o.description);

        //Powered Action
        o.powerCons = o.defaultCons.clone();

        o.rewards = new Option[] { treatWounds(), takeShelter() };
        o.rewardProbs = new float[] { 0.25f, 0.75f };

        Conduit.connector[] topConnections = Conduit.newConnectors((int)Conduit.powerColors.green, -1, -1);
        Conduit.connector[] bottomConnections = new Conduit.connector[3];
        o.conduit = new Conduit(o, topConnections, bottomConnections);

        return o;
    }

    //Reward Options
    public static Option treatWounds() {
        int healthGain = 2;

        Option o = new Option();
        o.description = string.Format("Treat Wounds\n\nRecover {0} health", healthGain);

        //Base Action
        o.defaultCons = new Consequence(healthGain, 0, 0, 0, null, o.description);

        return o;
    }

    public static Option takeShelter() {
        int stamGain = 2;

        Option o = new Option();
        o.description = string.Format("Take Shelter\n\nRecover {0} stamina", stamGain);

        //Base Action
        o.defaultCons = new Consequence(0, stamGain, 0, 0, null, o.description);


        return o;
    }

    public static Option investigateSurroundings() {
        Option o = new Option();
        o.description = string.Format("Investigate Surroundings");
        
        return o;
    }


    public static Option climbTree() {
        int stamGain = 5;

        Option o = new Option();
        o.description = string.Format("Climb Tree");

        //Base Action
        o.defaultCons = new Consequence(0, stamGain, 0, 0, null, o.description);

        o.rarity = 1;
        return o;
    }

    public static Option prepare() {
        int monsterDmg = 1;
        int natureDmg = 1;

        Option o = new Option();
        o.description = string.Format("Prepare \n\n Your next action does not spend stamina");

        o.rewards = new Option[] { treatWounds(), takeShelter(), investigateSurroundings() };
        o.rewardProbs = new float[] { 0.75f, 0.75f, 1f };

        //Base Action
        o.defaultCons = new Consequence(0, -1, -monsterDmg, -natureDmg, null, o.description);
        o.rarity = 1;
        return o;
    }

    public static Option dualStrike() {
        int dmg = 4;

        Option o = new Option();
        o.description = string.Format("Dual Strike. Deal {0} to both types.", dmg);

        //Base Action
        o.defaultCons = new Consequence(0, -1, -dmg, -dmg, null, o.description);

        o.rewards = new Option[] { treatWounds(), takeShelter() };
        o.rewardProbs = new float[] { 0.5f, 0.5f };

        return o;
    }
}
