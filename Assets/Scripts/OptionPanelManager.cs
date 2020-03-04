using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OptionPanelManager : MonoBehaviour
{
    //Global vars that ar looked up
    Option[] options;
    int numOptions;

    //Variables for self
    public bool[] optionsSelected;
    OptionWrapper[] optionButtons;

    //Properties for self
    RectTransform rectTransform;
    public float width;
    float height;
    public GameObject buttonPrefab;

    struct OptionWrapper
    {
        Text text;
        Button button;
        Image indicator;

        public OptionWrapper(Button button, Text text,  Image indicator) {
            this.text = text;
            this.button = button;
            this.indicator = indicator;
        }
        public void setText(string newText) {
            text.text = newText;
        }
        public void setState(bool state) {
            if (button.interactable) //Maybe make this check somewhere else
                indicator.enabled = state;
        }

        public void setEnabled(bool enabled) {
            button.interactable = enabled;
        }

        public void setColor(int rarity) {
            switch (rarity) {
                case 1:
                    indicator.color = new Color(0f, 0f, 1f, 0.15f);
                    break;
                case 0:
                default: 
                    indicator.color = new Color(0f, 1f, 0f, 0.15f);
                    break;
                
            }
        }

    }


    public void init() {
        options = GameManager.instance.room.options.ToArray();
        numOptions = options.Length;

        rectTransform = GetComponent<RectTransform>();
        width = 400f;
        height = 180f;
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, -height, height);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);

        optionButtons = new OptionWrapper[numOptions];
        optionsSelected = new bool[numOptions]; //Does instantiating set them all to false? I do this later anyways

        generateButtons();
    }

    void generateButtons() {
        for (int i = 0; i < numOptions; i++) {
            GameObject g = Instantiate(buttonPrefab, transform);

            //Resize the button appropriately. Somehow it was very difficult to figure out the appropriate code. Maybe there is a better way.
            RectTransform rt = g.GetComponent<RectTransform>();
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, i * width / numOptions , width/numOptions); 
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, height);

            g.GetComponent<Button>().onClick.AddListener(generateListener(i));
            optionButtons[i] = new OptionWrapper(g.GetComponent<Button>(), g.GetComponentInChildren<Text>(), g.transform.Find("OptionIndicator").GetComponent<Image>());

            optionButtons[i].setState(false);
            string reinfStr = "";
            if(options[i].conduit != null) reinfStr = "\n\n Reinforcement " + options[i].conduit.reinforcement;
            optionButtons[i].setText(options[i].getDescription() + reinfStr);
            optionButtons[i].setColor(options[i].rarity);

            optionsSelected[i] = false;
        }
    }

    //Generates the listener for button i
    UnityAction generateListener(int i) {
        return () => { processInput(i); };
    }

    // Processes an input of selecting button i
    public void processInput(int buttonPressed) {
        if (buttonPressed >= numOptions) return;

        //If we're selecting a new option and can't select multiple, active the reset flag
        bool resetFlag = (!optionsSelected[buttonPressed] && !Relic.tempRelicOneFlag); 
        for (int i = 0; i < numOptions; i++) {
            if (i == buttonPressed) {
                //Toggle button pressed
                optionsSelected[i] = !optionsSelected[i];
                optionButtons[i].setState(optionsSelected[i]);
            } else if (resetFlag && optionsSelected[i]) {
                //Reset other buttons if necessary
                optionsSelected[i] = false;
                optionButtons[i].setState(optionsSelected[i]);
            }

        }
    }

    public void disableButtons() {
        foreach (OptionWrapper opt in optionButtons) {
            opt.setEnabled(false);
        }
    }

}
