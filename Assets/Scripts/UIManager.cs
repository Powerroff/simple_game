using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameManager gm;
    public Button nextRoomButton, leftOption, rightOption;
    public Image background;
    public Player player;
    public Text hpText;
    public Text stamText;
    public Text strText;
    public Text obsName;
    public Text obsDescription;


    // Start is called before the first frame update
    void Start()
    {
        UpdateStatText();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateStatText() {
        hpText.text = "HP: " + player.stats.hp;
        stamText.text = "Stamina: " + player.stats.stamina;
        strText.text = "Strength: " + player.stats.strength;
    }


    public void nextRoom() {
        leftOption.GetComponent<OptionManager>().resetListeners();
        rightOption.GetComponent<OptionManager>().resetListeners();

        gm.nextRoom();
        setOptionsEnabled(true);


        player.stats.stamina -= 1;

        UpdateStatText();
    }

    public void updateWithRoomInformation(Room room) {
        background.color = room.backgroundColor;


        //Set up obstacle
        obsName.text = room.obstacle.name;
        obsDescription.text = room.obstacle.description;



        //Set up option buttons
        leftOption.onClick.AddListener(room.options[0].onpress);
        leftOption.GetComponentInChildren<Text>().text = room.options[0].description;

        rightOption.onClick.AddListener(room.options[1].onpress);
        rightOption.GetComponentInChildren<Text>().text = room.options[1].description;
    }


    public void setOptionsEnabled(bool enabled) {
        leftOption.interactable = enabled;
        rightOption.interactable = enabled;
    }

    
}
