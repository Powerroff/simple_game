using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public StatsManager stats;
    public EquipmentManager equipment;
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
    }

    //Research Action vs UnityAction?

    public UnityAction changeStam(int amount) {
        return (() => { stats.stamina += amount; });
    }

    public UnityAction changeHp(int amount) {
        return (() => { stats.hp += amount; });
    }

}
