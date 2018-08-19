using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class KabaddiPlayer : MonoBehaviour
{

    public GameObject SpeedUp;
    public GameObject player;
    float speed = 5.0f;
    NavMeshAgent agent;
    Animator anim;
    Vector3 startPosition;
    bool ifStartPos;
    public bool isWandering;
    bool isBoosted;
    public bool isHittable;
    float boostTimer;
    Vector3 wanderingDest;
    public float speedModificator = 1f;

    bool isHit;
    float hitTimer;

    float boostSpeed;

    // Use this for initialization
    void Start()
    {
        isHittable = true;
        isHit = false;
        hitTimer = 0.0f;
        boostSpeed = 1.0f;
        isWandering = true;
        isBoosted = false;
        boostTimer = 2.0f;
        ifStartPos = true;
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        startPosition = transform.position;
        wanderingDest = transform.position;
        // 
        speedModificator = GBNHZinit.GetDelay("botspeed");
        if (speedModificator == 0) speedModificator = 0.9f;
        agent.speed = speed * speedModificator;

        //смена скина
        var mySkinId = GameController.Instance.isDebug ? UnityEngine.Random.Range(0, 8) : GameController.Instance.saverGameData.gameData.currentSkin.Value;
        var changerSkin = GetComponent<ChangerSkin>();
        if (changerSkin)
        {
            if (mySkinId == 7)
                changerSkin.SetSkin(1);
            else
                changerSkin.SetSkin(0);
        }
    }


    void RunAway()
    {

        //здесь можно зарандомить ускорение (но 1 раз)
        if (!isBoosted)
        {
            // boostTimer = 2.0f;
            if (Random.Range(0.0f, 100.0f) < 20.0f) //20%
            {
                boostSpeed = 1.5f;
                agent.speed = speed * speedModificator * 1.5f;
                SpeedUp.SetActive(true);
            }
            else
            {
                boostSpeed = 1.0f;
                agent.speed = speed * speedModificator;
            }
            isBoosted = true;
        }
        else
        {
            // agent.speed = speed * speedModificator;
        }

        if (boostSpeed > 1.2f)
        {
            //anim.Play("Sprint");
            //anim.SetTrigger("Sprint");
            anim.SetInteger("State", 0);
        }
        else
        {
            //anim.Play("Running");
            //anim.SetTrigger("Running");
            anim.SetInteger("State", 1);
        }
        float runningRange = 0.5f;
        // 
        //Vector3 targetDestination = player.transform.forward * 5;// /*player.transform.TransformDirection(5*transform.forward)*/ + new Vector3(Random.Range(-runningRange, runningRange), 0, Random.Range(-runningRange, runningRange));
        // Use this targetDestination to where you want to move your enemy NavMesh Agent

        Vector3 targetDestination = transform.position + (transform.position - player.transform.position) + new Vector3(Random.Range(-runningRange, runningRange), 0, Random.Range(-runningRange, runningRange));

        agent.SetDestination(targetDestination);
        // if (player.transform.position - )
        /*
        
        var t1 = agent.destination;
        agent.destination = targetDestination;
        var t2 = agent.destination;*/
        //Debug.Log("dgfdfgdg" + agent.destination); 
    }


    float stateTimer = 1.0f;
    int state = 0;

    void RandomLookat()
    {
        agent.speed = speed * speedModificator / 2.0f;
        // anim.Play("Idle", 0, 0.57f);
        float rotationSpeed = 10f;
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        //state=0;

        float runningRange = 3.5f;
        /*
        if (Vector3.Distance(wanderingDest, transform.position) >= 0.1f)
        {
            // Use this targetDestination to where you want to move your enemy NavMesh Agent
            //   agent.SetDestination(wanderingDest);
            //agent.destination = wanderingDest;
            //transform.position = //transform.right * Time.deltaTime;
            //  Vector3.MoveTowards(transform.position, wanderingDest, Time.deltaTime);
            //transform.position += new Vector3(0.1f* Time.deltaTime, 0, 0);
            transform.position += transform.right * 0.5f * Time.deltaTime;
        }
        else
        {
            //новая позиция
            // wanderingDest = transform.TransformDirection(transform.right) + new Vector3(Random.Range(-runningRange, runningRange), 0, Random.Range(-runningRange, runningRange));
            wanderingDest = transform.position + new Vector3(Random.Range(-runningRange, runningRange), 0, Random.Range(-runningRange, runningRange));
        }
        */
        if (stateTimer <= 0)
        {
            stateTimer = 1.0f;
            state = Random.Range(0, 4);
        }
        else
        {
            stateTimer -= Time.deltaTime;
            switch (state)
            {
                //прямо
                case 0:
                    transform.position += transform.forward * 0.5f * Time.deltaTime;
                    //anim.Play("Walk");
                    //  anim.SetTrigger("Walk");
                    anim.SetInteger("State", 5);
                    break;
                //назад
                case 1:
                    transform.position -= transform.forward * 0.5f * Time.deltaTime;
                    //anim.Play("Back");
                    //  anim.SetTrigger("Back");
                    anim.SetInteger("State", 4);
                    break;
                //лево
                case 2:
                    transform.position -= transform.right * 0.5f * Time.deltaTime;
                    //anim.Play("Left");
                    // anim.SetTrigger("Left");
                    anim.SetInteger("State", 2);
                    break;
                //право
                case 3:
                    transform.position += transform.right * 0.5f * Time.deltaTime;
                    //anim.Play("Right");
                    //anim.SetTrigger("Right");
                    anim.SetInteger("State", 3);
                    break;
                default:
                    break;
            }


        }
    }



    public void GetHit()
    {
        isHittable = false;
        isHit = true;
        hitTimer = 0.9f;
    }



    // Update is called once per frame
    void Update()
    {
        if (isBoosted)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0.0f)
            {
                //isBoosted = false;
                SpeedUp.SetActive(false);
                isBoosted = false;
            }
            // boostSpeed = 1.5f;
        }
        else
        {
            // boostSpeed = 1.0f;
            boostTimer = 2.0f;
        }


        if (isHit)
        {
            agent.destination = transform.position += transform.forward * 0.001f;
            transform.position -= transform.forward * 0.1f * Time.deltaTime;
            hitTimer -= Time.deltaTime;
            //anim.Play("Hit");
            //anim.SetTrigger("Hit");
            anim.SetInteger("State", 6);
            if (hitTimer <= 0.0f)
            {
                isHit = false;
            }
        }
        else

            if (GameController.Instance.isCarryIt)
            {
                //преследуем
                agent.speed = speed * speedModificator;
                //anim.Play("Running");
                // anim.SetTrigger("Running");
                anim.SetInteger("State", 1);
                agent.destination = player.transform.position;
                ifStartPos = false;
                isWandering = false;
                isHittable = false;
            }
            else
            {
                if (Vector3.Distance(player.transform.position, transform.position) <= 2.5f)
                {
                    isWandering = true;//true
                    ifStartPos = false;
                    //игрок близко
                    RunAway();
                    isHittable = true;
                }
                else
                {
                    if (Vector3.Distance(startPosition, transform.position) >= 1.0f && !isWandering)
                    {
                        //бежим обратно
                        if (!ifStartPos)
                        {
                            isHittable = false;
                            isWandering = false;
                            agent.speed = speed * speedModificator;
                            //anim.Play("Running");
                            //anim.SetTrigger("Running");
                            anim.SetInteger("State", 1);
                            agent.destination = startPosition;
                        }
                        else
                        {
                            isHittable = true;
                            isWandering = true;
                            // RandomLookat();
                        }
                    }
                    else
                    {
                        isHittable = true;
                        isWandering = true;
                        ifStartPos = true;
                        //рандомно ходим вокруг точки
                        RandomLookat();
                    }



                }


            }

    }
}
