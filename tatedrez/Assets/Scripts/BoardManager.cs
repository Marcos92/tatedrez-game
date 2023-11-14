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

    private int turnCounter;
    public UnityEvent endTurnEvent;

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
            int x = i % boardSize - 1;
            int y = i / boardSize - 1;

            board[i] = Instantiate(tilePrefab, transform);
            RectTransform rectTransform = board[i].GetComponent<RectTransform>();
            float size = rectTransform.rect.width;
            rectTransform.localPosition = new Vector2(x * size, -y * size);

            board[i].SetColor(boardColors[i % 2]);

            board[i].SetLabel(i.ToString());
            board[i].gameObject.name = "TILE " + i;
        }

        currentPlayerColor = PlayerColor.WHITE;
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
                Debug.Log("Has piece on tile " + i);
                for (int j = 1; j < boardSize; j++)
                {
                    if (!board[i + j * boardSize].CheckMatch(currentPlayerColor))
                    {
                        break;
                    }

                    Debug.Log("Has piece on tile " + (i + j * boardSize));

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
                Debug.Log("Has piece on tile " + (i * boardSize));
                for (int j = 1; j < boardSize; j++)
                {
                    if (!board[i * boardSize + j].CheckMatch(currentPlayerColor))
                    {
                        break;
                    }

                    Debug.Log("Has piece on tile " + (i * boardSize + j));

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
            Debug.Log("Has piece on tile 0");
            for (int i = boardSize + 1; i < Mathf.Pow(boardSize, 2); i += boardSize + 1)
            {
                if (!board[i].CheckMatch(currentPlayerColor))
                {
                    break;
                }

                Debug.Log("Has piece on tile " + i);

                if (i == Mathf.Pow(boardSize, 2) - 1)
                {
                    return true;
                }
            }
        }

        if (board[boardSize - 1].CheckMatch(currentPlayerColor))
        {
            Debug.Log("Has piece on tile " + (boardSize - 1));
            for (int i = (boardSize - 1) * 2; i <= Mathf.Pow(boardSize, 2) - boardSize; i += boardSize - 1)
            {
                if (!board[i].CheckMatch(currentPlayerColor))
                {
                    break;
                }

                Debug.Log("Has piece on tile " + i);

                if (i == Mathf.Pow(boardSize, 2) - boardSize)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
