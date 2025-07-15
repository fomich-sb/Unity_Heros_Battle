using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;
using Cinemachine;
using Zenject;

public class GameController : MonoBehaviour
{
    public GameObject HeroRoot;
    public GameObject EnemyRoot;
    public bool HeroNotDead = false;

    [HideInInspector] public HeroController HeroController;
    [HideInInspector] public EnemyController EnemyController;
    private Health _heroHealth;
    private Health _enemyHealth;

    public GameStatus Status = GameStatus.Market;

    [SerializeField] private GameObject _marketCanvas;
    [SerializeField] private GameObject _battleCanvas;
    [SerializeField] private GameObject _enemyHealthCanvas;
    [SerializeField] private GameObject _prepareCanvas;
    [SerializeField] private GameObject _starsCanvas;
    [SerializeField] private GameObject _prepareBattleButton;
    [SerializeField] private Text _prepareBattleCountDown;
    [SerializeField] private GameObject _endCanvas;
    [SerializeField] private Text _endTextWin;
    [SerializeField] private Text _endTextLoss;

    public GameMode Mode = GameMode.Offline;

    [Inject] private Market _market;

    [HideInInspector] public float HerosDistance = 10;

    [SerializeField] private Transform _battleCameraTargetTransform;
    private Vector3 _battleCameraTargetPositionStart;

    [SerializeField] private CinemachineVirtualCamera battleVirtalCamera;
    [SerializeField] private CinemachineVirtualCamera marketVirtalCamera;
    [SerializeField] private CinemachineVirtualCamera heroWinVirtalCamera;
    [SerializeField] private CinemachineVirtualCamera enemyWinVirtalCamera;
    [SerializeField] private float _timeScale = 1;
    [SerializeField] protected float updateHeroNextStepInterval = 0.1f;


    private Dictionary<int, GameStatus> statusAccordance = new Dictionary<int, GameStatus>
    {
        {0, GameStatus.Market},
        {1, GameStatus.Prepare},
        {2, GameStatus.Main},
        {3, GameStatus.End},
    };

    private void Awake()
    {
        Time.timeScale = _timeScale;
        HeroController = HeroRoot.GetComponent<HeroController>();
        EnemyController = EnemyRoot.GetComponent<EnemyController>();
        _heroHealth = HeroRoot.GetComponent<Health>();
        _enemyHealth = EnemyRoot.GetComponent<Health>();

        HeroController?.Init(EnemyController);
        EnemyController?.Init(HeroController);
        _heroHealth.OnDead += EndBattle;
        _enemyHealth.OnDead += EndBattle;
        SetStatus(GameStatus.Market);
        SetMode(GameMode.Offline);
        _battleCameraTargetPositionStart = _battleCameraTargetTransform.position;

        StartCoroutine(UpdateHeroNextStep());
    }

    private void Update()
    {
        HerosDistance = Vector3.Distance(HeroController.transform.position, EnemyController.transform.position);
        SetBattleCameraPosition();
    }

    private void SetBattleCameraPosition()
    {
        if(Status == GameStatus.Main)
            _battleCameraTargetTransform.position = _battleCameraTargetPositionStart + new Vector3(0, 0, (HeroController.transform.position.z + EnemyController.transform.position.z) / 2 / 2);
    }

    public void SetStatus(int status)
    {
        if (!statusAccordance.ContainsKey(status)) return;

        GameStatus newStatus = statusAccordance[status];

        if (Status == newStatus) return;

        if (newStatus == GameStatus.Main)
            CountDownStart();
        else
            SetStatus(newStatus);
    }

    public void SetStatus(GameStatus status)
    {
        Status = status;

        if (status != GameStatus.End)
        {
            HeroController.Restart();
            EnemyController.Restart();
        }
        EnemyRoot.SetActive(Status != GameStatus.Market);

        UpdateCamerasPriority();
        UpdateUIVisibility();

        Time.timeScale = _timeScale;
    }

    public void UpdateCamerasPriority()
    {
        battleVirtalCamera.Priority = 10;
        marketVirtalCamera.Priority = (Status == GameStatus.Market ? 15 : 5);
        heroWinVirtalCamera.Priority = (Status == GameStatus.End && EnemyController.IsDead && !HeroController.IsDead ? 15 : 5);
        enemyWinVirtalCamera.Priority = (Status == GameStatus.End && !EnemyController.IsDead && HeroController.IsDead ? 15 : 5);
    }

    public void UpdateUIVisibility()
    {
        _marketCanvas.SetActive(Status == GameStatus.Market);
        _battleCanvas.SetActive(true);
        _enemyHealthCanvas.SetActive(Status != GameStatus.Market);
        _prepareCanvas.SetActive(Status == GameStatus.Prepare);
        _prepareBattleButton.SetActive(Status == GameStatus.Prepare && Mode == GameMode.Offline);
        _starsCanvas.SetActive(Status == GameStatus.Prepare);
        _endCanvas.SetActive(Mode == GameMode.Offline && Status == GameStatus.End);
        _endTextWin.enabled = Status == GameStatus.End && EnemyController.IsDead && !HeroController.IsDead;
        _endTextLoss.enabled = Status == GameStatus.End && !EnemyController.IsDead && HeroController.IsDead;

        if (Status != GameStatus.Prepare)
            _prepareBattleCountDown.enabled = false;
    }

    public void EndBattle()
    {
        StartCoroutine(EndBattleCoroutine());
    }

    protected IEnumerator EndBattleCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        if (_enemyHealth.Value <= 0 && _heroHealth.Value > 0)
            heroWinVirtalCamera.Priority = 15;
        if (_heroHealth.Value <=0 && _enemyHealth.Value > 0)
            enemyWinVirtalCamera.Priority = 15;
        yield return new WaitForSeconds(1f);
        Time.timeScale = _timeScale * 0.5f;
        yield return new WaitForSeconds(4f);
        SetStatus(GameStatus.End);
    }

    private void CountDownStart()
    {
        _prepareBattleButton.SetActive(false);
        _starsCanvas.SetActive(false);
        StartCoroutine(CountDownCoroutine());
    }

    protected IEnumerator CountDownCoroutine()
    {
        _prepareBattleCountDown.text = "3";
        _prepareBattleCountDown.enabled = true;
        yield return new WaitForSeconds(1f);
        _prepareBattleCountDown.text = "2";
        yield return new WaitForSeconds(1f);
        _prepareBattleCountDown.text = "1";
        yield return new WaitForSeconds(1f);
        _prepareBattleCountDown.enabled = false;
        SetStatus(GameStatus.Main);
    }

    public void SetMode(GameMode mode)
    {
        Mode = mode;
        _market.SetGameMode(Mode); 
    }

    public enum GameStatus
    {
        Market,
        Prepare,
        Main,
        End,
    }
    public enum GameMode
    {
        Offline,
        Online
    }


    [Preserve]
    public void UpdateGameDataOnline(string dataString)
    {
        InitGameData initGameData = JsonUtility.FromJson<InitGameData>(dataString);
        AudioListener.volume = initGameData.Mute ? 0 : 1;
        _timeScale = initGameData.TimeScale;
        HeroNotDead = initGameData.HeroNotDead;
        SetMode(GameMode.Online);

        _market.UpdateGameDataOnline(initGameData);
        HeroController.UpdateGameDataOnline(initGameData, false, initGameData.StartHealth);
        HeroController.UpdateStars(initGameData.AttackStars, initGameData.DefStars, initGameData.SpeedStars);
        EnemyController.UpdateGameDataOnline(initGameData, initGameData.EnemyAuto);

        if(initGameData.Status != 2 || Status != GameStatus.End)
            SetStatus(initGameData.Status);
    }


    [Preserve]
    public void UpdateHeroInput(string dataString)
    {
        InputFlags heroInput = JsonUtility.FromJson<InputFlags>(dataString);
        HeroController.UpdateHeroInputOnline(heroInput);
    }

    [Preserve]
    public void UpdateEnemyInput(string dataString)
    {
        InputFlags enemyInput = JsonUtility.FromJson<InputFlags>(dataString);
        EnemyController.UpdateHeroInputOnline(enemyInput);
    }


    private IEnumerator UpdateHeroNextStep()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateHeroNextStepInterval);

            if (!HeroController.IsDead && !EnemyController.IsDead && Status != GameStatus.Prepare && Status != GameController.GameStatus.End)
            {
                HeroController.UpdateHeroNextStep();
                if(EnemyController.isActiveAndEnabled)
                    EnemyController.UpdateHeroNextStep();
            }
        }
    }
}
