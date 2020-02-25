using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Option
{
    //These (global) variables are looked up from GameManager.instance upon creation
    Player player;
    Obstacle obstacle;

    //Other variables
    public string description;
    public UnityAction onpress;  //Capitalization?
    public List<Option> nextOptions, onKill;
    public int rarity;



    public Option() {
        onpress = (() => {; });
        player = GameManager.instance.player;
        obstacle = GameManager.instance.room.obstacle;
        nextOptions = new List<Option>();
        onKill = new List<Option>();
        rarity = 0;
    }

    public Option(bool usesObstacle) {
        onpress = (() => {; });
        player = GameManager.instance.player;
        if (usesObstacle)
            obstacle = GameManager.instance.room.obstacle;
        nextOptions = new List<Option>();
        onKill = new List<Option>();
        rarity = 0;
    }

    void setupStats(int monsterDmg, int natureDmg, int hpChange, int stamChange, int strChange) {
        if (obstacle.obstacleClass == Obstacle.ObstacleClass.Monster) {            //Maybe could be reworked a bit
            onpress += obstacle.changeHp(monsterDmg);
        } else if (obstacle.obstacleClass == Obstacle.ObstacleClass.Nature) {
            onpress += obstacle.changeHp(natureDmg);
        }

        onpress += player.changeHp(hpChange);
        onpress += player.changeStam(stamChange);
        onpress += player.changeStr(strChange);
    }

    void randomRewards(Option[] rewards, float[] probs) {
        for (int i = 0; i < rewards.Length; i++) {
            if (Random.Range(0f, 1f) < probs[i])
                onKill.Add(rewards[i]);
        }
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
        o.setupStats(-monsterDmg, -natureDmg, 0, 0, 0);

        o.nextOptions.AddRange(new Option[] { savageSlash(), clearPath() });
        o.randomRewards(new Option[] { treatWounds(), takeShelter() }, new float[] { 0.75f, 0.5f });

        return o;
    }

    //Option 0-1
    public static Option harvest() {
        int monsterDmg = 1;
        int natureDmg = 2;

        Option o = new Option();
        o.description = string.Format("Harvest\n\n{0} Damage to monsters\n {1} damage to nature", monsterDmg, natureDmg);
        o.setupStats(-monsterDmg, -natureDmg, 0, 0, 0);

        o.nextOptions = new List<Option>(new Option[] { clearPath(), skilledExploration() });
        o.randomRewards(new Option[] { treatWounds(), takeShelter() }, new float[] { 0.5f, 0.75f });

        return o;
    }

    //Option 1-0
    public static Option savageSlash() {
        int monsterDmg = 3;
        int natureDmg = 1;

        Option o = new Option();
        o.description = string.Format("Savage Slash\n\n{0} Damage to monsters\n {1} damage to nature", monsterDmg, natureDmg);
        o.setupStats(-monsterDmg, -natureDmg, 0, 0, 0);

        o.nextOptions.AddRange(new Option[] { recklessAssault(), swiftKill() });
        o.randomRewards(new Option[] { treatWounds(), takeShelter() }, new float[] { 0.75f, 0.5f });

        return o;
    }

    //Option 1-1
    public static Option clearPath() {
        int monsterDmg = 2;
        int natureDmg = 2;

        Option o = new Option();
        o.description = string.Format("Clear a Path\n\n{0} Damage to monsters\n {1} damage to nature", monsterDmg, natureDmg);
        o.setupStats(-monsterDmg, -natureDmg, 0, 0, 0);

        o.nextOptions.AddRange(new Option[] { swiftKill(), layLand(), rangerTactics() });
        o.randomRewards(new Option[] { treatWounds(), takeShelter() }, new float[] { 0.75f, 0.75f });

        return o;
    }

    //Option 1-2
    public static Option skilledExploration() {
        int monsterDmg = 1;
        int natureDmg = 3;

        Option o = new Option();
        o.description = string.Format("Skilled Exploration\n\n{0} Damage to monsters\n {1} damage to nature", monsterDmg, natureDmg);
        o.setupStats(-monsterDmg, -natureDmg, 0, 0, 0);

        o.nextOptions.AddRange(new Option[] { layLand(), rangerTactics(), conquerWilderness() });
        o.randomRewards(new Option[] { treatWounds(), takeShelter() }, new float[] { 0.5f, 0.75f });

        return o;
    }

    //Option 2-0
    public static Option recklessAssault() {
        int monsterDmg = 5;
        int stamLoss = 1;

        Option o = new Option();
        o.description = string.Format("Reckless Assault\n\n{0} Damage to monsters\n Spend {1} extra stamina", monsterDmg, stamLoss);
        o.setupStats(-monsterDmg, 0, 0, -stamLoss, 0);

        o.randomRewards(new Option[] { treatWounds(), takeShelter() }, new float[] { 0.75f, 0.25f });

        return o;
    }

    //Option 2-1
    public static Option swiftKill() {
        int monsterDmg = 3;
        int natureDmg = 1;

        Option o = new Option();
        o.description = string.Format("Swift Kill\n\n{0} Damage to monsters\n {1} damage to nature", monsterDmg, natureDmg);
        o.setupStats(-monsterDmg, -natureDmg, 0, 0, 0);

        o.randomRewards(new Option[] { treatWounds(), takeShelter() }, new float[] { 0.75f, 0.5f });

        return o;
    }

    //Option 2-2
    public static Option layLand() {
        int monsterDmg = 2;
        int natureDmg = 2;

        Option o = new Option();
        o.description = string.Format("Lay of the Land\n\n{0} Damage to monsters\n {1} damage to nature", monsterDmg, natureDmg);
        o.setupStats(-monsterDmg, -natureDmg, 0, 0, 0);

        o.randomRewards(new Option[] { treatWounds(), takeShelter(), investigateSurroundings() }, new float[] { 0.75f, 0.75f, .25f });

        return o;
    }

    //Option 2-3
    public static Option rangerTactics() {
        int monsterDmg = 1;
        int natureDmg = 3;

        Option o = new Option();
        o.description = string.Format("Ranger Tactics\n\n{0} Damage to monsters\n {1} damage to nature", monsterDmg, natureDmg);
        o.setupStats(-monsterDmg, -natureDmg, 0, 0, 0);

        o.randomRewards(new Option[] { treatWounds(), takeShelter() }, new float[] { 0.5f, 0.75f });

        return o;
    }

    //Option 2-4
    public static Option conquerWilderness() {
        int natureDmg = 5;
        int stamLoss = 1;

        Option o = new Option();
        o.description = string.Format("Conquer the Wilderness\n\n{0} Damage to nature\n Spend {1} extra stamina", natureDmg, stamLoss);
        o.setupStats(0, -natureDmg, 0, -stamLoss, 0);

        o.randomRewards(new Option[] { treatWounds(), takeShelter() }, new float[] { 0.25f, 0.75f });

        return o;
    }

    //Reward Options
    public static Option treatWounds() {
        int healthGain = 2;

        Option o = new Option();
        o.description = string.Format("Treat Wounds\n\nRecover {0} health", healthGain);
        o.setupStats(0, 0, healthGain, 0, 0);
        return o;
    }

    public static Option takeShelter() {
        int stamGain = 2;

        Option o = new Option();
        o.description = string.Format("Take Shelter\n\nRecover {0} stamina", stamGain);
        o.setupStats(0, 0, 0, stamGain, 0);
        return o;
    }

    public static Option investigateSurroundings() {
        Option o = new Option();
        o.description = string.Format("Investigate Surroundings");
        o.setupStats(0, 0, 0, 0, 0);
        return o;
    }
    

    //REFACTOR EVERYTHING INVOLVED HERE PLEASE
    public static Option climbTree() {
        int stamGain = 5;

        Option o = new Option(false);
        o.description = string.Format("Climb Tree");

        o.onpress = o.player.changeStam(stamGain);
        o.rarity = 1;
        return o;
    }

    
}
