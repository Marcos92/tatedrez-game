using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public BoardTile tilePrefab;
    private BoardTile[] board;
    private const int boardSize = 3;
    public Color lightColor;
    public Color darkColor;

    private RectTransform rectTransform;
    private GridLayoutGroup grid;

    public enum PlayerColor
    {
        WHITE,
        BLACK
    }
    public PlayerColor currentPlayerColor;

    public enum Phase
    {
        TICTACTOE,
        CHESS
    }
    public Phase currentPhase;

    private int turnCounter;
    public UnityEvent endTurnEvent;

    private bool[] validMoves;

    public static BoardManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        board = new BoardTile[(int)Mathf.Pow(boardSize, 2)];

        rectTransform = GetComponent<RectTransform>();
        grid = GetComponent<GridLayoutGroup>();
        grid.constraintCount = boardSize;
        grid.cellSize = new Vector2(rectTransform.rect.width / boardSize, rectTransform.rect.height / boardSize);

        for (int i = 0; i < Mathf.Pow(boardSize, 2); i++)
        {
            board[i] = Instantiate(tilePrefab, transform);
            board[i].SetColor((GetRow(i) % 2) == (GetColumn(i) % 2) ? lightColor : darkColor);
            board[i].SetLabel(i.ToString());
            board[i].gameObject.name = "TILE " + i;
        }

        currentPlayerColor = PlayerColor.WHITE;
        currentPhase = Phase.CHESS;
    }

    public void EndTurn()
    {
        if (++turnCounter >= boardSize * 2 - 1)
        {
            if (CheckMatch())
            {
                Debug.Log(currentPlayerColor + " wins!");
            }
        }

        if (turnCounter >= boardSize * 2)
        {
            currentPhase = Phase.CHESS;
        }

        //ChangePlayerTurn();

        endTurnEvent.Invoke();
    }

    private void ChangePlayerTurn()
    {
        if (currentPlayerColor == PlayerColor.WHITE)
        {
            currentPlayerColor = PlayerColor.BLACK;
        }
        else
        {
            currentPlayerColor = PlayerColor.WHITE;
        }
    }

    private bool CheckMatch()
    {
        //Columns
        for (int i = 0; i < boardSize; i++)
        {
            if (board[i].CheckMatch(currentPlayerColor))
            {
                for (int j = 1; j < boardSize; j++)
                {
                    if (!board[i + j * boardSize].CheckMatch(currentPlayerColor))
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

        //Rows
        for (int i = 0; i < boardSize; i++)
        {
            if (board[i * boardSize].CheckMatch(currentPlayerColor))
            {
                for (int j = 1; j < boardSize; j++)
                {
                    if (!board[i * boardSize + j].CheckMatch(currentPlayerColor))
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

        //Diagonals
        if (board[0].CheckMatch(currentPlayerColor))
        {
            for (int i = boardSize + 1; i < Mathf.Pow(boardSize, 2); i += boardSize + 1)
            {
                if (!board[i].CheckMatch(currentPlayerColor))
                {
                    break;
                }

                if (i == Mathf.Pow(boardSize, 2) - 1)
                {
                    return true;
                }
            }
        }

        if (board[boardSize - 1].CheckMatch(currentPlayerColor))
        {
            for (int i = (boardSize - 1) * 2; i <= Mathf.Pow(boardSize, 2) - boardSize; i += boardSize - 1)
            {
                if (!board[i].CheckMatch(currentPlayerColor))
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

    public void CheckValidMoves(BoardTile tile)
    {
        if (currentPhase != Phase.CHESS)
            return;

        int index = Array.IndexOf(board, tile);
        Piece.Type type = tile.GetPiece().type;
        validMoves = new bool[board.Length];

        switch (type)
        {
            case Piece.Type.ROOK:
                CheckRookValidMoves(index);
                break;

            case Piece.Type.BISHOP:
                CheckBishopValidMoves(index);
                break;

            case Piece.Type.KNIGHT:
                CheckKnightValidMoves(index);
                break;
        }

        HighlightValidMoves();
    }

    private void HighlightValidMoves()
    {
        for (int i = 0; i < validMoves.Length; i++)
        {
            if (validMoves[i])
            {
                board[i].ShowValidMove();
            }
            else
            {
                board[i].ToggleInteraction(false);
            }
        }
    }

    public void ResetValidMoves()
    {
        if (currentPhase != Phase.CHESS)
            return;

        validMoves = new bool[board.Length];
        for (int i = 0; i < validMoves.Length; i++)
        {
            board[i].ToggleInteraction(true);
            board[i].HideValidMove();
        }
    }

    private void CheckRookValidMoves(int index)
    {
        for (int i = 0; i < Mathf.Pow(boardSize, 2); i++)
        {
            bool validRow = IsOnSameRow(index, i) && !IsPieceBetweenRow(index, i);
            bool validColumn = IsOnSameColumn(index, i) && !IsPieceBetweenColumn(index, i);
            if (!board[i].HasPiece() && index != i && (validColumn || validRow))
            {
                validMoves[i] = true;
            }
        }
    }

    private void CheckBishopValidMoves(int index)
    {
        for (int i = 0; i < Mathf.Pow(boardSize, 2); i++)
        {
            bool validDiagonal = IsDiagonal(index, i) && !IsPieceBetweenDiagonal(index, i);
            if (!board[i].HasPiece() && validDiagonal)
            {
                validMoves[i] = true;
            }
        }
    }

    private void CheckKnightValidMoves(int index)
    {
        for (int i = 0; i < Mathf.Pow(boardSize, 2); i++)
        {
            if (!board[i].HasPiece() && !IsOnSameColumn(index, i) && !IsOnSameRow(index, i) && IsInKnightRange(index, i) && !IsDiagonal(index, i))
            {
                validMoves[i] = true;
            }
        }
    }

    private int GetRow(int i)
    {
        return i / boardSize;
    }

    private int GetColumn(int i)
    {
        return i % boardSize;
    }

    private bool IsOnSameRow(int a, int b)
    {
        return GetRow(a) == GetRow(b);
    }

    private bool IsOnSameColumn(int a, int b)
    {
        return GetColumn(a) == GetColumn(b);
    }

    private bool IsDiagonal(int a, int b)
    {
        return Mathf.Abs(GetRow(a) - GetRow(b)) == Mathf.Abs(GetColumn(a) - GetColumn(b));
    }

    private bool IsInKnightRange(int a, int b)
    {
        return Mathf.Abs(GetRow(a) - GetRow(b)) < 3 && Mathf.Abs(GetColumn(a) - GetColumn(b)) < 3;
    }

    private bool IsPieceBetweenRow(int a, int b)
    {
        int direction = 1;
        int first = Mathf.Min(a, b) + direction;
        int last = Mathf.Max(a, b);

        for(int i = first; i < last; i += direction)
        {
            if(board[i].HasPiece())
            {
                return true;
            }
        }

        return false;
    }

    private bool IsPieceBetweenColumn(int a, int b)
    {
        int direction = boardSize;
        int first = Mathf.Min(a, b) + direction;
        int last = Mathf.Max(a, b);

        for(int i = first; i < last; i += direction)
        {
            if(board[i].HasPiece())
            {
                return true;
            }
        }

        return false;
    }

    private bool IsPieceBetweenDiagonal(int a, int b)
    {
        int top = Mathf.Min(a, b);
        int bottom = Mathf.Max(a, b);

        bool topLeftDiagonal = GetColumn(top) < GetColumn(bottom); //Otherwise we have a top-right diagonal
        int direction = boardSize + (topLeftDiagonal ? 1 : -1);
        top += direction;

        for(int i = top; i < bottom; i += direction)
        {
            if(board[i].HasPiece())
            {
                return true;
            }
        }

        return false;
    }
}
