using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    public Color backgroundColor;
    public Option[] possibleEvents;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void init(Option[] pes) {
        backgroundColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        possibleEvents = pes;
    }


    public void newButtons(Button a, Button b)
    {
        if(possibleEvents.Length == 0)
        {
            return;
        }
        if(possibleEvents.Length == 1)
        {
            a.onClick.AddListener(possibleEvents[0].onpress);
            a.GetComponentInChildren<Text>().text = possibleEvents[0].description;
        }

        int selectionA = Random.Range(0, possibleEvents.Length);
        int selectionB = -1;

        do
        {
            selectionB = Random.Range(0, possibleEvents.Length);
        }
        while (selectionA == selectionB);

        a.onClick.AddListener(possibleEvents[selectionA].onpress);
        a.GetComponentInChildren<Text>().text = possibleEvents[selectionA].description;

        b.onClick.AddListener(possibleEvents[selectionB].onpress);
        b.GetComponentInChildren<Text>().text = possibleEvents[selectionB].description;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

}
