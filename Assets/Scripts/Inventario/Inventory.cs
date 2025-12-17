using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int maxSlots = 5;
    public List<InventorySlot> slots = new List<InventorySlot>();

    public Transform dropPoint;

    public event Action OnInventoryChanged;

    private void Awake()
    {
        for (int i = 0; i < maxSlots; i++)
            slots.Add(new InventorySlot());
    }

    public bool AddItem(ItemWorld itemWorld)
    {
        foreach (var slot in slots)
        {
            if (slot.IsEmpty())
            {
                slot.itemWorld = itemWorld;
                OnInventoryChanged?.Invoke();
                return true;
            }
        }
        return false;
    }

    public void DropItem(int index)
    {
        if (index < 0 || index >= slots.Count) return;
    
        ItemWorld itemWorld = slots[index].itemWorld;
        if (itemWorld == null) return;
    
        itemWorld.Drop(dropPoint.position);
    
        slots[index].itemWorld = null;
        OnInventoryChanged?.Invoke();
    }

}
