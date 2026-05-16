using Assets.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Data
{
    public static class AchievementsStorage
    {
        private static List<Achievement> _achievements;

        public static void Load()
        {
            _achievements = Repository.TryGetData<AchievementData>(out var data)
                ? data.Achievements 
                : new List<Achievement>();
        }

        public static void Save()
        {
            Repository.SetData<AchievementData>(new() { Achievements = _achievements});
        }

        public static void Add(Achievement achievement)
        {
            achievement.CompleteDate = DateTime.UtcNow.ToString("dd.MM.yyyy");
            _achievements.Add(achievement);
            Save();
        }

        public static bool Check(Achievement achievement)
        {
            UnityEngine.Debug.Log("achievs: "+_achievements.Count);
            return _achievements.Contains(achievement);
        }
    }
}