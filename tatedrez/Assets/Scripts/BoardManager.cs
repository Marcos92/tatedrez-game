using System;
using UnityEngine;
using UnityEngine.Events;

public class BoardManager : MonoBehaviour
{
    public BoardTile tilePrefab;
    private BoardTile[] board;
    private const int boardSize = 3;
    public Color[] boardColors;

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

        for (int i = 0; i < Mathf.Pow(boardSize, 2); i++)
        {
            int x = GetColumn(i) - 1;
            int y = GetRow(i) - 1;

            board[i] = Instantiate(tilePrefab, transform);
            RectTransform rectTransform = board[i].GetComponent<RectTransform>();
            float size = rectTransform.rect.width;
            rectTransform.localPosition = new Vector2(x * size, -y * size);

            board[i].SetColor(boardColors[i % 2]);

            board[i].SetLabel(i.ToString());
            board[i].gameObject.name = "TILE " + i;
        }

        currentPlayerColor = PlayerColor.WHITE;
        currentPhase = Phase.TICTACTOE;
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

        if (currentPlayerColor == PlayerColor.WHITE)
        {
            currentPlayerColor = PlayerColor.BLACK;
        }
        else
        {
            currentPlayerColor = PlayerColor.WHITE;
        }

        endTurnEvent.Invoke();
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
        if(currentPhase != Phase.CHESS)
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
        if(currentPhase != Phase.CHESS)
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
            if(!board[i].HasPiece() && index != i && (IsOnSameColumn(index, i) || IsOnSameRow(index, i)))
            {
                validMoves[i] = true;
            }
        }
    }

    private void CheckBishopValidMoves(int index)
    {
        for (int i = 0; i < Mathf.Pow(boardSize, 2); i++)
        {
            if(!board[i].HasPiece() && index % 2 == i % 2 && !IsOnSameColumn(index, i) && !IsOnSameRow(index, i))
            {
                validMoves[i] = true;
            }
        }
    }

    private void CheckKnightValidMoves(int index)
    {
        for (int i = 0; i < Mathf.Pow(boardSize, 2); i++)
        {
            if(!board[i].HasPiece() && index % 2 != i % 2 && !IsOnSameColumn(index, i) && !IsOnSameRow(index, i))
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
}
