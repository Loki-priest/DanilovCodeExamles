using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatforms : MonoBehaviour
{


    private Vector3 newPos;


    public int platformCount;
    int platformNumber;
    [SerializeField]
    private int platformMax;
    [SerializeField]
    private GameObject platformParent;
    [SerializeField]
    private GameObject platformPrefab;

    // Use this for initialization
    void Start()
    {
        platformCount = 0;
        platformNumber = 0;
        //newPos = new Vector3(0f, -84.6f, -144f);
        newPos = new Vector3(0f, -7.6f, -12);
    }

    public void SpawnPlatform()
    {
        var platform = Instantiate(platformPrefab, newPos, Quaternion.identity, platformParent.transform);
        platform.name = "Platform" + platformNumber;

        Vector3 tmpPos = newPos;
        //рандомайз
        float random = Random.Range(0f, 100f);
        if (random <= 35f)
        {
            tmpPos.x = -3f;
        }
        else if (random <= 65f)
        {
            tmpPos.x = 3f;
        }
        else if (random <= 80f)
        {
            tmpPos.x = 0f;
        }
        else
        {
            platform.AddComponent<MovePlatform>();
        }
        platform.transform.position = tmpPos;

        //монетка вкл?
        if (platformNumber % 2 == 1)
        {
            platform.GetComponent<PlatformDelete>().AddCoin();
        }
        int crossPlatforming = Random.Range(3, 6);
        if (crossPlatforming % 4 == 1)
        {
            int busterChance = Random.Range(0, 100);
            if (busterChance < 35)
            {
                int freezeChance = Random.Range(0, 2);
                if (freezeChance == 0)
                {
                    platform.GetComponent<PlatformDelete>().AddFreeze();
                }
                else
                {
                    platform.GetComponent<PlatformDelete>().AddX2();
                }
            }
        }


        newPos.y -= 7.0f;
        newPos.z -= 12.0f;
        platformNumber++;
        platformCount++;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (platformCount < platformMax)
        {
            SpawnPlatform();
        }

    }
}
