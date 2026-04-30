using Assets.Scripts.Models;
using Assets.Scripts.ScriptableObjects;
using System;
using UnityEngine;

public class DeffenseComponent : MonoBehaviour
{
    public event Action<Elements> OnElementChanged;
    [field: SerializeField]
    public int MaxHealth {  get; private set; }

    public int Health
    {
        get
        {
            return _health.Health;
        }
    }

    [SerializeField]
    private Element _deffenseElement;

    private HealthModel _health;

    public Element DeffenseElement
    {
        get => _deffenseElement;
        set
        {
            _deffenseElement = value;
            OnElementChanged?.Invoke(_deffenseElement.Type);
        }
    }

    private void Awake()
    {
        _health = new(MaxHealth);

    }

    public void GetDamage(Attack attack)
    {
        if (attack.Element > DeffenseElement)
        {
            _health.LoseHealth(attack.Damage * 2);
            Debug.Log($"get higher damage {attack.Damage * 2}");
            return;
        }

        if (attack.Element < DeffenseElement)
        {
            _health.LoseHealth(attack.Damage / 2);
            Debug.Log($"get lower damage {attack.Damage / 2}");
            return;
        }

        _health.LoseHealth(attack.Damage);
        Debug.Log($"get same damage {attack.Damage}");
    }

    public void GetHealth(int value)
    {
        _health.GetHealth(value);
    }

    public void SubscribeOnHealthChange(Action<int> action)
    {
        _health.OnHealthChange += action;
    }

    public void SubscribeOnDeath(Action action)
    {
        _health.OnDeath += action;
    }

    public void UnSubscribeOnHealthChange(Action<int> action)
    {
        _health.OnHealthChange -= action;
    }

    public void UnSubscribeOnDeath(Action action)
    {
        _health.OnDeath -= action;
    }
}
