using UnityEngine;

public class MatchValidation
{
    public static bool FindMatch(PlayerColor color)
    {
        return CheckRows(color) || CheckColumns(color) || CheckDiagonals(color);
    }

    private static bool CheckColumns(PlayerColor color)
    {
        Tile[] board = GameBoard.Instance.Board;
        int boardSize = GameBoard.Instance.Size;

        for (int i = 0; i < boardSize; i++)
        {
            if (board[i].HasPieceOfColor(color))
            {
                for (int j = 1; j < boardSize; j++)
                {
                    if (!board[i + j * boardSize].HasPieceOfColor(color))
                    {
                        break;
                    }

                    if (j == boardSize - 1)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private static bool CheckRows(PlayerColor color)
    {
        Tile[] board = GameBoard.Instance.Board;
        int boardSize = GameBoard.Instance.Size;

        for (int i = 0; i < boardSize; i++)
        {
            if (board[i * boardSize].HasPieceOfColor(color))
            {
                for (int j = 1; j < boardSize; j++)
                {
                    if (!board[i * boardSize + j].HasPieceOfColor(color))
                    {
                        break;
                    }

                    if (j == boardSize - 1)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private static bool CheckDiagonals(PlayerColor color)
    {
        Tile[] board = GameBoard.Instance.Board;
        int boardSize = GameBoard.Instance.Size;

        if (board[0].HasPieceOfColor(color))
        {
            for (int i = boardSize + 1; i < Mathf.Pow(boardSize, 2); i += boardSize + 1)
            {
                if (!board[i].HasPieceOfColor(color))
                {
                    break;
                }

                if (i == Mathf.Pow(boardSize, 2) - 1)
                {
                    return true;
                }
            }
        }

        if (board[boardSize - 1].HasPieceOfColor(color))
        {
            for (int i = (boardSize - 1) * 2; i <= Mathf.Pow(boardSize, 2) - boardSize; i += boardSize - 1)
            {
                if (!board[i].HasPieceOfColor(color))
                {
                    break;
                }

                if (i == Mathf.Pow(boardSize, 2) - boardSize)
                {
                    return true;
                }
            }
        }

        return false;
    }
}