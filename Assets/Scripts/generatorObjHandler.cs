using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class generatorObjHandler : MonoBehaviour
{
    public Sprite[] reds;
    public Sprite[] greens;
    public Sprite[] yellows;
    public Image image;

    //public void setImage(Conduit.powerColors color, int level) {
    public void setImage(int color, int level) {
        if (color == 0)
            image.sprite = reds[System.Math.Min(level, reds.Length - 1)];
        else if (color == 1) {
            image.sprite = greens[System.Math.Min(level, greens.Length - 1)];
        } else if (color == 2) {
            image.sprite = yellows[System.Math.Min(level, yellows.Length - 1)];
        } else Destroy(gameObject);
    }
}
