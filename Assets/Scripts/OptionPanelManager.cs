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
    float width;
    float height;
    public GameObject buttonPrefab;

    struct OptionWrapper
    {
        Text text;
        Button button;
        Image indicator;

        //Using a pointer here... maybe not best practice??
        public OptionWrapper(Button button, Text text,  Image indicator) {
            this.text = text;
            this.button = button;
            this.indicator = indicator;
        }
        public void setText(string newText) {
            text.text = newText;
        }
        public void setState(bool state) {
            indicator.enabled = state;
        }
    }


    public void init() {
        options = GameManager.instance.room.options;
        numOptions = options.Length;

        rectTransform = GetComponent<RectTransform>();
        width = rectTransform.rect.width;
        height = rectTransform.rect.height;

        optionButtons = new OptionWrapper[numOptions];
        optionsSelected = new bool[numOptions]; //Does this set them all to false?

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
            optionButtons[i].setText(options[i].description);
            
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

}
