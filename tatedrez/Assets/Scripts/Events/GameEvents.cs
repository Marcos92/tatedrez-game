using UnityEngine.Events;

public static class GameEvents
{
    public static UnityEvent EndGame = new();
    public static UnityEvent TieGame = new();

    public static UnityEvent StartTurn = new();
    public static UnityEvent NextTurn = new();

    public static UnityEvent StartTicTacToe = new();
    public static UnityEvent StartChess = new();

    public static UnityEvent<Piece> PieceHold = new();
    public static UnityEvent PieceRelease = new();
    public static UnityEvent PiecePlayed = new();

    public static UnityEvent NoMoves = new();
}