using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private TurnManager turnManager;
    private PhaseManager phaseManager;

    public PlayerColor CurrentPlayerColor { get => turnManager.CurrentPlayerColor; }
    public PlayerColor OtherPlayerColor { get => turnManager.OtherPlayerColor; }
    public GamePhase CurrentPhase { get => phaseManager.CurrentPhase; }

    private IMoveValidation moveValidation;
    public IMoveValidation MoveValidation { get => moveValidation; }

    public static GameManager Instance { get; private set; }
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

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        turnManager = GetComponent<TurnManager>();
        phaseManager = GetComponent<PhaseManager>();

        moveValidation = new TictactoeMoveValidation();

        GameEvents.PiecePlayed.AddListener(EndTurn);
    }

    void Start()
    {
        turnManager.ChooseStartingPlayer();
        phaseManager.StartTicTacToe();
        Invoke(nameof(StartGame), 3.0f);
    }

    private void StartGame()
    {
        turnManager.StartTurn();
    }

    private void EndTurn()
    {
        turnManager.EndTurn();
        CheckForMatches();
    }

    private void CheckForMatches()
    {
        if (turnManager.TurnCount >= GameBoard.Instance.Size * 2 - 1)
        {
            if (MatchValidation.FindMatch(turnManager.CurrentPlayerColor))
            {
                EndGame();
            }
            else
            {
                CheckForPhaseChange();
            }

            return;
        }

        NextTurn();
    }

    private void CheckForPhaseChange()
    {
        if (turnManager.TurnCount >= GameBoard.Instance.Size * 2)
        {
            if (phaseManager.CurrentPhase == GamePhase.TICTACTOE)
            {
                phaseManager.StartChess();
                moveValidation = new ChessMoveValidation();
                Invoke(nameof(NextTurn), 3.0f);
                return;
            }
        }

        NextTurn();
    }

    private void NextTurn()
    {
        if (moveValidation.CanPlayerMove(turnManager.OtherPlayerColor))
        {
            turnManager.ChangeTurn();
        }
        else if (moveValidation.CanPlayerMove(turnManager.CurrentPlayerColor))
        {
            GameEvents.NoMoves.Invoke();
            turnManager.StartTurn();
        }
        else
        {
            TieGame();
        }
    }

    private void TieGame()
    {
        GameEvents.TieGame.Invoke();
        Invoke(nameof(ResetGame), 5.0f);
    }

    private void EndGame()
    {
        GameEvents.EndGame.Invoke();
        Invoke(nameof(ResetGame), 5.0f);
    }

    private void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}