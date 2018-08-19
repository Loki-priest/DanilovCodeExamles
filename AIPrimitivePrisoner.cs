using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace PrisonRiotSniper
{
    public class AIPrimitivePrisoner : Bot
    {
        // public MeshRenderer knife;
        //  public bool isFight = false;
        //  public bool isFighting = false;
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



        // Use this for initialization
        void Start()
        {
            isDontRandom = false;
            randomTimer = 15.0f;

            tauntTimer = 3.5f;
            tauntNextTime = false;
            _waypoints = PrisonRiotSniperGameController.prisonersWaypoints;
            if (PlayerRandomPosition.instance.rand == 1)
                waypointsRobbers = PrisonRiotSniperGameController.Instance.robberWaypoints[Random.Range(0, 3)].waypoints;
            else
                waypointsRobbers = PrisonRiotSniperGameController.Instance.robberWaypoints[Random.Range(3, 6)].waypoints;
            totalPoint = waypointsRobbers.Length;
            currentPoint = 0;
            SetState(State.run);
            // MoveToClosestRightWaypoint();
            MoveToNextRandomWaypoint();
        }

        /*
        bool IsPathComplete()
        {

        }
        */
        // Update is called once per frame
        void Update()
        {
            //if (isFight)
            //{
            //    agent.SetDestination(targetPoint.position);
            //}

            randomTimer -= Time.deltaTime;



            if (pathComplete())
            {
                tauntTimer -= Time.deltaTime;
                if (!tauntNextTime)
                {
                    if (tauntTimer <= 0)
                        MoveToNextRandomWaypoint();
                }
                else
                {//или дразнить
                    transform.LookAt(PlayerRandomPosition.instance.gameObject.transform);
                    agent.Stop();
                    SetState(State.stay);
                    tauntNextTime = false;
                }
            }
            else
            {
                if (Random.Range(0, 4) == 0 && !isDontRandom)
                {
                    tauntNextTime = true;
                    tauntTimer = 3.5f;
                    isDontRandom = true;
                }
                if (randomTimer <= 0)
                {
                    isDontRandom = false;
                    randomTimer = 15.0f;
                }
            }




            //   base.StateBehaviour();
        }
        /*
        public void GoToFight(Bot enemy)
        {
            if (enemy == null || !isAlive())
                return;
            isFight = true;
            enemy.DieHandler += EndFight;
            DieHandler += EndFight;
            knife.enabled = true;
            targetEnemy = (AISecurity)enemy;
            targetPoint = enemy.transform;
            agent.speed = 2.5f;
            StartCoroutine("MoveToTarget");
            //agent.SetDestination(targetPoint.position);
            SetStateForced(State.run);
        }

        public void EndFight(Bot enemy)
        {
            isFight = false;
            isFighting = false;
            enemy.DieHandler -= EndFight;
            DieHandler -= EndFight;
            //StopCoroutine("MoveToTarget");
            if (isAlive())
            {
                Bot newEnemy = SearchNewEnemy();
                if (newEnemy != null)
                {
                    GoToFight(newEnemy);
                    return;
                }
                targetEnemy = null;
                agent.speed = 1.5f;
                MoveToNextRandomWaypoint();
            }
        }
        */
        public void MoveToNextRandomWaypoint()
        {
            Vector3 randPos = Vector3.left * Random.Range(-5f, 5f) + Vector3.forward * Random.Range(-5f, 5f);
            agent.SetDestination(waypointsRobbers[currentPoint].position + randPos);
            if (currentPoint < totalPoint - 1)
            {
                currentPoint++;
            }
            SetState(State.run);
        }
        /*
        public void MoveToNextRandomWaypoint()
        {
            Vector3 randPos = Vector3.left * Random.Range(-6f, 6f) + Vector3.forward * Random.Range(-6f, 6f); ;
            _nextWaypoint = _nextWaypoint.neighbours[Random.Range(0, _nextWaypoint.neighbours.Length)];
            targetPoint = _nextWaypoint.transform;
            agent.SetDestination(targetPoint.position + randPos);
            SetState(State.run);
        }

        public void MoveToClosestRightWaypoint()
        {
            float closeDistance = 1000;
            for (int i = 0; i < _waypoints.Length; i++)
            {
                float temp = Vector3.Distance(transform.position, _waypoints[i].position);
                if (temp < closeDistance)
                {
                    closeDistance = temp;
                    targetPoint = _waypoints[i];
                }                
            }

            if (targetPoint != null)
            {
                Vector3 randPos = Vector3.left * Random.Range(-6f, 6f) + Vector3.forward * Random.Range(-6f, 6f);
                _nextWaypoint = targetPoint.GetComponent<Waypoint>();
                _nextWaypoint = _nextWaypoint.neighbours[Random.Range(0, _nextWaypoint.neighbours.Length)];
                targetPoint = _nextWaypoint.transform;
                agent.SetDestination(targetPoint.position + randPos);
                SetState(State.run);
            }

        }
        */
        /*
        public Bot SearchNewEnemy()
        {
            //return BotController.instance.GetBots(BotGenerator.BotType.swat).First(b => b.isAlive() == true);
            return BotController.instance.GetBots(BotGenerator.BotType.swat).Find(b => (b.isAlive()));
        }

        IEnumerator MoveToTarget()
        {
            while (isFight)
            {
                agent.SetDestination(targetPoint.position);
                yield return new WaitForSeconds(0.2f);
            }

        }
        */
    }
}
