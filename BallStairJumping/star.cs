using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class star : MonoBehaviour {

    public GameObject effect;
    	
    void OnTriggerEnter(Collider collision)
    {
        GameController.Instance.bonus++;
        GameController.Instance.AddStars(1);

        SoundMng.Instance.PlaySound(SoundMng.SoundType.StarTouch);
        var boom = Instantiate(effect);
        boom.transform.position = transform.position;
        //boom.transform.parent = null;

        Destroy(gameObject);
    }


	void FixedUpdate ()
    {
        transform.Rotate(0*Time.timeScale, 5 * Time.timeScale, 0 * Time.timeScale);
    }
}
