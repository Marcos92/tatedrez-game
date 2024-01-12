using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PieceAudio : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayPieceSelectedAudio()
    {
        audioSource.Play();
    }
}