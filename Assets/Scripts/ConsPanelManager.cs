using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsPanelManager : MonoBehaviour
{

    public void init() {
        updateLocation();
        updateText();


    }

    void updateLocation() {
        float width = gameObject.GetComponentInParent<OptionPanelManager>().width;
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, -30, 30);
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, width);
    }

    void updateText() {
        gameObject.GetComponentInChildren<Text>().text = GameManager.instance.consequences;
    }

}
