using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;

public class jumping : MonoBehaviour
{
    public float startTimer;
    public int currentPoint = 0;
    public List<GameObject> tempPoints;
    public GameObject[] stairPoints;
    public GameObject stairPrefab;
    public GameObject stairParent;
    private GameObject nextPoint;
    private bool isReached = true;
    private Vector3 jumpTo;
    private bool isUp;
    public float speed;

    public GameObject textPrefab;

    public GameObject ballSkin;
    public GameObject ballDeath;

    Rigidbody rb;

    private IEnumerator Lose()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        GameController.Instance.EndRound();
    }

    public void KillBall()
    {
        //ballDeath.SetActive(true);
        ballSkin.GetComponent<MeshRenderer>().enabled = false;
        rb.isKinematic = true;//
        speed = 0.0f;
        Time.timeScale = 1.0f;
        GameController.Instance.freezeTimer = 0.0f;
        StartCoroutine(Lose());
    }

    Vector3 myPos;

    // Use this for initialization
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        speed = 1.0f;//1
        //nextPoint = stairPoints[0].GetComponentInChildren<GameObject>();
        nextPoint = stairPoints[0].transform.GetChild(0).gameObject;
        startTimer = 2.0f;

        isUp = true;

        jumpTo = new Vector3(
    transform.position.x + (nextPoint.transform.position.x - transform.position.x) / 2.0f,
    nextPoint.transform.position.y + 3.0f,
    transform.position.z + (nextPoint.transform.position.z - transform.position.z) / 2.0f
    );

        tempPoints = new List<GameObject>();
        //tempPoints = stairPoints;
        for (int i = 0; i < stairPoints.Length; i++)
        {
            tempPoints.Add(stairPoints[i]);
        }
    }

    private int stairCount = 17;

    private void NewStair()
    {
        //if (stairCount < 5)
        // {
        //var newStair = Instantiate(stairPrefab, stairPrefab.transform);

        var newStair = Instantiate(
                stairPrefab
            //new Vector3(tempPoints[tempPoints.Count - 1].transform.position.x + 0.0f, tempPoints[tempPoints.Count - 1].transform.position.y + 1.0f, tempPoints[tempPoints.Count - 1].transform.position.z - 1.0f),
            //tempPoints[tempPoints.Count - 1].transform.rotation
            //Quaternion.identity,
            //stairParent.transform
                );

        newStair.transform.parent = stairParent.transform;
        newStair.transform.position = new Vector3(tempPoints[tempPoints.Count - 1].transform.position.x + 0.0f, tempPoints[tempPoints.Count - 1].transform.position.y + 0.5f, tempPoints[tempPoints.Count - 1].transform.position.z - 1.0f);

        //newStair.transform.GetChild(0).transform.localPosition = new Vector3(Random.Range(-0.4f,0.4f), 0.5f, 0); // автопрыжок
        newStair.transform.GetChild(0).transform.localPosition = new Vector3(0, 0.5f, 0); // просто прыжок

        stairCount++;
        newStair.name = "Stair" + stairCount;
        //tempPoints[tempPoints.Count-1].transform.loca

        tempPoints.Add(newStair);

        // }
        //else
        // {
        if (tempPoints.Count > 20)
        {
            var tmp = tempPoints[0];
            tempPoints.Remove(tempPoints[0]);
            Destroy(tmp);
            //stairCount--;
        }
        // }

        myPos = transform.position;
    }

    private void NextPoint()
    {
        // currentPoint++;
        //if(currentPoint>= tempPoints.Count)
        // {
        //     currentPoint = tempPoints.Count - 1;
        //}
        currentPoint = tempPoints.Count - 14;    //

        nextPoint = tempPoints[currentPoint].transform.GetChild(0).gameObject;
        jumpTo = new Vector3(
            tempPoints[currentPoint - 1].transform.GetChild(0).gameObject.transform.position.x + (nextPoint.transform.position.x - tempPoints[currentPoint - 1].transform.GetChild(0).gameObject.transform.position.x) / 2.0f,
            nextPoint.transform.position.y + 3.0f,
            tempPoints[currentPoint - 1].transform.GetChild(0).gameObject.transform.position.z + (nextPoint.transform.position.z - tempPoints[currentPoint - 1].transform.GetChild(0).gameObject.transform.position.z) / 2.0f
            );
    }

    private void JumpToOther()
    {
        //transform.position += transform.up;
        //transform.position = Vector3.Lerp(transform.position, nextPoint.transform.position, .2f);
        currentPoint = tempPoints.Count - 14;
        //nextPoint = tempPoints[currentPoint].transform.GetChild(0).gameObject;
        jumpTo = new Vector3(
    tempPoints[currentPoint - 1].transform.GetChild(0).gameObject.transform.position.x + (nextPoint.transform.position.x - tempPoints[currentPoint - 1].transform.GetChild(0).gameObject.transform.position.x) / 2.0f,
    nextPoint.transform.position.y + 3.0f,
    tempPoints[currentPoint - 1].transform.GetChild(0).gameObject.transform.position.z + (nextPoint.transform.position.z - tempPoints[currentPoint - 1].transform.GetChild(0).gameObject.transform.position.z) / 2.0f
    );

        if (isUp)
        {
            transform.position = Vector3.Lerp(transform.position, jumpTo, .1f * speed * Time.timeScale);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, nextPoint.transform.position, .1f * speed * Time.timeScale);
        }

        //if (Vector3.Distance(transform.position, jumpTo) <= 0.5f) //определить, когда идти вниз
        if (Mathf.Abs(transform.position.y - jumpTo.y) <= 0.5f)
        {
            isUp = false;
        }
    }


    Vector3 SavedVelocity = new Vector3(0, 0, 0);
    private void FixedUpdate()
    {
        startTimer -= Time.deltaTime;

        if (startTimer <= 0 && speed > 0.5f)
        {
            var newVelocity = rb.velocity;

            newVelocity.z = -25.0f; //30
            rb.velocity = newVelocity;
            //rb.AddRelativeForce(Vector3.forward * -10);
            newVelocity.x = -15.0f * CnInputManager.GetAxis("Horizontal");
            //rb.AddRelativeForce(-0.5f * CnInputManager.GetAxis("Horizontal"), 0, 0, ForceMode.VelocityChange);
            //if (transform.position.x >= -5f && transform.position.x <= 5f)
            //{
            if (rb.velocity.y != 0)
            {
                SavedVelocity.y = newVelocity.y;
            }
            newVelocity.y = SavedVelocity.y;
            rb.velocity = newVelocity;


            Physics.gravity = new Vector3(0, -25, 0);
            if (Time.timeScale < 0.3f || GameController.Instance.isPaused)
            {
                if (rb.velocity.y != 0)
                {
                    SavedVelocity.y = rb.velocity.y / 1.25f;
                }
                Physics.gravity = new Vector3(0, 0, 0);
                rb.velocity = new Vector3(0, 0, 0);
            }



        }


    }
}