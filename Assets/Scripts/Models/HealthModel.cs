using System;

namespace Assets.Scripts.Models
{
    public sealed class HealthModel
    {
        public event Action<int> OnHealthChange;
        public event Action OnDeath;

        public int MaxHealth { get; private set; }
        public int Health { get; private set; }

        public HealthModel(int maxHealth)
        {
            MaxHealth = maxHealth;
            Health = maxHealth;
        }

        public void LoseHealth(int value)
        {
            Health = Math.Max(Health - value, 0);
            OnHealthChange?.Invoke(Health);

            if (Health == 0)
            {
                OnDeath?.Invoke();
            }
        }

        public void GetHealth(int value)
        {
            Health = Math.Min(Health + value, MaxHealth);
            OnHealthChange?.Invoke(Health);
        }
    }
}