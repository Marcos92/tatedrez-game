using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TileAudio : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayTileDropSound()
    {
        audioSource.Play();
    }
}