using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private PlayerColor currentPlayerColor;
    public PlayerColor CurrentPlayerColor { get => currentPlayerColor; }
    public PlayerColor OtherPlayerColor { get => currentPlayerColor == PlayerColor.WHITE ? PlayerColor.BLACK : PlayerColor.WHITE; }

    private int turnCount;
    public int TurnCount { get => turnCount; }

    public void ChooseStartingPlayer()
    {
        currentPlayerColor = Random.Range(0, 2) > 0 ? PlayerColor.WHITE : PlayerColor.BLACK;
    }

    public void StartTurn()
    {
        GameEvents.StartTurn.Invoke();
    }

    public void ChangeTurn()
    {
        currentPlayerColor = OtherPlayerColor;
        StartTurn();
    }

    public void EndTurn()
    {
        GameEvents.EndTurn.Invoke();
        turnCount++;
    }
}