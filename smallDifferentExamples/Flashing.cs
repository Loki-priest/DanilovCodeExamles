using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashing : MonoBehaviour {

    bool isHide = true;
    Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }


    void Update () {
        var color = text.color;
        if (isHide)
        {
            color.a -= Time.deltaTime;
        }
        else
        {
            color.a += Time.deltaTime;
        }
        text.color = color;
        if (text.color.a <= 0)
        {
            isHide = false;
        }
        if (text.color.a >= 1)
        {
            isHide = true;
        }
	}
}
