public class TictactoeMoveValidation : IMoveValidation
{
    public bool CanPieceMove(Piece piece)
    {
        return piece.Tile == null;
    }

    public bool CanPlayerMove(PlayerColor playerColor)
    {
        return true;
    }
}