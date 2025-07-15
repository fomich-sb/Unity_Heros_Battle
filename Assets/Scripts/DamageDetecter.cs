using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDetecter : MonoBehaviour
{
    [SerializeField] private Material[] _materialsForDamagePaint;
    [SerializeField] private HeroAudioController _heroAudioController;

    private Health _health;
    private HeroControllerBase _heroController;
    private IHeroAttack _otherHeroAttack;
    private Dictionary<Material, Color> _originalColors = new Dictionary<Material, Color>();

    private void Start()
    {
        _heroController = GetComponent<HeroControllerBase>();
        _health = GetComponent<Health>();
        for (int i = 0; i < _materialsForDamagePaint.Length; i++)
            _originalColors.Add(_materialsForDamagePaint[i], _materialsForDamagePaint[i].color);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.IsChildOf(transform) || !other.GetComponent<Damager>()) 
            return;

        if (_otherHeroAttack == null)
            _otherHeroAttack = other.GetComponentInParent<IHeroAttack>();

        if (_otherHeroAttack == null || _heroController.HasBlock())
            return;

        TryDamage();
    }

    private void TryDamage()
    {
        float damageValue = _otherHeroAttack.GetAttackDamageValue();
        if (damageValue > 0)
        {
            _health.Damage(damageValue);
            PaintOnDamage();
            _heroAudioController?.PlaySound("Damage");
        }
    }


    private void PaintOnDamage()
    {
        for (int i = 0; i < _materialsForDamagePaint.Length; i++)
            _materialsForDamagePaint[i].color = new Color(1, 0.5f, 0.5f);

        StartCoroutine(ResetColors());
    }

    private IEnumerator ResetColors()
    {
        if(_health.Value<=0)
            yield return new WaitForSeconds(2f);
        else
            yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < _materialsForDamagePaint.Length; i++)
            _materialsForDamagePaint[i].color = _originalColors[_materialsForDamagePaint[i]];
    }
}
