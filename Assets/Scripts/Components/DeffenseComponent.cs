using Assets.Scripts.Models;
using Assets.Scripts.ScriptableObjects;
using System;
using UnityEngine;

public class DeffenseComponent : MonoBehaviour
{
    [SerializeField]
    private int _maxHealth;

    [SerializeField]
    private Element _deffenseElement;

    private HealthModel _health;

    public Element DeffenseElement
    {
        get => _deffenseElement;
        set => _deffenseElement = value;
    }

    private void Start()
    {
        _health = new(_maxHealth);

    }

    public void GetDamage(Attack attack)
    {
        if (attack.Element > DeffenseElement)
        {
            _health.LoseHealth(attack.Damage * 2);
            return;
        }

        if (attack.Element < DeffenseElement)
        {
            _health.LoseHealth(attack.Damage / 2);
            return;
        }

        _health.LoseHealth(attack.Damage);
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
