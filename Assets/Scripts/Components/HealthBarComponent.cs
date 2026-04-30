using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Components
{
    [RequireComponent(typeof(DeffenseComponent))]
    public class HealthBarComponent : MonoBehaviour
    {
        [SerializeField] private DeffenseComponent _deffence;
        [SerializeField] private RectTransform _greenTransform;
        [SerializeField] private TextMeshProUGUI _textValue;

        private float _defaultGreenSize;

        private void Start()
        {
            _defaultGreenSize = _greenTransform.rect.width;

            _deffence.SubscribeOnHealthChange(OnChangeValue);
            OnChangeValue(_deffence.Health);
        }

        private void OnChangeValue(int value)
        {
            var newWidth = _defaultGreenSize * value / _deffence.MaxHealth;
            _greenTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);

            _textValue.text = value.ToString();
        }
    }
}