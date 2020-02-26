using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public StatsManager stats;
    public EquipmentManager equipment;
    public OptionTree optionTree;
    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void init() {
        stats = new StatsManager(100,100);
        equipment = new EquipmentManager();
        optionTree = OptionTree.defaultTree();
    }

    //Research Action vs UnityAction?

    public void updateStats(int hpChange, int stamChange) {
        stats.hp += hpChange;
        stats.stamina += stamChange;
    }

}
