using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptSelectManager : MonoBehaviour
{
    public Dropdown[] layer0;
    public Dropdown[] layer1;
    public Dropdown[] layer2;
    Dropdown[][] boxes;

    public OptionTree optionTree;

    public void init() {
        boxes = new Dropdown[][] { layer0, layer1, layer2 };
        optionTree = GameManager.instance.player.optionTree;
        for (int i = 0; i < optionTree.nodeTree.Length; i++) {
            for (int j = 0; j < optionTree.nodeTree[i].Length; j++) {
                boxes[i][j].ClearOptions();
                boxes[i][j].options.Add(datify(optionTree.nodeTree[i][j]));
                boxes[i][j].RefreshShownValue();
            }
        }
    }

    public static Dropdown.OptionData datify(OptionTree.OptionNode n) {
        Dropdown.OptionData data = new Dropdown.OptionData();
        data.text = n.option.description; //HM
        return data;
    }

}
