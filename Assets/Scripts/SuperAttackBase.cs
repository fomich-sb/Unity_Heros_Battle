using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class SuperAttackBase : MonoBehaviour
{
    [SerializeField] protected float _lastActivate = -1000;
    [SerializeField] protected Image _icon;
    [SerializeField] protected Image[] _sectors;
    [SerializeField] protected bool _available = true;
    [SerializeField] protected ParticleSystem[] _particleSystems;
    [SerializeField] private GameObject triggerCollaider;
    [SerializeField] private int _rechargeSec = 10;
    [SerializeField] private float _damageDelay = 2;
    private Transform _targetTransform;

    private void Update()
    {
        if (_available) return;

        UpdateRechargeStatus();
    }

    public void Init(Transform targetTransform)
    {
        _targetTransform = targetTransform;
    }

    public virtual bool Activate()
    {
        if (!IsAvailable())
            return false;

        foreach (var particleSystem in _particleSystems)
        {
            particleSystem.Play();
        }
        _lastActivate = Time.time;
        Disable();
        StartCoroutine(TriggerCollaiderActivate());
        return true;
    }

    private void UpdateRechargeStatus()
    {
        float loadedPercent = Mathf.Min(1, (Time.time - _lastActivate) / _rechargeSec);

        if (loadedPercent >= 1)
        {
            if (_icon)
                _icon.color = new Color(1, 1, 1, 1);
            _available = true;
        }

        if (_sectors.Length > 0)
        {
            float loadedValue = loadedPercent * _sectors.Length;
            for (int i = 0; i < _sectors.Length; i++)
            {
                if (i < Mathf.Floor(loadedValue))
                    _sectors[i].color = new Color(1, 1, 1, 1);
                else if (i == Mathf.Floor(loadedValue))
                    _sectors[i].color = new Color(1, 1, 1, 0.3f + 0.7f * (loadedValue - Mathf.Floor(loadedValue)));
                else
                    _sectors[i].color = new Color(1, 1, 1, 0.3f);
            }
        }
    }

    public bool IsAvailable()
    {
        return _available;
    }

    public void Restart()
    {
        _lastActivate = -1000;
        UpdateRechargeStatus();
    }

    protected void Disable()
    {
        if (_icon)
            _icon.color = new Color(1, 1, 1, 0.3f);
        _available = false;
    }

    public void SetRechargeSec(int sec)
    {
        _rechargeSec = sec;
    }

    protected IEnumerator TriggerCollaiderActivate()
    {
        if (_damageDelay >= 0)
        {
            yield return new WaitForSeconds(_damageDelay);
            triggerCollaider.transform.position = _targetTransform.position;
            triggerCollaider.SetActive(true);

            StartCoroutine(TriggerCollaiderDisactivate());
        }
    }

    private IEnumerator TriggerCollaiderDisactivate()
    {
        yield return new WaitForSeconds(0.1f);

        triggerCollaider.SetActive(false);
    }
}
