﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameManager gm;
    public Button nextRoomButton, leftOption, rightOption;
    public Image background;
    public Player player;


    // Start is called before the first frame update
    void Start()
    {
        leftOption.onClick.AddListener(() => { player.stats.strength += 2; });
        nextRoomButton.onClick.AddListener(() => { player.stats.stamina -= 1; });
        nextRoomButton.onClick.AddListener(() => { print(player.stats.strength); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void nextRoom() {
        leftOption.GetComponent<OptionManager>().resetListeners();
        rightOption.GetComponent<OptionManager>().resetListeners();

        gm.nextRoom();
        setOptionsEnabled(true);


    }

    public void updateBackground(Room room) {
        background.color = room.backgroundColor;
        print(room.backgroundColor);
    }



    public void setOptionsEnabled(bool enabled) {
        leftOption.interactable = enabled;
        rightOption.interactable = enabled;
        print("asdf");
    }

    
}