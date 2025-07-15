using DG.Tweening;
using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject _bar;
    [HideInInspector] public float Value = 1;

    [HideInInspector] public event Action OnDead;

    void Start()
    {
        Display();
    }

    public void SetValue(float value)
    {
        Value = value;
        Display();
    }

    public void Damage(float damageValue)
    {
        Value -= Mathf.Min(Value, damageValue);
        Display();
        if(Value<=0)
            OnDead?.Invoke();
    }

    void Display()
    {
        if (_bar != null)
            _bar.transform.DOScaleX(Value, 1);
    }
}
