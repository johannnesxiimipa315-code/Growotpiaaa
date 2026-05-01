using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public int maxSlots = 5;

    public List<InventorySlot> slots =
        new List<InventorySlot>();

    void Awake()
    {
        instance = this;
    }

    public void AddItem(string itemName)
    {
        // kalau item sudah ada
        foreach (InventorySlot slot in slots)
        {
            if (slot.itemName == itemName)
            {
                slot.amount++;
                return;
            }
        }

        // item baru
        if (slots.Count < maxSlots)
        {
            slots.Add(new InventorySlot(itemName, 1));
        }
    }

    public bool HasItem(string itemName)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.itemName == itemName && slot.amount > 0)
                return true;
        }

        return false;
    }

    public bool RemoveItem(string itemName)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].itemName == itemName)
            {
                slots[i].amount--;

                if (slots[i].amount <= 0)
                {
                    slots.RemoveAt(i); // auto geser kiri
                }

                return true;
            }
        }

        return false;
    }
}