using Assets.Scripts.Data;
using Assets.Scripts.ScriptableObjects;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class AchievementsListComponent : MonoBehaviour
    {
        [SerializeField]
        private GameObject _previousScreen;
        [SerializeField]
        private GameObject _content;
        [SerializeField]
        private GameObject _emptyText;
        [SerializeField]
        private GameObject _achievementTemplate;

        private AchievementData _achievements;

        private void Start()
        {
            if (!Repository.TryGetData(out _achievements))
            {
                return;
            }

            if (_achievements.Achievements.Count > 0)
            {
                _emptyText.SetActive(false);

                foreach (var achieve in _achievements.Achievements)
                {
                    var view = Instantiate(_achievementTemplate, _content.transform);
                    view.GetComponent<AchievementViewComponent>().InitiateView(achieve);
                }
            }
        }

        public void Back()
        {
            _previousScreen.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}