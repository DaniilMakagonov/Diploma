using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Element", menuName = "Game/Element")]
    public sealed class Element : ScriptableObject
    {
        [SerializeField]
        private Elements _type;

        [SerializeField]
        private Color _color;

        public Elements Type { 
            get => _type;
            private set => _type = value;
        }

        public Color Color
        {
            get => _color;
            private set => _color = value;
        }

        public static bool operator >(Element left, Element right)
        {
            return left.Type == Elements.Water && right.Type == Elements.Fire
                || left.Type == Elements.Fire && right.Type == Elements.Air
                || left.Type == Elements.Air && right.Type == Elements.Earth
                || left.Type == Elements.Earth && right.Type == Elements.Water;
        }

        public static bool operator <(Element left, Element right)
        {
            return right.Type == Elements.Water && left.Type == Elements.Fire
                || right.Type == Elements.Fire && left.Type == Elements.Air
                || right.Type == Elements.Air && left.Type == Elements.Earth
                || right.Type == Elements.Earth && left.Type == Elements.Water;
        }
    }
}