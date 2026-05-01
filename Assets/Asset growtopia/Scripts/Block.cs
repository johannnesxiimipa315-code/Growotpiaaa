using UnityEngine;

public class Block : IBlock
{
    public bool Place(
        Sprite sprite,
        Vector2 position,
        string itemName,
        GameObject dropPrefab
    )
    {
        int gridX = Mathf.RoundToInt(position.x);
        int gridY = Mathf.RoundToInt(position.y);

        Vector2 gridPos = new Vector2(gridX, gridY);

        RaycastHit2D hit = Physics2D.Raycast(gridPos, Vector2.zero);

        if (hit.collider == null)
        {
            GameObject tile = new GameObject(itemName);

            SpriteRenderer sr = tile.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;

            tile.transform.position = gridPos;

            tile.AddComponent<BoxCollider2D>();
            tile.tag = "Ground";

            BlockHealth bh = tile.AddComponent<BlockHealth>();
            bh.itemName = itemName;
            bh.dropPrefab = dropPrefab;

            return true;
        }

        return false;
    }

    public void Break(Vector2 position)
    {
        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.y);

        RaycastHit2D hit =
            Physics2D.Raycast(new Vector2(x, y), Vector2.zero);

        if (hit.collider != null)
        {
            BlockHealth bh =
                hit.collider.GetComponent<BlockHealth>();

            if (bh != null)
                bh.TakeDamage(1);
        }
    }
}