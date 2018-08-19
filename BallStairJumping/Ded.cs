using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ded : MonoBehaviour {

    bool isDissapear;
    Material mat;
    Color col;
    
    public GameObject[] prefabs;

    public GameObject effect;

    void Start () {
        mat = GetComponentInChildren<MeshRenderer>().material;

        col = mat.color;
        col.a = 0.0f;
        mat.color = col;
        isDissapear = false;
        GameController.OnChangeLocation += OnChangeMat;
    }

    void OnDestroy()
    {
        GameController.OnChangeLocation -= OnChangeMat;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (!isDissapear)
        {
            GameController.Instance.minus++;
            GameController.Instance.UpdateGUI();
            // звук касания препятствия
            SoundMng.Instance.PlaySound(SoundMng.SoundType.AngryTouch);
            // геймкотроллер.луз()
            if (GameController.Instance.minus >= 1) //10
            {
                GameController.Instance.Ball.KillBall();
            }
            //Destroy(gameObject);
        }

    }


    void OnChangeMat()  // плавная смена
    {
        isDissapear = true;
        // GetComponent<MeshRenderer>().material = materials[Random.Range(0,materials.Length)];
        var newDed = Instantiate(prefabs[Random.Range(GameController.Instance.matNum*2, GameController.Instance.matNum * 2 + 2)]);
        newDed.transform.parent = gameObject.transform.parent;
        newDed.transform.position = gameObject.transform.position;
        Destroy(gameObject,0.5f); //1.0f

    }

    private void Update()
    {
        //skyBox.SetColor("_Tint", Color.Lerp(skyBox.GetColor("_Tint"), nextColor, Time.deltaTime));
        if (isDissapear)
        {// исчезает
         /*
         col = mat.color;

         if (col.a >= 0.1f)
         {
             col.a = 0.0f;
             mat.color = Color.Lerp(mat.color, col, Time.deltaTime);
         }
         else
         {
             col.a = 0.0f;
             mat.color = col;
         }
         */
            
            if (col.a > 0.0)
            {
                col.a -= Time.deltaTime / 1.0f;
                mat.color = col;
            }
        }
        else
        {// появляется
         /*
         col = mat.color;

         if (col.a <= 0.9f)
         {
             col.a = 1.0f;
             mat.color = Color.Lerp(mat.color, col, Time.deltaTime);
         }
         else
         {
             col.a = 1.0f;
             mat.color = col;
         }
         */
            if (col.a < 1.0)
            {
                col.a += Time.deltaTime / 1.0f;
                mat.color = col;
            }
        }
    }

}
