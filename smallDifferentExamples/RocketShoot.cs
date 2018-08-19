using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketShoot : MonoBehaviour {

    public float timeBetweenShots = 1.5f;  // Allow 10 shots per second
    public UnityStandardAssets.CrossPlatformInput.TouchPad touchpad;
    private float timestamp;
    public Button thisButton;
    public Image imageCooldown;

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

    // Update is called once per frame
    void Update () {


        if (GeneralScript.Instance.playerInfo.missiles > 0)
        {
            if (Time.time >= timestamp)
            {
                thisButton.interactable = true;
                imageCooldown.gameObject.SetActive(false);
            }
            else
            {
                thisButton.interactable = false;
                imageCooldown.gameObject.SetActive(true);
                imageCooldown.fillAmount = (timestamp - Time.time) / timeBetweenShots;
            }
        }
        else
        {
            thisButton.interactable = false;
            imageCooldown.gameObject.SetActive(true);
            imageCooldown.fillAmount = 1;
        }



        if (isRacePressed)
        {

            if (Time.time >= timestamp && thisButton.IsInteractable())
            {
                //Instantiate(bullet, transform.position, transform.rotation);
                // GeneralScript.Instance.playerInfo.gunhealth -= timeBetweenShots;
                // GeneralScript.Instance.coolDelay = 0.5f;

                // touchpad.ButtonShoot();
                touchpad.LaunchRocket();
                //GeneralScript.Instance.playerInfo.missiles--;
                GeneralScript.Instance.SpendMissile();
                timestamp = Time.time + timeBetweenShots;
            }

        }

        else if (!isRacePressed)
        {

        }

    }
}
