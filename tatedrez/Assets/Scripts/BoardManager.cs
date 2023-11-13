using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public BoardTile tilePrefab;
    private BoardTile[] board;
    private const int boardSize = 3;
    public Color[] boardColors;

    void Awake()
    {
        board = new BoardTile[(int)Mathf.Pow(boardSize, 2)];

        for (int i = 0; i < Mathf.Pow(boardSize, 2); i++)
        {
            int x = i % boardSize - 1;
            int y = i / boardSize - 1;

            board[i] = Instantiate(tilePrefab, transform);
            board[i].transform.localPosition = new Vector2(x, y);
            board[i].SetColor(boardColors[i % 2]);
        }
    }
}
