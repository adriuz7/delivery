using UnityEngine;

public enum ItemWorldState
{
    EnMundo,
    EnInventario,
    Bloqueado
}

public class ItemWorld : MonoBehaviour
{
    public ItemData data;
    public ItemWorldState state = ItemWorldState.EnMundo;

    private Collider col;
    private Rigidbody rb;

    private void Awake()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        if (col == null)
        {
            Debug.LogError($"ItemWorld sin Collider: {name}");
        }
    }

    public void PickUp()
    {
        if (state != ItemWorldState.EnMundo) return;

        state = ItemWorldState.EnInventario;

        if (col != null)
            col.enabled = false;

        if (rb != null)
            rb.isKinematic = true;

        gameObject.SetActive(false);
    }

    public void Drop(Vector3 origin)
    {
        gameObject.SetActive(true);

        Vector3 dropPos = GetGroundPosition(origin);

        transform.position = dropPos;

        state = ItemWorldState.Bloqueado;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (col != null)
            col.enabled = true;

        Invoke(nameof(EnablePickup), 0.5f);
    }

    private Vector3 GetGroundPosition(Vector3 origin)
    {
        RaycastHit hit;
    
        Vector3 rayOrigin = origin + Vector3.up * 1.5f;
    
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 5f))
        {
            float offset = col.bounds.extents.y;
            return hit.point + Vector3.up * offset;
        }
        return origin;
    }


    private void EnablePickup()
    {
        col.enabled = true;
        state = ItemWorldState.EnMundo;
    }
}
