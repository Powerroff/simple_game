using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conduit 
{
    public bool activated;

    powerColors[] incomingPower;

    Option option;
    connector[] inputs;
    connector[] outputs;

    public enum powerColors { red, green, yellow, rainbow };
    public enum inputDirs { top_left, top_center, top_right };
    public enum outputDirs { bottom_left, bottom_center, bottom_right };
    
    struct connector
    {
        
        powerColors[] acceptedColors;

        public connector(powerColors[] acceptedColors) {
            this.acceptedColors = acceptedColors;
        }

        public bool accepts(powerColors color) {
            if (color == powerColors.rainbow) return true;
            for (int i = 0; i < acceptedColors.Length; i++)
                if (acceptedColors[i] == color) return true;
            return false;
        }


    }

    public Conduit(Option option) {
        this.option = option;
        inputs = new connector[3];
        outputs = new connector[3]; //3 directions right now, may rework later
        incomingPower = new powerColors[3];
        activated = false;
    }

    public void setInput(inputDirs dir, powerColors[] acceptedColors) {
        inputs[(int)dir] = new connector(acceptedColors);
    }

    public void setOutput(outputDirs dir, powerColors[] acceptedColors) {
        outputs[(int)dir] = new connector(acceptedColors);
    }


}
