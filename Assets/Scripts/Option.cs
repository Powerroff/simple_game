using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Option
{
    public string description;
    public UnityAction onpress;

    public Option(string s, UnityAction op)
    {
        description = s;
        onpress = op;
    }

    
}
