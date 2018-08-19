using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextMulti : MonoBehaviour {

    public float speed = 2.0f;
    bool goRight;
    Vector3 startPos;

    void Start () {
        startPos = new Vector3(0, 0, 0);
        startPos = transform.localPosition;

        GetComponent<Text>().text = "+" + GameController.Instance.multi.ToString();

        transform.parent.parent = null;

         

        Destroy(transform.parent.gameObject, 0.7f);
	}
	
	
	void Update () {
        //влево-вправо

        if (Vector3.Distance(transform.localPosition, startPos - new Vector3(5f,0,0)) <= 3f)
        {
            goRight = true;
        }
        if (Vector3.Distance(transform.localPosition, startPos + new Vector3(5f, 0, 0)) <= 3f)
        {
            goRight = false;
        }

        if (goRight)
        {
            transform.position -= transform.right * speed  * Time.deltaTime;
        }
        else
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }


    }
}
