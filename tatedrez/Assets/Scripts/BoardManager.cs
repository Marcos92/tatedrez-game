using UnityEngine;
using UnityEngine.Events;

public class BoardManager : MonoBehaviour
{
    public BoardTile tilePrefab;
    private BoardTile[] board;
    private const int boardSize = 3;
    public Color[] boardColors;

    public enum Player
    {
        WHITE,
        BLACK
    }
    public Player playerTurn;

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

            board[i].gameObject.name = "TILE " + (x + 2).ToString() + "-" + (y + 2).ToString();
        }

        playerTurn = Player.WHITE;
    }

    public void EndTurn()
    {
        if (playerTurn == Player.WHITE)
        {
            playerTurn = Player.BLACK;
        }
        else
        {
            playerTurn = Player.WHITE;
        }

        endTurnEvent.Invoke();
    }
}
