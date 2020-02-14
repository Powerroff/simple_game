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


    // Start is called before the first frame update
    void Start()
    {
        leftOption.onClick.AddListener(() => { player.stats.strength += 2; });
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
