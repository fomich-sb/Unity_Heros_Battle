using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class HeroControllerBase : MonoBehaviour
{
    [SerializeField] protected Animator _animator;
    [SerializeField] protected Animator _animatorRoot;
    [SerializeField] protected AnimationListener _animationListener;

    [SerializeField] protected bool _enabledAutoActions = false;

    [SerializeField] protected float _attackDamage = 0.05f;
    [SerializeField] protected float _superAttack1Damage = 0.1f;
    [SerializeField] protected float _superAttack2Damage = 0.1f;

    [SerializeField] protected SuperAttackBase _heroSuperAttack1;
    [SerializeField] protected SuperAttackBase _heroSuperAttack2;
    [SerializeField] protected HeroAudioController _heroAudioController;


    [SerializeField] public int _attackStarsCnt = 5;
    [SerializeField] public int _defStarsCnt = 4;
    [SerializeField] public int _speedStarsCnt = 3;
    [SerializeField] protected Transform _attackStarsPanel;
    [SerializeField] protected Transform _defStarsPanel;
    [SerializeField] protected Transform _speedStarsPanel;
    [SerializeField] protected Color startActive = new Color(237, 214, 152);
    [SerializeField] protected Color startDisactive = new Color(60, 60, 60);


    protected InputFlags InputFlags { get; set; }
    protected InputFlags _inputFlagsPrev = new InputFlags();

    protected int _currentAnimationId;
    protected int _nextAnimationId;
    protected bool _damageTakedOnCurrentStep = false;

    protected IInputSystem _inputSystem;
    protected IInputSystem _inputSystemAuto;
    protected InputHeroOnline _inputSystemOnline;
    protected int _idleId = 1;
    protected Health _health;
    protected HeroControllerBase _enemyController;
    protected Health _enemyHealth;
    protected float _startHealth = 1;
    protected Vector3 _startPosition;
    [HideInInspector] public bool IsDead = false;

    protected Dictionary<int, InputFlags> _animationInputs;
    private System.Random rand;

    [Inject] protected GameController _gameController;

    public void Init(HeroControllerBase enemyController)
    {
        _enemyController = enemyController;
        _enemyHealth = _enemyController.GetComponent<Health>();
        _heroAudioController = GetComponent<HeroAudioController>();
        _heroSuperAttack1.Init(_enemyController.transform);
        _heroSuperAttack2.Init(_enemyController.transform);
        _enemyHealth.OnDead += Fatality;
        _inputSystemAuto = new InputHeroAuto(_gameController, enemyController);
        _inputSystemOnline = new InputHeroOnline();

        _inputSystem = GetComponent<IInputSystem>();
        _health = GetComponent<Health>();
        _health.OnDead += OnDead;
        _animationListener.NewAnimationStart += OnNewAnimationStart;

        _startPosition = transform.position;
        rand = new System.Random();

        Restart();

    }

    public void UpdateHeroNextStep()
    {
        UpdateInputFlags();
        UpdateNextAnimationStep();
    }

    protected void UpdateInputFlags()
    {
        if (_enabledAutoActions && _inputSystemAuto != null && _gameController.Status == GameController.GameStatus.Main)
            InputFlags = _inputSystem.GetInputValues() | _inputSystemOnline.GetInputValues() | _inputSystemAuto.GetInputValues();
        else
            InputFlags = _inputSystem.GetInputValues() | _inputSystemOnline.GetInputValues();
    }

    protected void UpdateNextAnimationStep()
    {
        if (_nextAnimationId > 0 && _inputFlagsPrev.Equals(InputFlags)) //Ввод не поменялся
            return;

        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        float progressCurrentAnimation = stateInfo.normalizedTime % 1.0f;
        if (_nextAnimationId > 0 && _nextAnimationId != _idleId && (progressCurrentAnimation < 0.5f || progressCurrentAnimation >= 0.9f))
            return;

        _inputFlagsPrev = InputFlags;

        List<int> availableActionsAttack = new List<int>();
        List<int> availableActionsBlock = new List<int>();
        List<int> availableActionsSuperAttack1 = new List<int>();
        List<int> availableActionsSuperAttack2 = new List<int>();
        List<int> availableActionsOther = new List<int>();
        int maxScoreAttack = 0;
        int maxScoreBlock = 0;
        int maxScoreOther = 0;

        foreach (var animationInput in _animationInputs)
        {
            if(animationInput.Value.CorrespondsToInput(InputFlags))
            {
                int animationScore = animationInput.Value.GetScore();
                if (animationInput.Value.Attack)
                {
                    if (animationScore > maxScoreAttack)
                    {
                        availableActionsAttack = new List<int>();
                        maxScoreAttack = animationScore;
                    }
                    if (animationScore == maxScoreAttack)
                        availableActionsAttack.Add(animationInput.Key);
                }

                if (animationInput.Value.Block)
                {
                    if (animationScore > maxScoreBlock)
                    {
                        availableActionsBlock = new List<int>();
                        maxScoreBlock = animationScore;
                    }
                    if (animationScore == maxScoreBlock)
                        availableActionsBlock.Add(animationInput.Key);
                }
                if (animationInput.Value.SuperAttack1 && _heroSuperAttack1.IsAvailable())
                {
                    availableActionsSuperAttack1.Add(animationInput.Key);
                }
                if (animationInput.Value.SuperAttack2 && _heroSuperAttack2.IsAvailable())
                    availableActionsSuperAttack2.Add(animationInput.Key);

                if (!InputFlags.Attack && !InputFlags.Block && !animationInput.Value.SuperAttack1 && !animationInput.Value.SuperAttack2)
                {
                    if (animationScore > maxScoreOther)
                    {
                        availableActionsOther = new List<int>();
                        maxScoreOther = animationScore;
                    }
                    if (animationScore == maxScoreOther)
                        availableActionsOther.Add(animationInput.Key);
                }
            }
        }
        List<int> availableActions = availableActionsAttack;
        availableActions.AddRange(availableActionsBlock);
        availableActions.AddRange(availableActionsSuperAttack1);
        availableActions.AddRange(availableActionsSuperAttack2);

        if (availableActions.Count == 0)
            availableActions = availableActionsOther;

        int actionAnimationId = availableActions[rand.Next(availableActions.Count)];
        _nextAnimationId = actionAnimationId;
        _animator.SetInteger("id", actionAnimationId);
    }

    void OnNewAnimationStart(int id)
    {
        _currentAnimationId = id;
        _damageTakedOnCurrentStep = false;
        _animatorRoot.SetInteger("id", _currentAnimationId);

        _nextAnimationId = 0;

        if (_animationInputs[_currentAnimationId].SuperAttack1)
            _heroSuperAttack1.Activate();
        if (_animationInputs[_currentAnimationId].SuperAttack2)
            _heroSuperAttack2.Activate();
    }

    public void Restart()
    {
        _health.SetValue(_startHealth);
        IsDead = false;
        _currentAnimationId = _idleId;
        _animator.SetInteger("id", _idleId);
        _animatorRoot.SetInteger("id", _idleId);
        _nextAnimationId = 0;
        _animator.CrossFade("Idle", 0.2f);
        _heroSuperAttack1.Restart();
        _heroSuperAttack2.Restart();
        transform.position = _startPosition;
        DisplayStars();
    }

    protected void OnDead()
    {
        IsDead = true;
        _animator.CrossFade("Death", 0.2f);
        _animator.SetInteger("id", 0);
        _animatorRoot.SetInteger("id", 0);
        _nextAnimationId = 0;
    }

    protected void Fatality()
    {
        transform.DOMove(_enemyController.transform.position + Vector3.Normalize(transform.position - _enemyController.transform.position), 2f).SetEase(Ease.Linear);
        _animator.CrossFade("Fatality", 0.2f);
        _animator.SetInteger("id", 0);
        _animatorRoot.SetInteger("id", 0);
        _nextAnimationId = _idleId;

    }

    public bool HasBlock()
    {
        if (_currentAnimationId > 0 && _animationInputs.ContainsKey(_currentAnimationId))
            return _animationInputs[_currentAnimationId].Block;
        return false;
    }

    public bool HasAttack()
    {
        if(_currentAnimationId>0 && _animationInputs.ContainsKey(_currentAnimationId))
            return _animationInputs[_currentAnimationId].Attack;
        return false;
    }


    public void UpdateGameDataOnline(InitGameData initGameData, bool enabledAutoActions, float startHealth = 1)
    {
        _attackDamage = initGameData.AttackDamage;
        _superAttack1Damage = initGameData.SuperAttack1Damage;
        _superAttack2Damage = initGameData.SuperAttack2Damage;
        _heroSuperAttack1.SetRechargeSec(initGameData.SuperAttack1Delay);
        _heroSuperAttack2.SetRechargeSec(initGameData.SuperAttack2Delay);
        _enabledAutoActions = enabledAutoActions;
        if(_startHealth != startHealth && (_gameController.Status == GameController.GameStatus.Market || _gameController.Status == GameController.GameStatus.Prepare))
            _health.SetValue(_startHealth);

        _startHealth = startHealth;
    }
    public void UpdateStars(int attack, int def, int speed)
    {
        _attackStarsCnt = attack;
        _defStarsCnt = def;
        _speedStarsCnt = speed;
        DisplayStars();
    }

    public void UpdateHeroInputOnline(InputFlags heroInputData)
    {
        _inputSystemOnline.UpdateHeroInputOnline(heroInputData);
    }

    private void DisplayStars()
    {
        for (int i = 0; i < 5; i++) {
            if (_attackStarsPanel.childCount > i)
            {
                var star = _attackStarsPanel.GetChild(i);
                if (star.TryGetComponent(out Image image))
                    image.color = i < _attackStarsCnt ? startActive : startDisactive;
            }
            if (_defStarsPanel.childCount > i)
            {
                var star = _defStarsPanel.GetChild(i);
                if (star.TryGetComponent(out Image image))
                    image.color = i < _defStarsCnt ? startActive : startDisactive;
            }
            if (_speedStarsPanel.childCount > i)
            {
                var star = _speedStarsPanel.GetChild(i);
                if (star.TryGetComponent(out Image image))
                    image.color = i < _speedStarsCnt ? startActive : startDisactive;
            }
        }
    }

    public void SetStarsCnt(int attackStarsCnt, int defStarsCnt, int speedStarsCnt)
    {
        _attackStarsCnt = attackStarsCnt;
        _defStarsCnt = defStarsCnt;
        _speedStarsCnt = speedStarsCnt;
    }
}
