﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    
    
    //Static instance of GameManager which allows it to be accessed by any other script.
    public static GameManager instance = null;
    void Awake() {
        Debug.Log("awoke");

        //Check if instance already exists
        if (instance == null) {
            Debug.Log("Instantiating gm");
            //if not, set instance to this
            instance = this;
        }

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, 
            // meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene / Switching scenes
        DontDestroyOnLoad(gameObject); // VERY IMPORTANT

    }
    //^^ Above section creates a static GameManager

    public Room room;
    public UIManager uim;
    public Player player;
    public int roomCount;
    public bool[] optionsSelected;
    public string consequences;
    // Start is called before the first frame update
    void Start()
    {
        roomCount = 0;

        optionsSelected = new bool[2];

        uim.init();
        nextRoom();

    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public void nextRoom() {
        //Process old room
        if (room) {
            processObstacle();

            //Pickup relic
            if (room.relic != null) room.relic.onPickup.Invoke();
        }

        Room tempRoom = gameObject.AddComponent<Room>() as Room;
        if(room) Destroy(room);
        room = tempRoom;

        //Increment roomCount
        roomCount++;

        //Generate Relic **TEMP**
        //if (roomCount == 10) room.relic = Relic.tempRelicOne();

        //Generate Obstacle
        Obstacle o = Obstacle.tempPackage()[Random.Range(0, Obstacle.tempPackage().Length)];
        room.obstacle = o;
        

        //Generate Options
        room.options = player.optionTree.getFirstLayer();

        //Generate Background Color
        room.backgroundColor = Random.ColorHSV(0f, .5f, .5f, .5f, 0.5f, 1f);  // I've limited the background colors to the lighter half of the spectrum.

        uim.nextRoom();


    }

    public void takeAction() {

        if (room.options.Count == 0) {
            Invoke("nextRoom", .5f);
            return;
        }

        StatsManager oldStats = player.stats.clone();
        int obstacleHp = room.obstacle.health;
        nextOptions(processOption()); //hmm
        generateConsequenceStr(oldStats, obstacleHp);
        uim.displayConsequence();

        if (room.options.Count == 0)
            uim.endRoom();
        else
            uim.Invoke("newOPM", .5f);

    }

    //Jank
    public void generateConsequenceStr(StatsManager oldStats, int obstacleHp) {
        consequences = "";

        int deltaStam = player.stats.stamina - oldStats.stamina;
        string dStam = (deltaStam >= 0 ? "+" : "") + deltaStam;
        int deltaHp = player.stats.hp - oldStats.hp;
        string dHp = (deltaHp >= 0 ? "+" : "") + deltaHp;
        int deltaObsHp = room.obstacle.health - obstacleHp;
        string dObsHp = (deltaObsHp >= 0 ? "+" : "") + deltaObsHp;

        consequences += "Obstacle " + dObsHp + " health. Player "  + dStam + " stamina, " + dHp + " hp.";
    }


    public List<OptionTree.OptionNode> processOption() {
        player.stats.stamina--;
        List<OptionTree.OptionNode> selected = new List<OptionTree.OptionNode>();
        for (int i = 0; i < room.options.Count; i++) {
            if (uim.opm.optionsSelected[i]) {
                room.options[i].option.onpress();
                selected.Add(room.options[i]);
            }
        }
        player.stats.stamina -= System.Math.Max(selected.Count - 1, 0);
        return selected;
    }

    public void nextOptions(List<OptionTree.OptionNode> selected) {
        room.options = new List<OptionTree.OptionNode>();
        foreach (OptionTree.OptionNode node in selected) {
            if (room.obstacle.health > 0) {
                room.options.AddRange(player.optionTree.getChildren(node));
            } else {
                foreach (Option o in node.option.onKill) {
                    room.options.Add(new OptionTree.OptionNode(0, null, o));
                }
                
            }
        if (room.options.Count > 0 && room.obstacle.uniqueOption != null) {
                room.options.Add(new OptionTree.OptionNode(0, null, room.obstacle.uniqueOption));
            }
        }
    }


    void processObstacle() {
        if (room)
            if (room.obstacle.health > 0) {
                room.obstacle.unCleared.Invoke();
            } else {
                room.obstacle.cleared.Invoke();
            }
    }

    public Player getPlayer() {
        return player;
    }

    public Obstacle getObstacle() {
        return room.obstacle;
    }



}
