using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conduit
{
    int _reinforcement = 0;
    public int max_reinforcement = 2; //Eventually make this a property of the option?
    public int reinforcement {
        get {
            return _reinforcement;
        }
        set {
            if (value < 0) _reinforcement = 0;                                      //Min is 0
            else if (value > max_reinforcement) _reinforcement = max_reinforcement; //Upper bound
            else if (_reinforcement < max_reinforcement) _reinforcement = value;    //Otherwise change unless at max
        }
    }

    public int[] incomingPower;
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
            return color != -1 && (acceptedColors[color] || acceptedColors[(int)powerColors.rainbow]);
        }


    }

    public Conduit(Option option, connector[] inputs, connector[] outputs) {
        this.option = option;
        this.inputs = inputs;
        this.outputs = outputs; //3 directions right now, may rework later
        incomingPower = new int[] { -1, -1, -1 };
        activePowers = new bool[numColors];
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
                activePowers[(int)incomingPower[i]] = true; //rework for rainbow inputs at some point
        }
    }

    protected virtual bool[] getOutputs() {
        processInputs();
        return activePowers;
    }

    public int getOutput(int dir) { //returns color of output in given direction
        if (reinforcement < max_reinforcement) return -1;
        bool[] outputColors = getOutputs();
        for (int color = numColors - 1; color >= 0; color--) //Priority for colors in reverse order (??)
            if (outputs[dir] != null && (outputColors[color] && outputs[dir].accepts(color))) 
                return color;
            
        return -1;

    }

    public virtual bool isReceivingPower() {
        processInputs();
        foreach (bool b in activePowers)
            if (b) return true;
        return false;
    }

    public bool isPowered() {
        return (isReceivingPower() && reinforcement == max_reinforcement);
    }

    public virtual void resetPower() {
        _reinforcement = 0;
    }

    // Useful connectors --------------------


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
            bool[] outputColors = new bool[numColors];
            outputColors[color] = true;
            return outputColors;
        }

        //Generator never requires power input.
        public override bool isReceivingPower() {
            return true;
        }

        public override void resetPower() {
            return; //Cannot be unpowered once powered!
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

        public Reactor(Option option, connector[] inputs, connector[] outputs, int color, int[] requirements) : base(option, inputs, outputs) {
            this.color = color;
            this.requirements = new bool[numColors];
            foreach (int i in requirements)
                this.requirements[i] = true;
        }

        protected override bool[] getOutputs() {
            processInputs();
            bool[] outputColors = new bool[numColors];
            if (isPowered())
                outputColors[color] = true;
            return outputColors;
        }

        public override bool isReceivingPower() {
            processInputs();
            if (activePowers[(int)powerColors.rainbow]) return true; //hm
            for (int c = 0; c < numColors; c++)
                if (requirements[c] && !activePowers[c]) {
                    return false;
                }
            return true;
        }
    }




}
