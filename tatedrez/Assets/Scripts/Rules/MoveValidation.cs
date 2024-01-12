using System.Linq;

public class MoveValidation
{
    public static bool CanPieceMove(Piece piece)
    {
        if(GameManager.Instance.CurrentPhase == GamePhase.TICTACTOE)
        {
            return piece.Tile == null;
        }

        int index = GameBoard.Instance.GetIndexOfTile(piece.Tile);
        var validMoves = piece.GetComponent<IPieceMovementRules>().CheckValidMoves(index);
        return !validMoves.All(value => !value); //Returns true if there is at least one valid move (not all elements are false)
    }

    public static bool CanPlayerMove(PlayerColor playerColor)
    {
        if(GameManager.Instance.CurrentPhase == GamePhase.TICTACTOE)
        {
            return true;
        }

        var board = GameBoard.Instance.Board;

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i].Piece && board[i].Piece.Color == playerColor && CanPieceMove(board[i].Piece))
            {
                return true;
            }
        }

        return false;
    }
}
