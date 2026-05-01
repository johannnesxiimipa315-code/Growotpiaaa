using UnityEngine;

public class Cam : MonoBehaviour
{   
    [Range(0,1)]
    public float smoothTime = 0.2f;

    public Transform playerTransform;

    void Start()
    {
        // langsung snap kamera ke posisi player saat game mulai
        transform.position = new Vector3(
            playerTransform.position.x,
            playerTransform.position.y,
            transform.position.z // biar z tetap
        );
    }

    void FixedUpdate() {
        Vector3 pos = transform.position;

        pos.x = Mathf.Lerp(pos.x, playerTransform.position.x, smoothTime);
        pos.y = Mathf.Lerp(pos.y, playerTransform.position.y, smoothTime);

        transform.position = pos;
    }
}
