using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Maybe will eventually include a static instance for this; right now there's no need

    GameManager gm; //Including this here for convenience only.

    public GameObject opmPrefab;
    public GameObject consequencePrefab;

    //UI Elements
    public Button nextRoomButton;
    public Image background;
    public Text hpText, stamText, strText, obsName, obsDescription, relicName, relicDescription;
    public Canvas canvas;
    
    public OptionPanelManager opm = null;
    



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
        //if (Input.GetKeyDown(KeyCode.Space)) nextRoom();
        if (Input.GetKeyDown(KeyCode.Space)) processAction();
            if (Input.GetKeyDown(KeyCode.Alpha1)) opm.processInput(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) opm.processInput(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) opm.processInput(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) opm.processInput(3);
    }

    void UpdateStatText() {
        hpText.text = "HP: " + gm.player.stats.hp;
        stamText.text = "Stamina: " + gm.player.stats.stamina;
        strText.text = "Strength: " + gm.player.stats.strength;
    }

    public void processAction() {
        gm.takeAction();
        ConsPanelManager cBox = Instantiate(consequencePrefab, opm.transform).GetComponentInChildren<ConsPanelManager>();
        cBox.init();
    }

    public void displayConsequence() {
        ConsPanelManager cBox = Instantiate(consequencePrefab, opm.transform).GetComponentInChildren<ConsPanelManager>();
        cBox.init();
    }

    public void nextRoom() {

        //GM processes this room and generates the next room
        


        //Destroy old option panel
        if (opm != null)
            Destroy(opm.gameObject);

        //Update with new information
        updateWithRoomInformation();
        UpdateStatText();

    }

    public void updateWithRoomInformation() {
        background.color = gm.room.backgroundColor;


        //Set up obstacle
        obsName.text = gm.room.obstacle.name;
        obsDescription.text = gm.room.obstacle.description;

        //Set up option panel
        opm = Instantiate(opmPrefab, canvas.transform).GetComponent<OptionPanelManager>();
        opm.init();


        //Set up relic text
        if (gm.room.relic != null) {
            relicName.text = gm.room.relic.name;
            relicDescription.text = gm.room.relic.description;
        } else {
            relicName.text = "";
            relicDescription.text = "";
        }

    }

    
}
