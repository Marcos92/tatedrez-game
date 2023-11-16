using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
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
    [HideInInspector] public PlayerColor currentPlayerColor;

    public enum Phase
    {
        TICTACTOE,
        CHESS
    }
    [HideInInspector] public Phase currentPhase;

    private int turnCounter;
    private bool[] validMoves;

    [HideInInspector] public UnityEvent startGameEvent;
    [HideInInspector] public UnityEvent endTurnEvent;
    [HideInInspector] public UnityEvent endGameEvent;

    [Header("UI")]
    public GameObject blur;
    public GameObject whiteTurnLabel;
    public GameObject blackTurnLabel;
    public GameObject whiteNoMovesLabel;
    public GameObject blackNoMovesLabel;
    public GameObject whiteWinLabel;
    public GameObject blackWinLabel;
    public GameObject whiteLoseLabel;
    public GameObject blackLoseLabel;
    public GameObject whiteChessLabel;
    public GameObject blackChessLabel;
    public GameObject whiteTictactoeLabel;
    public GameObject blackTictactoeLabel;
    public GameObject whiteTieLabel;
    public GameObject blackTieLabel;

    [Header("Audio")]
    public AudioClip sfxGameMode;
    public AudioClip sfxEndGame;
    public AudioClip sfxNoMoves;
    private AudioSource audioSource;

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

        audioSource = GetComponent<AudioSource>();

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        SetupBoard();
        PreparePhase(Phase.TICTACTOE);
    }
    private void SetupBoard()
    {
        board = new BoardTile[(int)Mathf.Pow(boardSize, 2)];

        rectTransform = GetComponent<RectTransform>();
        grid = GetComponent<GridLayoutGroup>();
        grid.constraintCount = boardSize;
        float size = rectTransform.rect.width / boardSize - grid.padding.left * 2 / boardSize;
        grid.cellSize = new Vector2(size, size);

        for (int i = 0; i < Mathf.Pow(boardSize, 2); i++)
        {
            board[i] = Instantiate(tilePrefab, transform);
            board[i].SetColor((GetRow(i) % 2) == (GetColumn(i) % 2) ? lightColor : darkColor);
            board[i].SetLabel(i.ToString());
            board[i].gameObject.name = "TILE " + i;
        }
    }

    private void PreparePhase(Phase phase)
    {
        currentPhase = phase;
        blur.SetActive(true);

        audioSource.clip = sfxGameMode;
        audioSource.Play();

        if (phase == Phase.TICTACTOE)
        {
            blackTictactoeLabel.SetActive(true);
            whiteTictactoeLabel.SetActive(true);
            Invoke(nameof(StartTicTacToe), 3.0f);
        }
        else
        {
            blackChessLabel.SetActive(true);
            whiteChessLabel.SetActive(true);
            Invoke(nameof(StartChess), 3.0f);
        }
    }

    private void StartTicTacToe()
    {
        currentPlayerColor = Random.Range(0, 2) > 0 ? PlayerColor.WHITE : PlayerColor.BLACK;

        startGameEvent.Invoke();

        blur.SetActive(false);

        blackTictactoeLabel.SetActive(false);
        whiteTictactoeLabel.SetActive(false);

        whiteTurnLabel.SetActive(currentPlayerColor == PlayerColor.WHITE);
        blackTurnLabel.SetActive(currentPlayerColor == PlayerColor.BLACK);
    }

    private void StartChess()
    {
        blur.SetActive(false);
        blackChessLabel.SetActive(false);
        whiteChessLabel.SetActive(false);

        PostEndTurn();
    }

    public void EndTurn()
    {
        if (++turnCounter >= boardSize * 2 - 1)
        {
            if (CheckMatch())
            {
                EndGame();
                return;
            }
        }

        whiteNoMovesLabel.SetActive(false);
        blackNoMovesLabel.SetActive(false);

        Phase lastPhase = currentPhase;
        ChangeGamePhase();

        //Avoid showing "your turn" label during phase transition, StartChess method will handle the player turn switch
        if (lastPhase == currentPhase)
        {
            PostEndTurn();
        }
    }

    private void PostEndTurn()
    {
        ChangePlayerTurn();
        ResetValidMoves();
        endTurnEvent.Invoke();
    }

    private void EndGame(bool tie = false)
    {
        if (!tie)
        {
            whiteWinLabel.SetActive(currentPlayerColor == PlayerColor.WHITE);
            blackWinLabel.SetActive(currentPlayerColor == PlayerColor.BLACK);
            whiteLoseLabel.SetActive(currentPlayerColor != PlayerColor.WHITE);
            blackLoseLabel.SetActive(currentPlayerColor != PlayerColor.BLACK);
        }
        else
        {
            whiteTieLabel.SetActive(true);
            blackTieLabel.SetActive(true);
        }

        blur.SetActive(true);

        audioSource.clip = sfxEndGame;
        audioSource.Play();

        endGameEvent.Invoke();

        Invoke(nameof(ResetGame), 5.0f);
    }

    private void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ChangeGamePhase()
    {
        if (turnCounter >= boardSize * 2)
        {
            if (currentPhase == Phase.TICTACTOE)
            {
                PreparePhase(Phase.CHESS);
            }
            validMoves = new bool[board.Length];
        }
    }

    private void ChangePlayerTurn()
    {
        PlayerColor otherPlayerColor = currentPlayerColor == PlayerColor.WHITE ? PlayerColor.BLACK : PlayerColor.WHITE;

        if (CheckIfPlayerHasValidMoves(otherPlayerColor))
        {
            currentPlayerColor = otherPlayerColor;
        }
        else if (CheckIfPlayerHasValidMoves(currentPlayerColor))
        {
            whiteNoMovesLabel.SetActive(otherPlayerColor == PlayerColor.WHITE);
            blackNoMovesLabel.SetActive(otherPlayerColor == PlayerColor.BLACK);

            audioSource.clip = sfxNoMoves;
            audioSource.Play();
        }
        else
        {
            EndGame(true);
            return;
        }

        //Set false first to reset animation
        whiteTurnLabel.SetActive(false);
        blackTurnLabel.SetActive(false);
        whiteTurnLabel.SetActive(currentPlayerColor == PlayerColor.WHITE);
        blackTurnLabel.SetActive(currentPlayerColor == PlayerColor.BLACK);
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

    public bool CheckIfPieceHasValidMoves(BoardTile tile, bool highlight = false)
    {
        if (currentPhase != Phase.CHESS)
            return false;

        int index = System.Array.IndexOf(board, tile);
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

        if (highlight)
        {
            HighlightValidMoves();
        }

        return !validMoves.All(value => !value); //Returns true if there is at least one valid move (not all elements are false)
    }

    private bool CheckIfPlayerHasValidMoves(PlayerColor playerColor)
    {
        if (currentPhase == Phase.TICTACTOE)
            return true;

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i].HasPiece() && board[i].GetPiece().playerColor == playerColor && CheckIfPieceHasValidMoves(board[i]))
            {
                return true;
            }
        }

        return false;
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
            board[i].FadeOutAllHighlights();
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
            bool validDiagonal = IsOnSameDiagonal(index, i) && !IsPieceBetweenDiagonal(index, i);
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
            if (!board[i].HasPiece() && !IsOnSameColumn(index, i) && !IsOnSameRow(index, i) && IsInKnightRange(index, i) && !IsOnSameDiagonal(index, i))
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

    private bool IsOnSameDiagonal(int a, int b)
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

        for (int i = first; i < last; i += direction)
        {
            if (board[i].HasPiece())
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

        for (int i = first; i < last; i += direction)
        {
            if (board[i].HasPiece())
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

        for (int i = top; i < bottom; i += direction)
        {
            if (board[i].HasPiece())
            {
                return true;
            }
        }

        return false;
    }
}
