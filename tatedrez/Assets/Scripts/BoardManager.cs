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
        }
    }

    public void ResetValidMoves()
    {
        validMoves = new bool[board.Length];
        for (int i = 0; i < validMoves.Length; i++)
        {
            board[i].HideValidMove();
        }
    }

    private void CheckRookValidMoves(int index)
    {
        int verticalMove = boardSize;
        int horizontalMove = 1;

        int upLimit = 0;
        int downLimit = (int)Mathf.Pow(boardSize, 2);
        int leftLimit = boardSize * (GetColumn(index) + 1) - boardSize - 1;
        int rightLimit = boardSize * (GetColumn(index) + 1);

        //Up
        for (int i = index - verticalMove; i >= upLimit; i -= verticalMove)
        {
            if (!board[i].HasPiece())
            {
                validMoves[i] = true;
            }
            else
            {
                break;
            }
        }

        //Down
        for (int i = index + verticalMove; i < downLimit; i += verticalMove)
        {
            if (!board[i].HasPiece())
            {
                validMoves[i] = true;
            }
            else
            {
                break;
            }
        }

        //Left
        for (int i = index - horizontalMove; i > leftLimit; i -= horizontalMove)
        {
            if (!board[i].HasPiece())
            {
                validMoves[i] = true;
            }
            else
            {
                break;
            }
        }

        //Right
        for (int i = index + horizontalMove; i < rightLimit; i += horizontalMove)
        {
            if (!board[i].HasPiece())
            {
                validMoves[i] = true;
            }
            else
            {
                break;
            }
        }
    }

    private void CheckBishopValidMoves(int index)
    {
        int leftDiagonalMove = boardSize + 1;
        int rightDiagonalMove = boardSize - 1;

        //Up-Left
        if (GetColumn(index) > 0 && GetRow(index) > 0)
        {
            for (int i = index - leftDiagonalMove; i >= 0; i -= leftDiagonalMove)
            {
                if (!board[i].HasPiece() && !IsOnSameRow(index, i) && !IsOnSameColumn(index, i))
                {
                    validMoves[i] = true;
                }
                else
                {
                    break;
                }
            }
        }

        //Down-Right
        if (GetColumn(index) < boardSize - 1 && GetRow(index) < boardSize - 1)
        {
            for (int i = index + leftDiagonalMove; i < (int)Mathf.Pow(boardSize, 2); i += leftDiagonalMove)
            {
                if (!board[i].HasPiece() && !IsOnSameRow(index, i) && !IsOnSameColumn(index, i))
                {
                    validMoves[i] = true;
                }
                else
                {
                    break;
                }
            }
        }

        //Up-Right
        if (GetColumn(index) < boardSize - 1 && GetRow(index) > 0)
        {
            for (int i = index - rightDiagonalMove; i >= 0; i -= rightDiagonalMove)
            {
                if (!board[i].HasPiece() && !IsOnSameRow(index, i) && !IsOnSameColumn(index, i))
                {
                    validMoves[i] = true;
                }
                else
                {
                    break;
                }
            }
        }

        //Down-Left
        if (GetColumn(index) > 0 && GetRow(index) < boardSize - 1)
        {
            for (int i = index + rightDiagonalMove; i < (int)Mathf.Pow(boardSize, 2); i += rightDiagonalMove)
            {
                if (!board[i].HasPiece() && !IsOnSameRow(index, i) && !IsOnSameColumn(index, i))
                {
                    validMoves[i] = true;
                }
                else
                {
                    break;
                }
            }
        }
    }

    private void CheckKnightValidMoves(int index)
    {
        int[] moves = { -(boardSize - 2), -(boardSize * 2 - 1), -(boardSize * 2 + 1), boardSize - 2, boardSize * 2 - 1, boardSize * 2 + 1 };

        for (int i = 0; i < moves.Length; i++)
        {
            int newIndex = index + moves[i];
            if (newIndex >= 0 && newIndex < (int)Mathf.Pow(boardSize, 2) && !board[newIndex].HasPiece()
            && !IsOnSameColumn(index, newIndex) && !IsOnSameRow(index, newIndex))
            {
                validMoves[newIndex] = true;
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
