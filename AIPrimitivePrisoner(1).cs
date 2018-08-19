using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
//using Managers;
using PoliceSniper;

namespace PrisonRiotSniper
{
    public class AIPrimitivePrisoner : Bot
    {

        public AISecurity targetEnemy;

        private Transform[] _waypoints;
        private Transform[] waypointsRobbers;

        private Waypoint _nextWaypoint;
        private int currentPoint;
        private int totalPoint;

        bool tauntNextTime;
        float tauntTimer;
        float randomTimer;
        bool isRandomed;
        bool isDontRandom;

        public Transform playerTransform;

        public bool isAgressive = false;
        public bool isAction = false;

        float Speed = 6; //3
        float speedMult = 1;
        float seeMult = 1;

        public AudioSource audio;

        // Use this for initialization
        void Start()
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

            isDontRandom = false;
            randomTimer = 15.0f;

            tauntTimer = 3.5f;
            tauntNextTime = false;
            _waypoints = PrisonRiotSniperGameController.prisonersWaypoints;
            waypointsRobbers = PrisonRiotSniperGameController.Instance.robberWaypoints[PlayerRandomPosition.instance.rand].waypoints;


            totalPoint = waypointsRobbers.Length;
            currentPoint = 0;

            int difficult = PlayerPrefs.GetInt("SelectedDifficult", 0);
            switch (difficult)
            {
                case 0:
                    speedMult = 1.0f;
                    seeMult = 1.0f;
                    break;
                case 1:
                    speedMult = 1.5f;
                    seeMult = 1.0f;
                    break;
                case 2:
                    speedMult = 2.0f;
                    seeMult = 1.5f;
                    break;
                case 3:
                    speedMult = 2.5f;
                    seeMult = 2.0f;
                    break;
            }

            SetState(State.run);

            // MoveToNextRandomWaypoint();

            wanderTimer = 3.0f;
            Vector3 randPos = Vector3.left * Random.Range(-5f, 5f) + Vector3.forward * Random.Range(-5f, 5f);
            agent.SetDestination(gameObject.transform.position + randPos);
        }


        bool isStay;
        public float wanderTimer = 3.0f;
        //бродим
        void Wander()
        {
            wanderTimer -= Time.deltaTime;
            //каждые 3 сек = ходим или стоим
            if (wanderTimer <= 0.0f)
            {
                agent.speed = 1.5f; //3
                SetState(State.walk);////SetState(State.run);
                Vector3 randPos = Vector3.left * Random.Range(-5f, 5f) + Vector3.forward * Random.Range(-5f, 5f);
                agent.SetDestination(gameObject.transform.position + randPos);
                wanderTimer = 5.0f;
            }

            if (myState != State.stay && (agent.remainingDistance <= 1.0f || agent.isPathStale == true))
            {
                wanderTimer = 3.0f;


                SetState(State.stay);
            }

        }

        //убегаем или догоняем
        void Action()
        {
            if (isAgressive)
            {
                //догоняем
                //if(!GameManager.instance.btnShop.gameObject.active)
                //{
                agent.SetDestination(playerTransform.position);

                agent.speed = Speed * speedMult;
                SetState(State.run);
                //}

                if (Vector3.Distance(transform.position, playerTransform.position) <= 3)
                {
                    audio.mute = true;
                    audio.loop = false;
                    GameManager.instance.LoseGame();
                    Destroy(this);
                }
            }
            else
            {
                //убегаем
                float runningRange = 0.5f;
                Vector3 targetDestination = transform.position + (transform.position - playerTransform.position) * 2.0f + new Vector3(Random.Range(-runningRange, runningRange), 0, Random.Range(-runningRange, runningRange));
                agent.SetDestination(targetDestination + Vector3.up * 2f);


                agent.speed = Speed * 1.5f;
                SetState(State.run);
            }

        }



        // Update is called once per frame
        void Update()
        {

            if (isAction)
            {
                if (Time.timeScale != 0)
                {
                    audio.mute = false;
                }
                else
                {
                    audio.mute = true;
                }
                Action();
            }
            else
            {
                audio.mute = true;
                Wander();
            }

            //если 20 метров = action
            if (isAgressive)
            {
                if (!GameManager.instance.btnShop.gameObject.active && Vector3.Distance(transform.position, playerTransform.position) <= 20 * seeMult)
                {
                    isAction = true;
                }
                else
                {
                    isAction = false;
                }
            }
            else
            {
                if (!GameManager.instance.btnShop.gameObject.active && Vector3.Distance(transform.position, playerTransform.position) <= 20)
                {
                    isAction = true;
                }
                else
                {
                    isAction = false;
                }
            }

            //   base.StateBehaviour();
        }


        public void MoveToNextRandomWaypoint()
        {
            Vector3 randPos = Vector3.left * Random.Range(-5f, 5f) + Vector3.forward * Random.Range(-5f, 5f);
            agent.SetDestination(waypointsRobbers[currentPoint].position + randPos);
            if (currentPoint < totalPoint - 1)
            {
                currentPoint++;
            }
            else
            {
                currentPoint = 0;
            }
            SetState(State.run);
        }

    }
}
