using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Maybe will eventually include a static instance for this; right now there's no need

    GameManager gm; //Including this here for convenience only.

    //UI Elements
    public Button nextRoomButton, leftOption, rightOption;
    public Image background;
    public Text hpText;
    public Text stamText;
    public Text strText;
    public Text obsName;
    public Text obsDescription;



    // Start is called before the first frame update
    void Start() {

    }

    public void init()
    {
        gm = GameManager.instance;
        UpdateStatText();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        if (Input.GetKeyDown(KeyCode.Space)) nextRoom();
        if (Input.GetKeyDown(KeyCode.Alpha1)) selectLeftOption();
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectRightOption();
    }

    void UpdateStatText() {
        hpText.text = "HP: " + gm.player.stats.hp;
        stamText.text = "Stamina: " + gm.player.stats.stamina;
        strText.text = "Strength: " + gm.player.stats.strength;
    }


    public void nextRoom() {
        
        //GM loads the next room
        gm.nextRoom();

        //Update with new information
        updateWithRoomInformation();
        setOptionsEnabled(true);
        UpdateStatText();
    }

    public void updateWithRoomInformation() {
        background.color = gm.room.backgroundColor;


        //Set up obstacle
        obsName.text = gm.room.obstacle.name;
        obsDescription.text = gm.room.obstacle.description;


        //Set up option buttons text
        leftOption.GetComponentInChildren<Text>().text = gm.room.options[0].description;
        rightOption.GetComponentInChildren<Text>().text = gm.room.options[1].description;
    }

    public void selectLeftOption() {
        leftOption.interactable = false;
        rightOption.interactable = true;
        gm.optionSelected = gm.room.options[0];
    }
    public void selectRightOption() {
        leftOption.interactable = true;
        rightOption.interactable = false;
        gm.optionSelected = gm.room.options[1];
    }


    public void setOptionsEnabled(bool enabled) {
        leftOption.interactable = enabled;
        rightOption.interactable = enabled;
    }

    
}
