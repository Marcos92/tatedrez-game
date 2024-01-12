using UnityEngine;

public class MoveHighlighter : MonoBehaviour
{
    void Awake()
    {
        GameEvents.PieceHold.AddListener(HighlightValidMoves);
    }

    private void HighlightValidMoves(Piece piece)
    {
        if (GameManager.Instance.CurrentPhase == GamePhase.TICTACTOE)
        {
            return;
        }

        int index = GameBoard.Instance.GetIndexOfTile(piece.Tile);
        var validMoves = piece.GetComponent<IPieceMovementRules>().CheckValidMoves(index);

        var board = GameBoard.Instance.Board;

        for (int i = 0; i < validMoves.Length; i++)
        {
            if (validMoves[i])
            {
                board[i].ShowValidMoveHighlight();
            }
        }
    }

    private void ResetValidMoves()
    {
        var board = GameBoard.Instance.Board;
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i])
            {
                board[i].ShowValidMoveHighlight();
            }
        }
    }
}