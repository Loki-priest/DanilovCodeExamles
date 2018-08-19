using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handanim : MonoBehaviour
{

    public GameObject spin;
    public bool ifSpin = false;
    public UnityStandardAssets.CrossPlatformInput.TouchPad touchpad;
    // Use this for initialization
    void Start()
    {
        //spin.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpinIt()
    {
        ifSpin = true;
        touchpad.isRotate = true;
    }

    private void OnSpinnerTake()
    {
        touchpad.isRotate = false;
        spin.SetActive(true);
    }

    private void OnSpinnerThrow()
    {
        GeneralScript.Instance.DeactivateSpin();
        touchpad.Shoot();
    }
}
