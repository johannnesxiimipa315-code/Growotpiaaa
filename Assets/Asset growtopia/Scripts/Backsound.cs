using UnityEngine;

public class Backsound : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // ambil AudioSource dari GameObject ini
        audioSource.loop = true;
        audioSource.Play();
    }
}
