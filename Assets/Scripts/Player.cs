﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        stats = new StatsManager(10,10,0);
        equipment = new EquipmentManager();
    }
}