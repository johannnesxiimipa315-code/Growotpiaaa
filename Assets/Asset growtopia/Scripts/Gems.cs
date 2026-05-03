using UnityEngine;
using TMPro;

public class GemManager : MonoBehaviour
{
    public static GemManager instance;

    public int gems;              
    private float displayGems;     
    private float targetGems;

    public TMP_Text gemText;

    public float speed = 10f;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
      
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