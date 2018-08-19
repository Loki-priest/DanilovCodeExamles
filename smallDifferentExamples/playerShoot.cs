using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerShoot : MonoBehaviour {

    // Update is called once per frame
    public float timeBetweenShots = 0.1f;  // Allow 10 shots per second
    public UnityStandardAssets.CrossPlatformInput.TouchPad touchpad;
    private float timestamp;
    public Button thisButton;
    public GameObject shootParticle;

    public void Shoot()
    {
        if (Time.time >= timestamp)
        {
            //Instantiate(bullet, transform.position, transform.rotation);
            touchpad.ButtonShoot();
            timestamp = Time.time + timeBetweenShots;
        }
    }
    public bool isRacePressed = false;
    public bool isbrakePressed = false;

    public void onPointerDownRaceButton()
    {
        isRacePressed = true;
    }
    public void onPointerUpRaceButton()
    {
        isRacePressed = false;
    }


    void Update()
    {
        if (Time.time >= timestamp && (Input.GetKey(KeyCode.DownArrow)) && thisButton.IsInteractable())
        {
            //Instantiate(bullet, transform.position, transform.rotation);
            GeneralScript.Instance.playerInfo.gunhealth -= timeBetweenShots;
            GeneralScript.Instance.coolDelay = 0.5f;
            touchpad.ButtonShoot();
            timestamp = Time.time + timeBetweenShots;
        }


        if (isRacePressed)
        {
            shootParticle.SetActive(thisButton.IsInteractable());
            
            if (Time.time >= timestamp && thisButton.IsInteractable())
            {
                //Instantiate(bullet, transform.position, transform.rotation);
                GeneralScript.Instance.playerInfo.gunhealth -= timeBetweenShots;
                GeneralScript.Instance.coolDelay = 0.5f;
                touchpad.ButtonShoot();
                timestamp = Time.time + timeBetweenShots;
            }
        }

        else if (!isRacePressed)
        {
            shootParticle.SetActive(false);
           
        }

    }
}
