using UnityEngine;

public class BishopMovementRules : MonoBehaviour, IPieceMovementRules
{
    public bool[] CheckValidMoves(int index)
    {
        Tile[] board = GameBoard.Instance.Board;
        int tileCount = board.Length;
        bool[] validMoves = new bool[tileCount];

        for (int i = 0; i < tileCount; i++)
        {
            bool validDiagonal = BoardUtils.IsOnSameDiagonal(index, i) && !BoardUtils.IsPieceBlockingDiagonal(index, i);
            if (!board[i].HasPiece() && validDiagonal)
            {
                validMoves[i] = true;
            }
        }
        return validMoves;
    }
}