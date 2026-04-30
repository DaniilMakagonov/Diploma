using Assets.Scripts.Components;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [SerializeField]
    private string _name;
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerKeyInventoryComponent inventory = other.GetComponent<PlayerKeyInventoryComponent>();

        if (inventory != null)
        {
            inventory.AddKey(_name);
            Destroy(gameObject);
        }
    }
}