using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnObjects : MonoBehaviour {

    public GameObject[] angryPrefabs;
    public GameObject[] goodPrefabs;
    public GameObject bonusPrefab;

    public Transform[] objectsPositions;
    List<Transform> objectsTransforms;




    void OnChangeMat()
    {
       // Debug.Log("Hello");
        GetComponent<MeshRenderer>().material = GameController.Instance.stairMaterials[GameController.Instance.matNum];
        
    }

    void SpawnAll()
    {
        Transform where;

        //бонусный слот
        where = objectsTransforms[Random.Range(0, objectsTransforms.Count)];
        GameObject star;
        star = Instantiate(bonusPrefab);
        star.transform.parent = transform;
        star.transform.position = where.position;
        objectsTransforms.Remove(where);

        //препятствия в зависимости от номера ступени (из оставшихся 4)
        int angryNum = 0;
        if (GameController.Instance.stairs < 11)
        {
            angryNum = Random.Range(1, 4);
        }
        else if (GameController.Instance.stairs < 21)
        {
            angryNum = Random.Range(1, 5);
        }
        else
        {
            angryNum = Random.Range(2, 5);
        }
        for (int i = 0; i < angryNum; i++)
        {
            where = objectsTransforms[Random.Range(0, objectsTransforms.Count)];
            star = Instantiate(angryPrefabs[Random.Range(GameController.Instance.matNum*2, GameController.Instance.matNum * 2+2)]);
            star.transform.parent = transform;
            star.transform.position = where.position;
            objectsTransforms.Remove(where);
        }

        //на остальное, если есть свободные - звезды
        int starNum = objectsTransforms.Count;
        if (starNum > 2)
        {
            starNum = 2; //не больше 2
        }
        float ifStar = 0.0f;
        for (int i=0;i<starNum;i++)
        {
            where = objectsTransforms[Random.Range(0, objectsTransforms.Count)];
            ifStar = Random.Range(0.0f, 100.0f);
            if (ifStar <= 20.0f)
            {
                star = Instantiate(goodPrefabs[Random.Range(0, goodPrefabs.Length)]);
                star.transform.parent = transform;
                star.transform.position = where.position + new Vector3(0, 0.5f, 0);
            }
            objectsTransforms.Remove(where);
        }


    }


	void Start () {
        objectsTransforms = new List<Transform>();
        for(int i = 0; i<objectsPositions.Length;i++)
        {
            objectsTransforms.Add(objectsPositions[i]);
        }

        GetComponent<MeshRenderer>().material = GameController.Instance.stairMaterials[GameController.Instance.matNum];

        SpawnAll();

        //все что ниже - комментить
        /*
        GameObject star;
        int starsCount;
        int status; //0 - просто ступенька, 1 - злая ступенька, 2 - добрая ступенька
        status = Random.Range(0, 3);
        switch(status)
        {
            case 0:
                //ничего не спавним
                break;
            case 1:
                //не больше 3х препятствий
                starsCount = Random.Range(1, 4);
                switch (starsCount)
                {
                    case 1:
                        star = Instantiate(angryPrefabs[0]);
                        star.transform.parent = transform;
                        star.transform.localPosition = new Vector3(Random.Range(-2, 3)*0.2f, 0.5f, 0);
                        //star.transform.parent = null;
                        break;
                    case 2:
                        var first = Random.Range(-1, 3);
                        star = Instantiate(angryPrefabs[0]);
                        star.transform.parent = transform;
                        star.transform.localPosition = new Vector3(Random.Range(-2, first) * 0.2f, 0.5f, 0);
                        //star.transform.parent = null;
                        star = Instantiate(angryPrefabs[0]);
                        star.transform.parent = transform;
                        star.transform.localPosition = new Vector3(Random.Range(first, 3) * 0.2f, 0.5f, 0);
                       // star.transform.parent = null;
                        break;
                    case 3:
                        var fst = Random.Range(-1, 2);
                        var snd = Random.Range(fst, 3);
                        star = Instantiate(angryPrefabs[0]);
                        star.transform.parent = transform;
                        star.transform.localPosition = new Vector3(Random.Range(-2, fst) * 0.2f, 0.5f, 0);
                       // star.transform.parent = null;
                        star = Instantiate(angryPrefabs[0]);
                        star.transform.parent = transform;
                        star.transform.localPosition = new Vector3(Random.Range(fst, snd) * 0.2f, 0.5f, 0);
                        //star.transform.parent = null;
                        star = Instantiate(angryPrefabs[0]);
                        star.transform.parent = transform;
                        star.transform.localPosition = new Vector3(Random.Range(snd, 3) * 0.2f, 0.5f, 0);
                        //star.transform.parent = null;
                        break;
                    default:
                        break;
                }
                        break;
            case 2:
                //не больше 3х звезд на ступеньку
                starsCount = Random.Range(1, 4);
                switch(starsCount)
                {
                    case 1:
                        star = Instantiate(goodPrefabs[0]);
                        star.transform.parent = transform;
                        star.transform.localPosition = new Vector3(Random.Range(-2, 3) * 0.2f, 1, 0);
                       // star.transform.parent = null;
                        break;
                    case 2:
                        var first = Random.Range(-1, 3);
                        star = Instantiate(goodPrefabs[0]);
                        star.transform.parent = transform;
                        star.transform.localPosition = new Vector3(Random.Range(-2, first) * 0.2f, 1, 0);
                       // star.transform.parent = null;
                        star = Instantiate(goodPrefabs[0]);
                        star.transform.parent = transform;
                        star.transform.localPosition = new Vector3(Random.Range(first, 3) * 0.2f, 1, 0);
                       // star.transform.parent = null;
                        break;
                    case 3:
                        var fst = Random.Range(-1, 2);
                        var snd = Random.Range(fst, 3);
                        star = Instantiate(goodPrefabs[0]);
                        star.transform.parent = transform;
                        star.transform.localPosition = new Vector3(Random.Range(-2, fst) * 0.2f, 1, 0);
                        //star.transform.parent = null;
                        star = Instantiate(goodPrefabs[0]);
                        star.transform.parent = transform;
                        star.transform.localPosition = new Vector3(Random.Range(fst, snd) * 0.2f, 1, 0);
                       // star.transform.parent = null;
                        star = Instantiate(goodPrefabs[0]);
                        star.transform.parent = transform;
                        star.transform.localPosition = new Vector3(Random.Range(snd, 3) * 0.2f, 1, 0);
                        //star.transform.parent = null;
                        break;
                    default:
                        break;

                }

                
                //1
                //var star = Instantiate(goodPrefabs[0]);
                //star.transform.position = new Vector3(-2, 1, 0);
                //2
                //star = Instantiate(goodPrefabs[0]);
                //3
                //star = Instantiate(goodPrefabs[0]);
                //4
                //star = Instantiate(goodPrefabs[0]);
                //5
                //star = Instantiate(goodPrefabs[0]);
                break;
                
            default:

                break;

        }


        */


        // OnChangeMat() += GameController.Instance.ChangeLocation;

        GameController.OnChangeLocation += OnChangeMat;
	}

    void OnDestroy()
    {
        GameController.OnChangeLocation -= OnChangeMat;
    }
	

}
