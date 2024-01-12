using UnityEngine;

public class BoardUtils
{
    public static int GetRow(int i)
    {
        return i / GameBoard.Instance.Size;
    }

    public static int GetColumn(int i)
    {
        return i % GameBoard.Instance.Size;
    }

    public static bool IsOnSameRow(int a, int b)
    {
        return GetRow(a) == GetRow(b);
    }

    public static bool IsOnSameColumn(int a, int b)
    {
        return GetColumn(a) == GetColumn(b);
    }

    public static bool IsOnSameDiagonal(int a, int b)
    {
        return Mathf.Abs(GetRow(a) - GetRow(b)) == Mathf.Abs(GetColumn(a) - GetColumn(b));
    }

    public static bool IsPieceBlockingRow(int a, int b)
    {
        int direction = 1;
        int first = Mathf.Min(a, b) + direction;
        int last = Mathf.Max(a, b);

        for (int i = first; i < last; i += direction)
        {
            if (GameBoard.Instance.Board[i].HasPiece())
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsPieceBlockingColumn(int a, int b)
    {
        int direction = GameBoard.Instance.Size;
        int first = Mathf.Min(a, b) + direction;
        int last = Mathf.Max(a, b);

        for (int i = first; i < last; i += direction)
        {
            if (GameBoard.Instance.Board[i].HasPiece())
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsPieceBlockingDiagonal(int a, int b)
    {
        int top = Mathf.Min(a, b);
        int bottom = Mathf.Max(a, b);

        bool topLeftDiagonal = GetColumn(top) < GetColumn(bottom); //Otherwise we have a top-right diagonal
        int direction = GameBoard.Instance.Size + (topLeftDiagonal ? 1 : -1);
        top += direction;

        for (int i = top; i < bottom; i += direction)
        {
            if (GameBoard.Instance.Board[i].HasPiece())
            {
                return true;
            }
        }

        return false;
    }
}