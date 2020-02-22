using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsPanelManager : MonoBehaviour
{

    RectTransform rt;

    public void init() {
        rt = gameObject.GetComponent<RectTransform>();

        updateSizeAndLocation();
        updateText();


    }

    public void updateLocation() {
        StartCoroutine(moveToParent());
    }

    void updateSizeAndLocation() {
        float width = gameObject.GetComponentInParent<OptionPanelManager>().width;
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, -30, 30);
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, width);
    }

    void updateText() {
        gameObject.GetComponentInChildren<Text>().text = GameManager.instance.consequences;
    }

    IEnumerator moveToParent() {
        rt = gameObject.GetComponent<RectTransform>();
        float amountToMove = -rt.offsetMax.y * Time.deltaTime;
        while (-rt.offsetMax.y > amountToMove) {
            transform.Translate(new Vector2(0, amountToMove));
            yield return null;
        }
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, -30, 30);
    }

}
