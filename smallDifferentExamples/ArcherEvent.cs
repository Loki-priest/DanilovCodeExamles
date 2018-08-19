using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEvent : MonoBehaviour {

    public GameObject leftArrow;
    public GameObject rightArrow;
    public GameObject[] rightArrows;
    public UnityStandardAssets.CrossPlatformInput.TouchPad touchpad;
    bool ifShoot;
    Transform[] pos;

    // Use this for initialization
    void Awake () {

        pos = new Transform[2];
        pos[0] = rightArrows[0].transform;
        pos[1] = rightArrows[4].transform;


        ifShoot = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (ifShoot)
            for (int i = 0; i < rightArrows.Length; i++)
            {
                if(i!=0&&i!=4)
                    rightArrows[i].transform.position += rightArrows[i].transform.forward * 5f * Time.deltaTime;
            }
        else
        {
            for (int i = 1; i < 4; i++)
            {
                rightArrows[i].transform.position = pos[0].position;
            }
            for (int i = 5; i < 8; i++)
            {
                rightArrows[i].transform.position = pos[1].position;
            }
        }

    }


    private void onBowTakeArrow()
    {
        ifShoot = false;
        rightArrow.SetActive(true);
    }


    private void OnCrossTakeArrow()
    {
        ifShoot = false;
        leftArrow.SetActive(true);
    }


    private void OnPreShoot()
    {
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
        ifShoot = true;
    }

  private void OnCrossShoot()
    {

        for (int i = 0; i < rightArrows.Length; i++)
        rightArrows[i].SetActive(false);
       rightArrow.SetActive(false);

        ifShoot = false;
        touchpad.Shoot();
    }

   private void OnCrossStart()
    {
        ifShoot = false;
        leftArrow.SetActive(true);
        for (int i=0; i<rightArrows.Length; i++)
            rightArrows[i].SetActive(false);
    }


    private void OnCrossReload()
    {
        ifShoot = false;
        leftArrow.SetActive(false);
        for (int i=0; i<rightArrows.Length; i++)
            rightArrows[i].SetActive(true);

    }


}
