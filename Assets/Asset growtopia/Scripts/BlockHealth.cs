using UnityEngine;

public class BlockHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public GameObject dropPrefab;
    public string itemName;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Break();
        }
    }

    void Break()
    {
        if (dropPrefab != null)
        {
            GameObject drop =
                Instantiate(dropPrefab, transform.position, Quaternion.identity);

            ItemDrop item = drop.GetComponent<ItemDrop>();

            if (item != null)
                item.itemName = itemName;
        }

        Destroy(gameObject);
    }
}