using UnityEngine;

public class KnightMovementRules : MonoBehaviour, IPieceMovementRules
{
    public bool[] CheckValidMoves(int index)
    {
        Tile[] board = GameBoard.Instance.Board;
        int tileCount = board.Length;
        bool[] validMoves = new bool[tileCount];

        for (int i = 0; i < tileCount; i++)
        {
            if (!board[i].HasPiece() && !BoardUtils.IsOnSameColumn(index, i) && !BoardUtils.IsOnSameRow(index, i) && !BoardUtils.IsOnSameDiagonal(index, i) && IsInKnightRange(index, i))
            {
                validMoves[i] = true;
            }
        }
        return validMoves;
    }

    private bool IsInKnightRange(int a, int b)
    {
        return Mathf.Abs(BoardUtils.GetRow(a) - BoardUtils.GetRow(b)) <= 2 && Mathf.Abs(BoardUtils.GetColumn(a) - BoardUtils.GetColumn(b)) <= 2;
    }
}