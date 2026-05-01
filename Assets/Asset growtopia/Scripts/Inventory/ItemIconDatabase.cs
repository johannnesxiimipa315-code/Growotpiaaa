using UnityEngine;

[System.Serializable]
public class ItemIconData
{
    public string itemName;
    public Sprite icon;
}

public class ItemIconDatabase : MonoBehaviour
{
    public static ItemIconDatabase instance;

    public ItemIconData[] items;

    void Awake()
    {
        instance = this;
    }

    public Sprite GetIcon(string name)
    {
        foreach (var item in items)
        {
            if (item.itemName == name)
                return item.icon;
        }

        return null;
    }
}