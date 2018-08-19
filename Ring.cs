using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ring : MonoBehaviour
{


    public GameObject white;
    public GameObject red;
    public GameObject arrow;

    public float timeToChange;
    public float timer;

    public int counter = 3;

    public float currentScale;
    public float nextScale;

    public float nextX;
    public float nextZ;

    Vector3 nextWhite;

    public float alpha;

    public GameObject[] groupInside;
    public GameObject[] groupOutide;

    public void Reset()
    {
        foreach (var v in groupInside)
        {
            v.SetActive(true);
        }
        foreach (var v in groupOutide)
        {
            v.SetActive(false);
        }

        red.transform.localScale = new Vector3(287.5f, 500f, 287.5f);
        white.transform.localScale = new Vector3(287.5f, 500f, 287.5f);
        red.transform.position = new Vector3(0, 491.84f, 0);
        white.transform.position = new Vector3(0, 491.84f, 0);
        red.SetActive(true);
        white.SetActive(true);
        counter = 3;

        Start();
    }

    private void Start()
    {
        timer = timeToChange;
        currentScale = red.transform.localScale.x;
        nextScale = currentScale;

        alpha = Random.Range(0, 2 * Mathf.PI);

        nextX = red.transform.position.x;
        nextZ = red.transform.position.z;

        white.transform.localScale = new Vector3(nextScale / 2.0f, 500.0f, nextScale / 2.0f);
        nextWhite = white.transform.position;
        nextWhite =
            new Vector3(
                Random.Range(white.transform.position.x - nextScale / 2.0f * Mathf.Cos(alpha), white.transform.position.x + nextScale / 2.0f * Mathf.Cos(alpha)),
                nextWhite.y,
                Random.Range(white.transform.position.z - nextScale / 2.0f * Mathf.Sin(alpha), white.transform.position.z + nextScale / 2.0f * Mathf.Sin(alpha)));
        white.transform.position = nextWhite;

        CanvasManager.Instance.timerZone.text = "ZONE TIMER: " + timer.ToString("00");
    }

    IEnumerator Sound2()
    {
        yield return new WaitForSeconds(1.0f);
        SndManager.Instance.Play("Zone");
    }

    IEnumerator Sound3()
    {
        yield return new WaitForSeconds(2.0f);
        SndManager.Instance.Play("Zone");
    }

    private void Update()
    {
        if (GamePlayController.Instance.isG)
        {
            timer -= Time.deltaTime;
            if (counter > 0)
            {
                CanvasManager.Instance.timerZone.text = "ZONE TIMER: " + timer.ToString("00");
            }
            else
            {
                CanvasManager.Instance.timerZone.text = "ZONE TIMER: 00";
            }

            if (timer <= 0 && counter > 0)
            {
                timer = timeToChange;
                currentScale = red.transform.localScale.x;
                nextScale = currentScale / 2.0f;

                alpha = Random.Range(0, 2 * Mathf.PI);

                nextX = nextWhite.x; //nextX = Random.Range(red.transform.position.x - nextScale * Mathf.Cos(alpha), red.transform.position.x + nextScale * Mathf.Cos(alpha));//red.transform.position.x + nextScale * Mathf.Cos(alpha)
                nextZ = nextWhite.z; //nextZ = Random.Range(red.transform.position.z - nextScale * Mathf.Sin(alpha), red.transform.position.z + nextScale * Mathf.Sin(alpha));//red.transform.position.z + nextScale * Mathf.Sin(alpha)

                if (counter > 1)
                {
                    white.transform.localScale = new Vector3(nextScale / 2.0f, 500.0f, nextScale / 2.0f);
                    nextWhite = white.transform.position;
                    nextWhite =
                        new Vector3(
                            Random.Range(white.transform.position.x - nextScale / 2.0f * Mathf.Cos(alpha), white.transform.position.x + nextScale / 2.0f * Mathf.Cos(alpha)),
                            nextWhite.y,
                            Random.Range(white.transform.position.z - nextScale / 2.0f * Mathf.Sin(alpha), white.transform.position.z + nextScale / 2.0f * Mathf.Sin(alpha)));
                    white.transform.position = nextWhite;
                }
                else
                {
                    white.SetActive(false);
                }
                SndManager.Instance.Play("Zone");
                StartCoroutine(Sound2());
                StartCoroutine(Sound3());


                counter--;
            }

            //change
            red.transform.localScale = Vector3.Lerp(red.transform.localScale, new Vector3(nextScale, 500.0f, nextScale), 0.25f * Time.deltaTime);
            red.transform.position = Vector3.Lerp(red.transform.position, new Vector3(nextX, 491.84f, nextZ), 0.25f * Time.deltaTime);

            //damage
            var distance =
                (GamePlayController.Instance.player.gameObject.transform.position.x - red.transform.position.x) *
                (GamePlayController.Instance.player.gameObject.transform.position.x - red.transform.position.x) +
                (GamePlayController.Instance.player.gameObject.transform.position.z - red.transform.position.z) *
                (GamePlayController.Instance.player.gameObject.transform.position.z - red.transform.position.z);
            if (distance >= red.transform.localScale.x * red.transform.localScale.x)
            {
                GamePlayController.Instance.playerController.DamageTake(0.0f, 5.0f * Time.deltaTime);
                foreach (var v in groupInside)
                {
                    v.SetActive(false);
                }
                foreach (var v in groupOutide)
                {
                    v.SetActive(true);
                }
            }
            else
            {
                foreach (var v in groupInside)
                {
                    v.SetActive(true);
                }
                foreach (var v in groupOutide)
                {
                    v.SetActive(false);
                }
            }

            //стрелка до белой
            var distance2 =
                (GamePlayController.Instance.player.gameObject.transform.position.x - white.transform.position.x) *
                (GamePlayController.Instance.player.gameObject.transform.position.x - white.transform.position.x) +
                (GamePlayController.Instance.player.gameObject.transform.position.z - white.transform.position.z) *
                (GamePlayController.Instance.player.gameObject.transform.position.z - white.transform.position.z);
            if (distance2 >= white.transform.localScale.x * white.transform.localScale.x)
            {
                //GamePlayController.Instance.playerController.DamageTake(0.0f, 5.0f * Time.deltaTime);
                arrow.SetActive(true);
                //lookat
                arrow.transform.LookAt(new Vector3(white.transform.position.x, 0.91f, white.transform.position.z), Vector3.up);
            }
            else
            {
                arrow.SetActive(false);
            }

        }
    }




}
