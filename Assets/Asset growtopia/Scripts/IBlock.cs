using UnityEngine;

public interface IBlock
{
    bool Place(Sprite sprite, Vector2 pos, string itemName, GameObject dropPrefab);
    void Break(Vector2 pos);
}