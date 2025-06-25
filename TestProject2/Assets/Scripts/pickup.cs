using UnityEngine;

public class pickup : MonoBehaviour
{
    [SerializeField] gunStats gun;

    private void OnTriggerEnter(Collider other)
    {
        IPickup pickupItem = other.GetComponent<IPickup>();

        if (pickupItem != null)
        {
            pickupItem.getGunStats(gun);
            Destroy(gameObject);
            Debug.Log("Kill me.");
        }
    }
}
