using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Color backgroundColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void init() {
        backgroundColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
