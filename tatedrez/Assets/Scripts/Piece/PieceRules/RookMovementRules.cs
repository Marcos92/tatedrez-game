using UnityEngine;

public class RookMovementRules : MonoBehaviour, IPieceMovementRules
{
    public bool[] CheckValidMoves(int index)
    {
        Tile[] board = GameBoard.Instance.Board;
        int tileCount = board.Length;
        bool[] validMoves = new bool[tileCount];

        for (int i = 0; i < tileCount; i++)
        {
            bool validRow = BoardUtils.IsOnSameRow(index, i) && !BoardUtils.IsPieceBlockingRow(index, i);
            bool validColumn = BoardUtils.IsOnSameColumn(index, i) && !BoardUtils.IsPieceBlockingColumn(index, i);
            if (!board[i].HasPiece() && index != i && (validColumn || validRow))
            {
                validMoves[i] = true;
            }
        }
        return validMoves;
    }
}