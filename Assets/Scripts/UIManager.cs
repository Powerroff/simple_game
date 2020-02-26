using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    //Maybe will eventually include a static instance for this; right now there's no need

    GameManager gm; //Including this here for convenience only.

    public GameObject opmPrefab, consequencePrefab, optSelectPrefab;

    //UI Elements
    public Button nextRoomButton;
    public Image background;
    public Text hpText, stamText, obsName, obsHp, obsDescription, relicName, relicDescription;
    public Canvas canvas;
    
    public OptionPanelManager opm = null;
    public OptSelectManager osm = null;
    public ConsPanelManager consequencePanel = null;
    public ConsPanelManager oldConsequencePanel = null;




    public void init()
    {
        gm = GameManager.instance;
        nextRoomButton.onClick.AddListener(() => { takeAction(); });

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        if (Input.GetKeyDown(KeyCode.Space)) takeAction();
        if (Input.GetKeyDown(KeyCode.Alpha1)) opm.processInput(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) opm.processInput(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) opm.processInput(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) opm.processInput(3);
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            if (osm == null) {
                osm = Instantiate(optSelectPrefab, canvas.transform).GetComponent<OptSelectManager>();
                osm.init();
            } else {
                Destroy(osm.gameObject);
                osm = null;
            }
        }

    }

    void UpdateStatText() {
        hpText.text = "HP: " + gm.player.stats.hp;
        stamText.text = "Stamina: " + gm.player.stats.stamina;
        obsHp.text = "Health: " + gm.room.obstacle.health;
    }


    public void displayConsequence() {
        oldConsequencePanel = consequencePanel;
        consequencePanel = Instantiate(consequencePrefab, opm.transform).GetComponentInChildren<ConsPanelManager>();
        consequencePanel.init();
        UpdateStatText();
    }

    public void newOPM() {
        Transform opmParent = null;
        if (consequencePanel != null) {
            consequencePanel.transform.SetParent(obsDescription.transform);
            consequencePanel.updateLocation();
            opmParent = consequencePanel.transform;
        } else {
            opmParent = obsDescription.transform;
        }

        if (oldConsequencePanel != null) 
            Destroy(oldConsequencePanel.gameObject);
        else if (opm != null) 
            Destroy(opm.gameObject);
        opm = Instantiate(opmPrefab, opmParent).GetComponent<OptionPanelManager>();
        opm.init();

        nextRoomButton.interactable = true;

    }

    public void nextRoom() {


        //Destroy old option panel
        if (oldConsequencePanel != null)
            Destroy(oldConsequencePanel.gameObject);
        if (opm != null)
            Destroy(opm.gameObject);
        if (consequencePanel != null)
            Destroy(consequencePanel.gameObject);

        //Update with new information
        updateWithRoomInformation();
        UpdateStatText();
        nextRoomButton.gameObject.GetComponentInChildren<Text>().text = "Take Action";


        consequencePanel = null;
        oldConsequencePanel = null;
        newOPM();

    }

    void takeAction() {
        if (!nextRoomButton.interactable) return;
        opm.disableButtons();  //Cosmetic
        nextRoomButton.interactable = false;
        gm.takeAction();
    }

    public void updateWithRoomInformation() {
        background.color = gm.room.backgroundColor;


        //Set up obstacle
        obsName.text = gm.room.obstacle.name;
        obsHp.text = "Health: " + gm.room.obstacle.health;
        obsDescription.text = gm.room.obstacle.description;


        //Set up relic text
        if (gm.room.relic != null) {
            relicName.text = gm.room.relic.name;
            relicDescription.text = gm.room.relic.description;
        } else {
            relicName.text = "";
            relicDescription.text = "";
        }

    }

    public void endRoom() {
        nextRoomButton.gameObject.GetComponentInChildren<Text>().text = "Next Room";
        nextRoomButton.interactable = true;
    }
}
