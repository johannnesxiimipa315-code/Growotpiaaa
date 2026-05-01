using UnityEngine;

public class Cam : MonoBehaviour
{   
    public float moveSpeed;
    [Range(0,1)]
    public float smoothTime;

    public Transform playerTransform;
    void FixedUpdate() {
        Vector3 pos = GetComponent<Transform>().position;

        pos.x = Mathf.Lerp(pos.x, playerTransform.position.x,smoothTime);
        pos.y = Mathf.Lerp(pos.y, playerTransform.position.y,smoothTime);

        GetComponent<Transform>().position = pos;



    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
