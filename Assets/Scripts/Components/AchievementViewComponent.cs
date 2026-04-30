using Assets.Scripts.ScriptableObjects;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class AchievementViewComponent : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _description;
        [SerializeField]
        private TextMeshProUGUI _date;
        [SerializeField]
        private Image _image;

        public void InitiateView(Achievement achievement)
        {
            _description.text = achievement.Description;
            _date.text = achievement.CompleteDate.Date.ToString("dd.MM.yyyy");
            _image.sprite = achievement.Sprite;
        }
    }
}