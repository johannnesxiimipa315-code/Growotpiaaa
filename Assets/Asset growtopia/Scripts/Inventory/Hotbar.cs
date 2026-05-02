using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotbarUI : MonoBehaviour
{
    public Transform[] slotObjects;
    public int selectedSlot = 0;

    void Update()
    {
        RefreshUI();
        InputSlot();
    }

    void InputSlot()
    {   
        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedSlot = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectedSlot = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) selectedSlot = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) selectedSlot = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) selectedSlot = 4;
    }

    void RefreshUI()
    {
        for (int i = 0; i < slotObjects.Length; i++)
        {
            Transform slot = slotObjects[i];

            Image bg = slot.GetComponent<Image>();
            Image icon = slot.Find("Icon").GetComponent<Image>();
            TMP_Text amount = slot.Find("Amount").GetComponent<TMP_Text>();

            if (i < Inventory.instance.slots.Count)
            {
                var data = Inventory.instance.slots[i];

                icon.enabled = true;
                amount.text = data.amount.ToString();

                icon.sprite =
                    ItemIconDatabase.instance.GetIcon(data.itemName);
            }
            else
            {
                icon.enabled = false;
                amount.text = "";
            }

            if (i == selectedSlot)
                bg.color = Color.yellow;
            else
                bg.color = Color.white;
        }
    }

    public string GetSelectedItem()
    {
        if (selectedSlot < Inventory.instance.slots.Count)
        {
            return Inventory.instance.slots[selectedSlot].itemName;
        }

        return "";
    }
}