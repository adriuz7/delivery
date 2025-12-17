using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItemPickup : MonoBehaviour
{
    public Inventory inventory;
    public Transform dropPoint;
    public InputActionReference interactAction;

    private ItemWorld currentItem;

    private void OnEnable()
    {
        interactAction.action.Enable();
    }

    private void OnDisable()
    {
        interactAction.action.Disable();
    }

    private void Update()
    {
        if (currentItem == null) return;

        if (interactAction.action.WasPressedThisFrame())
        {
            if (currentItem.state != ItemWorldState.EnMundo)
                return;

            bool added = inventory.AddItem(currentItem);

            if (added)
            {
                ItemPickupUI.Instance.Hide();
                currentItem.PickUp();
                currentItem = null;
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ItemWorld item))
        {
            currentItem = item;
            ItemPickupUI.Instance.Show(item.data);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ItemWorld item))
        {
            if (currentItem == item)
            {
                currentItem = null;
                ItemPickupUI.Instance.Hide();
            }
        }
    }

}
