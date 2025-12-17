using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("Referencias")]
    public Inventory inventory;            
    public Transform slotsParent;           
    public GameObject slotPrefab;           
    
    private List<SlotUI> slotUIs = new List<SlotUI>();

    private void Start()
    {
        if (inventory == null)
        {
            Debug.LogError("InventoryUI: falta referencia al Inventory.");
            return;
        }

        CreateSlots();

        inventory.OnInventoryChanged += RefreshUI;

        RefreshUI();
    }

    private void OnDestroy()
    {
        if (inventory != null)
            inventory.OnInventoryChanged -= RefreshUI;
    }

    private void CreateSlots()
    {
        foreach (Transform child in slotsParent)
            Destroy(child.gameObject);

        slotUIs.Clear();

        int count = inventory.maxSlots;
        for (int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(slotPrefab, slotsParent);
            SlotUI slotUI = go.GetComponent<SlotUI>();
            if (slotUI == null)
            {
                Debug.LogError("Slot prefab no tiene componente SlotUI.");
                continue;
            }

            slotUI.Setup(i, inventory);
            slotUIs.Add(slotUI);
        }
    }

    public void RefreshUI()
    {
        for (int i = 0; i < slotUIs.Count; i++)
        {
            ItemWorld itemWorld = inventory.slots[i].itemWorld;
            slotUIs[i].SetItem(itemWorld != null ? itemWorld.data : null);
            slotUIs[i].ActualizarSeleccion();
        }
    }
}
