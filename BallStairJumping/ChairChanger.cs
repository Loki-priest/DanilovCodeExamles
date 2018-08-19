using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairChanger : MonoBehaviour {

    public Texture[] textures;

    bool isDissapear;
    Material mat;
    Color col;

    public GameObject[] prefabs;
    /*
    void OnChangeMat()
    {
        //GetComponent<MeshRenderer>().material = GameController.Instance.stairMaterials[GameController.Instance.matNum];
        GetComponent<MeshRenderer>().material.DoBlend(textures[GameController.Instance.matNum], 1 * 1.5f);

    }

    void Start () {
        //GetComponent<MeshRenderer>().material = GameController.Instance.stairMaterials[GameController.Instance.matNum];
        GetComponent<MeshRenderer>().material.DoBlend(textures[GameController.Instance.matNum], Time.deltaTime);
        GameController.OnChangeLocation += OnChangeMat;
    }
    */



    void Start()
    {
        //GetComponent<MeshRenderer>().material = GameController.Instance.stairMaterials[GameController.Instance.matNum];
        // GetComponent<MeshRenderer>().material.DoBlend(textures[GameController.Instance.matNum], Time.deltaTime);
        GetComponent<MeshRenderer>().material = GameController.Instance.stairMaterials[GameController.Instance.matNum];
        mat = GetComponent<MeshRenderer>().material;
         
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

    void OnChangeMat()  // плавная смена
    {
        isDissapear = true;
        // GetComponent<MeshRenderer>().material = materials[Random.Range(0,materials.Length)];
        var newDed = Instantiate(prefabs[GameController.Instance.matNum]);
        newDed.transform.parent = gameObject.transform.parent;
        newDed.transform.position = gameObject.transform.position;
        Destroy(gameObject, 1.0f);

    }

    private void Update()
    {
        //skyBox.SetColor("_Tint", Color.Lerp(skyBox.GetColor("_Tint"), nextColor, Time.deltaTime));
        if (isDissapear)
        {// исчезает
            col = mat.color;
            /*
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
            col = mat.color;
            /*
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
