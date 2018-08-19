using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSkinChanger : MonoBehaviour {


    public Material[] materials;
    private SaverGameData saverGameData;

    void Start () {
        if (!GameController.Instance.isDebug)
        {
            saverGameData = SaverGameData.Instance;
        }

        if (!GameController.Instance.isDebug)
        {
            GetComponent<MeshRenderer>().material = materials[saverGameData.gameData.currentBall.Value];
        }
        else
        {
            GetComponent<MeshRenderer>().material = materials[Random.Range(0, materials.Length)];
        }
        
	}

}
