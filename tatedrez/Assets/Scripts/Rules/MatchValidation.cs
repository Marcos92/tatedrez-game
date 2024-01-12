using UnityEngine;

public class MatchValidation
{
    public static bool FindMatch()
    {
        return CheckRows() || CheckColumns() || CheckDiagonals();
    }

    private static bool CheckTileForMatch(Tile tile)
    {
        return tile.Piece && tile.Piece.Color == GameManager.Instance.CurrentPlayerColor;
    }

    private static bool CheckColumns()
    {
        Tile[] board = GameBoard.Instance.Board;
        int boardSize = GameBoard.Instance.Size;

        for (int i = 0; i < boardSize; i++)
        {
            if (CheckTileForMatch(board[i]))
            {
                for (int j = 1; j < boardSize; j++)
                {
                    if (!CheckTileForMatch(board[i + j * boardSize]))
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

    private static bool CheckRows()
    {
        Tile[] board = GameBoard.Instance.Board;
        int boardSize = GameBoard.Instance.Size;

        for (int i = 0; i < boardSize; i++)
        {
            if (CheckTileForMatch(board[i * boardSize]))
            {
                for (int j = 1; j < boardSize; j++)
                {
                    if (!CheckTileForMatch(board[i * boardSize + j]))
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

    private static bool CheckDiagonals()
    {
        Tile[] board = GameBoard.Instance.Board;
        int boardSize = GameBoard.Instance.Size;

        if (CheckTileForMatch(board[0]))
        {
            for (int i = boardSize + 1; i < Mathf.Pow(boardSize, 2); i += boardSize + 1)
            {
                if (!CheckTileForMatch(board[i]))
                {
                    break;
                }

                if (i == Mathf.Pow(boardSize, 2) - 1)
                {
                    return true;
                }
            }
        }

        if (CheckTileForMatch(board[boardSize - 1]))
        {
            for (int i = (boardSize - 1) * 2; i <= Mathf.Pow(boardSize, 2) - boardSize; i += boardSize - 1)
            {
                if (!CheckTileForMatch(board[i]))
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