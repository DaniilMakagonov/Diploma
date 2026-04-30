using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class ElementViewComponent : MonoBehaviour
    {
        private Dictionary<Elements, Sprite> _sprites;

        [SerializeField]
        private DeffenseComponent _character;
        [SerializeField]
        private Image _image;
        [SerializeField]
        private List<KeyValue<Elements, Sprite>> _spritesSource;


        private void Start()
        {
            _sprites = new(_spritesSource.Select(p => new KeyValuePair<Elements, Sprite>(p.Key, p.Value)));
            _character.OnElementChanged += ChangeElement;
            ChangeElement(_character.DeffenseElement.Type);
        }

        private void ChangeElement(Elements element)
        {
            _image.sprite = _sprites[element];
        }
    }
}