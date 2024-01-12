using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    private GamePhase currentPhase;
    public GamePhase CurrentPhase { get => currentPhase; }

    public void StartTicTacToe()
    {
        currentPhase = GamePhase.TICTACTOE;
        GameEvents.StartTicTacToe.Invoke();
    }

    public void StartChess()
    {
        currentPhase = GamePhase.CHESS;
        GameEvents.StartChess.Invoke();
    }
}