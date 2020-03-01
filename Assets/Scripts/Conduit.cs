using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conduit
{
    public bool activated;

    int[] incomingPower;
    bool[] activePowers;

    Option option;
    connector[] inputs;
    connector[] outputs;

    const int numColors = 4;
    public enum powerColors { red, green, yellow, rainbow };
    //public enum inputDirs { top_left, top_center, top_right };
    //public enum outputDirs { bottom_left, bottom_center, bottom_right };
    public enum type { conduit, generator, reactor };

    public class connector {

        bool[] acceptedColors;

        public connector(bool[] acceptedColors) {
            this.acceptedColors = acceptedColors;
        }

        public bool accepts(int color) {
            return color != -1 && acceptedColors[color];
        }


    }

    public Conduit(Option option, connector[] inputs, connector[] outputs) {
        this.option = option;
        this.inputs = inputs;
        this.outputs = outputs; //3 directions right now, may rework later
        incomingPower = new int[3];
        activePowers = new bool[numColors];
        activated = false;
    }

    public void setInput(int dir, bool[] acceptedColors) {
        inputs[dir] = new connector(acceptedColors);
    }

    public void setOutput(int dir, bool[] acceptedColors) {
        outputs[dir] = new connector(acceptedColors);
    }

    public void processInputs() {
        activePowers = new bool[numColors];
        for (int i = 0; i < 3; i++) {
            if (inputs[i] != null && inputs[i].accepts(incomingPower[i]))
                activePowers[(int)incomingPower[i]] = true;
        }
    }

    protected virtual bool[] getOutputs() {
        //processInputs();
        return activePowers;
    }

    public int getOutput(int dir) { //returns color of output in given direction
        bool[] outputColors = getOutputs();
        for (int color = numColors - 1; color >= 0; color--) //Priority for colors in reverse order (??)
            if (outputs[dir] != null && (outputColors[color] && outputs[dir].accepts(color)))
                return color;
        return -1;

    }

    // Useful connectors --------------------

    public static connector red_connector() {
        bool[] accept = new bool[numColors];
        accept[(int)powerColors.red] = true;
        return new connector(accept);
    }

    public static connector green_connector() {
        bool[] accept = new bool[numColors];
        accept[(int)powerColors.red] = true;
        return new connector(accept);
    }

    public static connector yellow_connector() {
        bool[] accept = new bool[numColors];
        accept[(int)powerColors.red] = true;
        return new connector(accept);
    }

    public static connector all_connector() {
        bool[] accept = new bool[numColors];
        for (int i = 0; i < numColors; i++)
            accept[i] = true;
        return new connector(accept);
    }

    public static connector[] newConnectors(int left_color, int center_color, int right_color) {
        int[] colors = new int[] { left_color, center_color, right_color };
        connector[] c = new connector[3];
        for (int inp = 0; inp < 3; inp++) {
            if(colors[inp] < 0) continue;
            //if (colors[inp] == numColors) c[inp] = all_connector();

            bool[] accept = new bool[numColors];
            accept[colors[inp]] = true;
            c[inp] = new connector(accept);
        }
        return c;
    }

    //Special Conduits ----------------------

    public class Generator : Conduit {
        int color;

        public Generator(Option option, connector[] outputs, int color) : base(option, new connector[3], outputs) {
            this.color = color;
        }

        protected override bool[] getOutputs() {
            //processInputs();
            bool[] outputColors = new bool[numColors];
            outputColors[color] = true;
            return outputColors;
        }
    }

    public class Reactor : Conduit
    {
        int color;
        bool[] requirements;

        public Reactor(Option option, connector[] inputs, connector[] outputs, int color, bool[] requirements) : base(option, inputs, outputs) {
            this.color = color;
            this.requirements = requirements;
        }

        protected override bool[] getOutputs() {
            //processInputs();
            bool[] outputColors = new bool[numColors];
            for (int c = 0; c < numColors; c++)
                if (requirements[c] && !activePowers[c]) //Returns no power unless all requirements are met
                    return outputColors;
            
            outputColors[color] = true;
            return outputColors;
        }
    }




}
