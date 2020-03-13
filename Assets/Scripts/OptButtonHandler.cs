using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptButtonHandler : MonoBehaviour
{
    public Text title;
    public Text body;
    public Text monsterDmg;
    public Text natureDmg;

    public void setup(Option option) {
        title.text = option.shortened;
        body.text = option.consequence.description;
        monsterDmg.text = "" + (-option.consequence.monsterDmg);
        natureDmg.text = "" + (-option.consequence.natureDmg);
    }

}
