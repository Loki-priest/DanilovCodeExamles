using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PositiveCounter : MonoBehaviour
{

    public ItemType type;
    public int coeffDiffer = 1; //коэф разнообразия
    public float coeffReward = 1; //коэф награды
    public int coeffValue = 0; //коэф ценности
    public int sizeMod;

    public bool isBuilt = true;

    float timer;
    MarketItem t;

    public void AddNum(int val)
    {
        t.builtNum += val;
    }

    public void SetPenalty()
    {
        coeffReward = -1f;
    }

    void Start()
    {
        timer = 1.0f;

        // MarketItem t = MarketContainer.Instance.list.Select(x => x.Where(item => item.type == type).FirstOrDefault()).FirstOrDefault();
        t = MarketContainer.Instance.list.SelectMany(a => a.Where(item => item.type == type)).ToList().FirstOrDefault();
        //  Debug.Log((t.item.leafs + t.item.stones + t.item.woods).ToString());

#if ADS_VERSION
        if (t.item.leafs + t.item.stones+t.item.woods==0)
        {
            coeffValue = 0;
        }
        else
        {
            coeffValue = 0;
        }
#endif

        if (isBuilt)
        {
            AddNum(1);
        }

        Debug.Log(t.item.itemName);

    }

    void OnDestroy()
    {
        if (isBuilt)
        {
            KillMe();    //осторожнее с этим (ondestroy произойдет и при выходе из игры)
            AddNum(-1);
        }
    }


    public void BuildMe()
    {
        if (isBuilt)
        {
            if (t.builtNum < 2)
            {
                coeffDiffer = 1;
            }
            else
            {
                coeffDiffer = 0;
            }

            float bonus = sizeMod * coeffReward + 5 * coeffValue + 10 * coeffDiffer;
            GameController.Instance.totalCount += bonus;
            GameController.Instance.ShowDelta(bonus);
        }
    }

    public void KillMe()
    {
        if (GameController.Instance)
        {
            if (t.builtNum < 2)
            {
                coeffDiffer = 1;
            }
            else
            {
                coeffDiffer = 0;
            }

            float bonus = sizeMod * coeffReward + 5 * coeffValue + 10 * coeffDiffer;
            GameController.Instance.totalCount -= bonus;
            GameController.Instance.ShowDelta(-bonus);
        }
    }


    void Update()
    {
        /*if (isBuilt)
        {
            if(t.builtNum < 2)
            {
                coeffDiffer = 2;
            }
            else
            {
                coeffDiffer = 1;
            }
            //coeffDiffer = 1;

            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                timer = 1.0f;
                GameController.Instance.totalCount += (sizeMod * coeffReward + 5 * coeffValue + 10 * coeffDiffer);
            }
        }*/
    }
}
