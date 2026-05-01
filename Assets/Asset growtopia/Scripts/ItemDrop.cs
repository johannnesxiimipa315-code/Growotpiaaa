using UnityEngine;
using System.Collections;

public class ItemDrop : MonoBehaviour
{
    public string itemName = "Stone";
    private bool pickedUp = false;

    public float collectRange = 1.5f;

    void Start()
    {
        transform.localScale = Vector3.zero;
        StartCoroutine(Appear());
    }

    IEnumerator Appear()
    {
        float duration = 0.2f;
        float time = 0f;

        Vector3 targetScale = new Vector3(0.5f, 0.5f, 1f);

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            transform.localScale =
                Vector3.Lerp(Vector3.zero, targetScale, t);

            yield return null;
        }

        transform.localScale = targetScale;
    }

    void Update()
    {
        if (pickedUp) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.transform.position);

        if (dist <= collectRange)
        {
            pickedUp = true;
            Inventory.instance.AddItem(itemName);
            Destroy(gameObject);
        }
    }
}