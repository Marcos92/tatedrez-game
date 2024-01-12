using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip sfxGameMode;
    [SerializeField] private AudioClip sfxEndGame;
    [SerializeField] private AudioClip sfxNoMoves;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        GameEvents.EndGame.AddListener(PlayEndGameAudio);

        GameEvents.StartTicTacToe.AddListener(PlayStartPhaseAudio);
        GameEvents.StartChess.AddListener(PlayStartPhaseAudio);

        GameEvents.NoMoves.AddListener(PlayNoMovesAudio);
    }

    private void PlayStartPhaseAudio()
    {
        audioSource.clip = sfxGameMode;
        audioSource.Play();
    }

    private void PlayEndGameAudio()
    {
        audioSource.clip = sfxEndGame;
        audioSource.Play();
    }

    private void PlayNoMovesAudio()
    {
        audioSource.clip = sfxNoMoves;
        audioSource.Play();
    }
}