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
    public ItemWorld ObtenerItemEntregable()
    {
        foreach (var slot in slots)
        {
            if (slot.itemWorld != null)
                return slot.itemWorld;
        }
        return null;
    }

    public void RemoverItem(ItemWorld item)
    {
        foreach (var slot in slots)
        {
            if (slot.itemWorld == item)
            {
                slot.itemWorld = null;
                OnInventoryChanged?.Invoke();
                return;
            }
        }
    }

    public int SlotSeleccionado { get; private set; } = -1;

    public void SeleccionarSlot(int index)
    {
        if (index < 0 || index >= slots.Count) return;
        if (slots[index].itemWorld == null) return;
    
        SlotSeleccionado = index;
        OnInventoryChanged?.Invoke();
    }
    
    public ItemWorld ObtenerItemSeleccionado()
    {
        if (SlotSeleccionado < 0 || SlotSeleccionado >= slots.Count)
            return null;
    
        return slots[SlotSeleccionado].itemWorld;
    }
    
    public void LimpiarSeleccion()
    {
        SlotSeleccionado = -1;
    }

}
