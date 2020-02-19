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
    public Image background, leftOptionIndicator, rightOptionIndicator;
    public Text hpText, stamText, strText, obsName, obsDescription, relicName, relicDescription;



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
        UpdateStatText();

        //Reset option indicators
        leftOptionIndicator.gameObject.SetActive(false);
        rightOptionIndicator.gameObject.SetActive(false);
    }

    public void updateWithRoomInformation() {
        background.color = gm.room.backgroundColor;


        //Set up obstacle
        obsName.text = gm.room.obstacle.name;
        obsDescription.text = gm.room.obstacle.description;


        //Set up option buttons text
        leftOption.GetComponentInChildren<Text>().text = gm.room.options[0].description;
        rightOption.GetComponentInChildren<Text>().text = gm.room.options[1].description;


        //Set up relic text
        if (gm.room.relic != null) {
            relicName.text = gm.room.relic.name;
            relicDescription.text = gm.room.relic.description;
        } else {
            relicName.text = "";
            relicDescription.text = "";
        }

    }


    //TODO rework tempreliconeflag
    public void selectLeftOption() {
        //Ensure right option is unselected if we aren't allowed to take multiple
        if (!gm.optionsSelected[0] && !Relic.tempRelicOneFlag) {
            rightOptionIndicator.gameObject.SetActive(false);
            gm.optionsSelected[1] = false;
        }

        //Toggle self
        gm.optionsSelected[0] = !gm.optionsSelected[0];
        leftOptionIndicator.gameObject.SetActive(!leftOptionIndicator.gameObject.activeSelf);
    }
    public void selectRightOption() {
        //Ensure left option is unselected if we aren't allowed to take multiple
        if (!gm.optionsSelected[1] && !Relic.tempRelicOneFlag) {
            leftOptionIndicator.gameObject.SetActive(false);
            gm.optionsSelected[0] = false;
        }

        //Toggle self
        gm.optionsSelected[1] = !gm.optionsSelected[1];
        rightOptionIndicator.gameObject.SetActive(!rightOptionIndicator.gameObject.activeSelf);
    }

    
}
