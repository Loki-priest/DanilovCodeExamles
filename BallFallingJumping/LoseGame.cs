using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseGame : MonoBehaviour {

    void OnTriggerEnter(Collider collision)
    {
        GameController.Instance.minus++;
        GameController.Instance.UpdateGUI();
        // звук касания препятствия
        SoundMng.Instance.PlaySound(SoundMng.SoundType.AngryTouch);
        // геймкотроллер.луз()
        if (GameController.Instance.minus >= 1 && !GameController.Instance.isLost) //10
        {
            GameController.Instance.isLost = true;
            GameController.Instance.bonus+=1;
            GameController.Instance.Ball.KillBall();
        }
    }
}
