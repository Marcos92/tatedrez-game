using System;
using UnityEngine;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private Tile tilePrefab;

    private Tile[] board;
    public Tile[] Board { get => board; }

    private const int boardSize = 3;
    public int Size { get => boardSize; }

    private RectTransform rectTransform;
    private GridLayoutGroup grid;

    public static GameBoard Instance { get; private set; }
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

        SetupBoard();
    }

    private void SetupBoard()
    {
        int tileCount = (int)Mathf.Pow(boardSize, 2);
        board = new Tile[tileCount];

        rectTransform = GetComponent<RectTransform>();
        grid = GetComponent<GridLayoutGroup>();
        grid.constraintCount = boardSize;
        float size = rectTransform.rect.width / boardSize - grid.padding.left * 2 / boardSize;
        grid.cellSize = new Vector2(size, size);

        for (int i = 0; i < tileCount; i++)
        {
            board[i] = Instantiate(tilePrefab, transform);
            board[i].Initialize((BoardUtils.GetRow(i) % 2) == (BoardUtils.GetColumn(i) % 2));
            board[i].gameObject.name = "TILE " + i;
        }
    }

    public int GetIndexOfTile(Tile tile)
    {
        return Array.IndexOf(board, tile);
    }
}