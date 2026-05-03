using UnityEngine;

public class Backsound : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();  
        audioSource.loop = true;
        audioSource.Play();
    }
}
