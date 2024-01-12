using System.Linq;

public class ChessMoveValidation : IMoveValidation
{
    public bool CanPieceMove(Piece piece)
    {
        int index = GameBoard.Instance.GetIndexOfTile(piece.Tile);
        var validMoves = piece.GetComponent<IPieceMovementRules>().CheckValidMoves(index);
        return !validMoves.All(value => !value); //Returns true if there is at least one valid move (not all elements are false)
    }

    public bool CanPlayerMove(PlayerColor playerColor)
    {
        var board = GameBoard.Instance.Board;
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i].HasPieceOfColor(playerColor) && CanPieceMove(board[i].Piece))
            {
                return true;
            }
        }
        return false;
    }
}