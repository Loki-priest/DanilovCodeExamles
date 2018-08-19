using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using System.Linq;
using DG.Tweening;
using System;
using DigitalRuby.SoundManagerNamespace;
using UnityEngine.AI;

public class GameController : Singleton<GameController>
{

    [Header("Settings Scenes")]
    [HideInInspector]
    public float fadeDuration = 0.5f;
    public string nameMenuScene = "Menu";
    public string nameGameScene = "Game";
    public bool isDebug = false;
    public bool useCountdown = false;

    [Space(10)]
    [SerializeField]
    GameObject world;

    [Header("Popup Panels")]
    public GameObject popupPause;
    public GameObject popupEndGame;
    public GameObject popupRecords;
    public GameObject countdown;
    public Tutorial tutorial;

    public List<Camera> mainCameras;
    [HideInInspector]
    public SaverGameData saverGameData;

    [Space(10)]
    [SerializeField]
    private SaverGameData.TypeGame typeGame;

    public float turnTimer;
    public int turn;
    public bool isCarryIt = false;
    public bool isAnyPenalty = false;
    public int scoreTotal;

    [Header("Kabaddi GUI")]
    public GameObject canvasUI;
    public GameObject bonusTimer;
    public Text timer;
    public Text tutorialHints;
    public Text score;
    public Text rounds;
    //public Tutorial tutorial;

    [Header("Kabaddi GO")]
    public GameObject platform;
    public GameObject player;
    public GameObject location;
    public GameObject leaves;


    public static List<NavMeshAgentType> agentTypes;



    protected override void Awake()
    {
        base.Awake();
        InitNavMeshAgentType();


        if ((float)Screen.width / (float)Screen.height < 1.5f)
        {
            foreach (var mainCamera in mainCameras)
            {
                mainCamera.fieldOfView = 1.2f;
            }


        }
        else
        {
            foreach (var mainCamera in mainCameras)
            {
                mainCamera.fieldOfView = 1.0f;
            }
        }

        if (!isDebug) saverGameData = SaverGameData.Instance;
        if (world) Instantiate(world);
        typeGame = isDebug ? typeGame : saverGameData.typeGame;

        if (!isDebug)
        {
            if (saverGameData.isReplay)
            {

                Debug.Log("replay");
            }
            else
            {

                Debug.Log("start");
            }
            //или реплей
            //
            saverGameData.isReplay = false;
        }

        InitPlayers();
        InitLocation();

        turn = 0;
        isCarryIt = false;
        isAnyPenalty = false;
        scoreTotal = 0;
        AddScore(0);
        ResetTimer();


    }

    public void AddScore(int val)
    {
        scoreTotal += val;
        if (val < 0)
        {
            GameObject g = Instantiate(bonusTimer) as GameObject;
            g.GetComponent<Text>().text = val.ToString();
            //g.transform.parent = canvasUI.transform ;
            g.transform.SetParent(canvasUI.transform, false);
            // g.transform.position = new Vector3(0, 0, 0);
            SoundMng.Instance.PlaySound(SoundMng.SoundType.LosePoint);
        }
        if (val > 0)
        {
            GameObject g = Instantiate(bonusTimer) as GameObject;
            g.GetComponent<Text>().text = "+" + val;
            // g.transform.parent = canvasUI.transform;
            g.transform.SetParent(canvasUI.transform, false);
            //  g.transform.position = new Vector3(0, 0, 0);
            SoundMng.Instance.PlaySound(SoundMng.SoundType.GetPoint);
        }
        UpdateUI();

        //если очков отрицательно и нельзя их восстановить - то луз.
        /*
         6 -2 
         5 -4
         4 -6
         3 -8
         2 -10
         1 -12
         0 -14

        if(scoreTotal < -(7-turn)*2) 
        {
            EndGame();
        }

       */


    }

    public void CompleteTurn()
    {
        isAnyPenalty = false;
        turn++;
        DeativateRunning();
        ResetTimer();
        UpdateUI();

        if (turn == 7)
        {
            EndGame();
        }

    }

    public void UpdateUI()
    {
        score.text = "Score: " + scoreTotal;
        rounds.text = turn + " / " + 7;
        if (isCarryIt)
        {
            tutorialHints.text = "Run to your base and don't be caught!";
        }
        else
        {
            tutorialHints.text = "Touch the enemy player";
        }
    }

    public void DeativateRunning()
    {
        isCarryIt = false;
        UpdateUI();
    }

    public void ActivateRunning()
    {
        isCarryIt = true;
        UpdateUI();
    }


    private void InitNavMeshAgentType()
    {
        agentTypes = new List<NavMeshAgentType>();
        for (var i = 0; i < NavMesh.GetSettingsCount(); i++)
        {
            var id = NavMesh.GetSettingsByIndex(i).agentTypeID;
            var name = NavMesh.GetSettingsNameFromID(id);
            agentTypes.Add(new NavMeshAgentType(name, id));
        }
    }

    private void InitPlayers()
    {
        var mySkinId = isDebug ? UnityEngine.Random.Range(0, 8) : saverGameData.gameData.currentSkin.Value;
        var changerSkin = player.GetComponent<ChangerSkin>();
        if (changerSkin)
            changerSkin.SetSkin(mySkinId);
    }

    private void InitLocation()
    {
        var mySkinId = isDebug ? UnityEngine.Random.Range(0, 4) : saverGameData.gameData.currentGate.Value;
        var changerSkin = location.GetComponent<ChangerSkin>();
        if (changerSkin)
        {
            changerSkin.SetSkin(mySkinId);
            if (mySkinId == 3)
            {
                leaves.SetActive(true);
            }
        }
    }


    void Start()
    {
        if (!isDebug)
        {
            saverGameData.SaveGame();
            saverGameData.FaderOn(false, fadeDuration);
        }


        Observable.Interval(TimeSpan.FromSeconds(fadeDuration))
            .Take(1)
            .Subscribe(_ =>
            {
                // tutorial.ShowPage(0);   //туториал, можно вернуть
                if (useCountdown)
                    countdown.SetActive(true);
                else
                    StartGame();
                if (!isDebug) SoundMng.Instance.PlayMusic(1);
            })
            .AddTo(this);
    }

    public void StartGame()
    {
        if (useCountdown)
        {
            StartCoroutine(HelpFade.FadeImage(countdown.GetComponent<Image>(), fadeDuration, new Color(0, 0, 0, 0f)));
            Observable.Interval(TimeSpan.FromSeconds(fadeDuration))
                .Take(1)
                .Subscribe(_ => countdown.SetActive(false))
                .AddTo(this);
        }
    }


    public void EndGame()
    {

        // if (!isDebug && scoreTotal>0) saverGameData.gameData.money.Value += scoreTotal*10;
        if (!isDebug && scoreTotal > 0) saverGameData.gameData.money.Value += scoreTotal * SaverGameData.Instance.moneyPerPoint;
        OpenPopup(popupEndGame);
        SoundMng.Instance.PlaySound(SoundMng.SoundType.EndRace);

    }

    public void CheatEndRace()
    {

    }

    public void OpenPopupPause()
    {

        SoundMng.Instance.PlaySound(SoundMng.SoundType.Click);
        OpenPopup(popupPause);
    }

    public void OpenPopupRecords()
    {
        SoundMng.Instance.PlaySound(SoundMng.SoundType.Click);
        OpenPopup(popupRecords);
    }


    private void OpenPopup(GameObject go)
    {
        var popup = Instantiate(go) as GameObject;
        popup.SetActive(true);
        popup.transform.localScale = Vector3.zero;
        popup.transform.SetParent(isDebug ? GameObject.Find("CanvasUI").GetComponent<Canvas>().transform : SaverGameData.Instance.Canvas.transform, false);
        popup.transform.SetAsLastSibling();
        popup.transform.SetSiblingIndex(isDebug ? popup.transform.GetSiblingIndex() - 1 : popup.transform.GetSiblingIndex() - 2);
        popup.GetComponent<PanelPopup>().Open();
    }

    public void LoadMenu(bool goInShop = false)
    {
        SaverGameData.Instance.goInShop = goInShop;
        LoadScene(nameMenuScene);

    }

    public void LoadRestart()
    {
        LoadScene(nameGameScene);

    }

    private void LoadScene(string idScene)
    {

        if (tutorial)
        {
            Destroy(tutorial.gameObject);
        }
        var saverGameData = SaverGameData.Instance;
        saverGameData.SaveGame();
        saverGameData.FaderOn(true, fadeDuration, () =>
        {
            SoundManager.StopAll();
            Destroy(SoundMng.Instance.gameObject);
            LoadingManager.LoadScene(idScene);
        });
    }

    public class NavMeshAgentType
    {
        public string name;
        public int id;

        public NavMeshAgentType(string name, int id)
        {
            this.name = name;
            this.id = id;
        }
    }

    public void ResetTimer()
    {
        turnTimer = 30.0f;
    }

    void Update()
    {
        turnTimer -= Time.deltaTime;
        timer.text = turnTimer.ToString(" 0 ");
        if (turnTimer <= 0.0f && turnTimer >= -5.0)
        {
            AddScore(-1);
            // isAnyPenalty = true;
            turnTimer = -10.0f;
            // CompleteTurn();
        }
        if (turnTimer <= -10.0f)
        {
            timer.text = " 0 ";
        }
    }



}
