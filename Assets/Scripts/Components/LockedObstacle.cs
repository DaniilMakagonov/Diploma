using Assets.Scripts.Components;
using UnityEngine;

public class LockedObstacle : MonoBehaviour
{
    [SerializeField] private string _lockKeyName;
    [SerializeField] private float activationDistance = 2f;
    [SerializeField] private Transform player;

    private bool isOpened = false;

    private void Update()
    {
        if (isOpened || player == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= activationDistance)
        {
            PlayerKeyInventory inventory = player.GetComponent<PlayerKeyInventory>();

            if (inventory != null && inventory.HasKey(_lockKeyName))
            {
                OpenObstacle();
            }
        }
    }

    private void OpenObstacle()
    {
        isOpened = true;
        Destroy(gameObject);
    }
}