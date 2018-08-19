using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotGeneral : MonoBehaviour
{

    public int baseHealth;
    public int baseDamage;

    public int maxHealth;
    public int maxDamage;
    public float bonus;

    public float currentHealth;

    //public int attackRate;

    public float attackDelay;
    public float attackTimer;
    public GameObject attackEffect;

    public bool isAlive;

    public GameObject dieEffect;
    public float dieTimer;

    public GameObject playerPos;
    public GameObject towerPos;

    public Vector3 newPosition;

    private float distanseForWalk = 3.0f;

    private List<Transform> waypoints;
    private int waypointIndex;
    public Vector3 newTransportPosition;

    bool ifmove;


    void Start()
    {
        ifmove = false;
        waypointIndex = 0;

        var wps = transform.parent.GetComponent<WaypointsForAuto>();
        if (wps)
        {
            waypoints = wps.waypoints;
        }

        if (wps)
        {
            if (waypoints.Count > 0)
            {
                ifmove = true;
                newTransportPosition = GetNextPosition();
            }
        }
        //сюда функцию выбора nextpoint и по кругу по waypoints



        maxHealth = baseHealth + (int)(bonus * GeneralScript.Instance.difficulty);
        maxDamage = baseDamage + (int)(bonus * GeneralScript.Instance.difficulty);

        currentHealth = maxHealth;
        isAlive = true;

        attackTimer = 0.0f;
        dieTimer = 0.0f;

        playerPos = GameObject.FindGameObjectWithTag("Player");


        newPosition = GetNewPosition();
    }

    private Vector3 GetNextPosition()
    {
        waypointIndex++;
        if (waypointIndex == waypoints.Count)
        {
            waypointIndex = 1;
        }

        return waypoints[waypointIndex].position;
    }

    private Vector3 GetNewPosition()
    {
        var spawnPoint = transform.parent.transform.position;
        var newPos = new Vector3(spawnPoint.x + Random.Range(-3.0f, 3.0f), 0, spawnPoint.z + Random.Range(-distanseForWalk, distanseForWalk));
        return newPos;
    }


    public void Die()
    {
        if (attackEffect)
            attackEffect.SetActive(false);
        GeneralScript.Instance.killed++;
        //  Instantiate(dieEffect);
        Instantiate(dieEffect, gameObject.transform.position, gameObject.transform.rotation);
        // if (dieTimer >= 1.0f)
        // {
        //можно после времени или триггера
        if (maxDamage < 4 && maxDamage > 0)
        {
            GetComponent<Animator>().SetTrigger("Die");
            GameManager.Instance.SoundPlay("ManDie");
            Destroy(gameObject, 3);
            // transform.LookAt(playerPos.transform );
        }
        else
        {
            GameManager.Instance.SoundPlay("TargetBoom");
            Destroy(gameObject, 1);
        }

        //}
    }


    public void GetDamage(float amount)
    {
        currentHealth -= amount;
        if (isAlive && currentHealth <= 0.0f)
        {
            isAlive = false;
            // Instantiate(dieEffect, gameObject.transform.position, gameObject.transform.rotation);
            Die();
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Gun"))
        {
            currentHealth -= GeneralScript.Instance.gunDMG;
            if (isAlive && currentHealth <= 0.0f)
            {
                isAlive = false;
                // Instantiate(dieEffect, gameObject.transform.position, gameObject.transform.rotation);
                Die();
            }
        }
        else if (collision.gameObject.CompareTag("Rocket"))
        {

        }


    }



    public void ShootPlayer()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackDelay)
        {
            if (maxDamage > 0)
            {
                attackEffect.SetActive(false);
                attackEffect.SetActive(true);
                //shoot
                if (maxDamage < 4)
                {
                    var lookPos = playerPos.transform.position - transform.position;
                    lookPos.y = 0;
                    var rotation = Quaternion.LookRotation(lookPos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5);
                    GameManager.Instance.SoundPlay("ManShoot");
                    // transform.LookAt(playerPos.transform );
                }
                else
                {
                    //башня
                    var lookPos = playerPos.transform.position - towerPos.transform.position;
                    lookPos.y = 0;
                    var rotation = Quaternion.LookRotation(lookPos);
                    towerPos.transform.rotation = Quaternion.Slerp(towerPos.transform.rotation, rotation, Time.deltaTime * 10);
                    GameManager.Instance.SoundPlay("TankShoot");
                }
            }
            attackTimer = 0.0f;
            GeneralScript.Instance.GetHit(maxDamage);

            newPosition = GetNewPosition();
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            if (maxDamage < 4 && maxDamage > 0)
            {
                transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 0.5f);
            }
            if (maxDamage == 0 && ifmove)
            {
                var lookPos = newTransportPosition - transform.position;
                // lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 3.75f);

                if (Time.deltaTime > 0)
                    transform.position = Vector3.Lerp(transform.position, newTransportPosition, 0.035f * Time.timeScale);
                //float distCovered = (Time.time - startTime) ;
                // float fracJourney = distCovered / journeyLength;
                //transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);

                if (Mathf.Abs(transform.position.x - newTransportPosition.x) <= 1.9f && Mathf.Abs(transform.position.z - newTransportPosition.z) <= 1.9f)
                {
                    newTransportPosition = GetNextPosition();
                }


            }
            ShootPlayer();
        }
        else
        {
            // dieTimer += Time.deltaTime;
            // Die();
            //die
        }

    }

    private int countCheck = 0;
    void FixedUpdate()
    {

        if (countCheck < 5)
        {
            countCheck++;
            return;
        }
        else
        {
            countCheck = 0;
            var distance = Vector3.ProjectOnPlane(transform.position - transform.parent.transform.position, Vector3.up).magnitude;
            if (distance > distanseForWalk)
                newPosition = GetNewPosition();
        }
    }
}
