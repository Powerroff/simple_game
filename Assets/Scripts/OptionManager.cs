using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public void onClick() {
        UIManager uim = GameObject.Find("UIManager").GetComponent<UIManager>();
        uim.setOptionsEnabled(false);
        
    }

    public void resetListeners() {
        Button b = gameObject.GetComponent<Button>();
        b.onClick.RemoveAllListeners();
        b.onClick.AddListener(onClick);
    }
}
