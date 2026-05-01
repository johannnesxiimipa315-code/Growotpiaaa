using UnityEngine;
using TMPro;

public class GemManager : MonoBehaviour
{
    public static GemManager instance;

    public int gems;              // nilai yang tampil
    private float displayGems;    // untuk animasi
    private float targetGems;

    public TMP_Text gemText;

    public float speed = 10f;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        // 🔥 animasi smooth (float ke float, aman)
        displayGems = Mathf.MoveTowards(displayGems, targetGems, speed * Time.deltaTime);

        gems = Mathf.RoundToInt(displayGems);

        if (gemText != null)
            gemText.text = gems.ToString();
    }

    public void AddGems(int amount)
    {
        targetGems += amount;
    }
}