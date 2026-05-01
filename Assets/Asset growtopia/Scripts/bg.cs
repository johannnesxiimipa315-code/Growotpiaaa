using UnityEngine;

public class bg : MonoBehaviour
{
    public Transform cameraTarget;
    public Vector3 offset;

    void Update()
    {
        transform.position = cameraTarget.position + offset;
    }
}