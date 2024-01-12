using UnityEngine;

public class UIManager : MonoBehaviour
{
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

    void Awake()
    {
        GameEvents.EndGame.AddListener(ShowEndGameUI);
        GameEvents.TieGame.AddListener(ShowTieUI);

        GameEvents.StartTurn.AddListener(ShowTurnStartUI);

        GameEvents.StartTicTacToe.AddListener(ShowTickTacToeUI);
        GameEvents.StartChess.AddListener(ShowChessUI);
    }

    private void ShowTickTacToeUI()
    {
        blur.SetActive(true);
        blackTictactoeLabel.SetActive(true);
        whiteTictactoeLabel.SetActive(true);
    }

    private void ShowChessUI()
    {
        blur.SetActive(true);
        blackChessLabel.SetActive(true);
        whiteChessLabel.SetActive(true);
    }

    private void ShowTurnStartUI()
    {
        blur.SetActive(false);

        blackTictactoeLabel.SetActive(false);
        whiteTictactoeLabel.SetActive(false);

        blackChessLabel.SetActive(false);
        whiteChessLabel.SetActive(false);

        whiteNoMovesLabel.SetActive(false);
        blackNoMovesLabel.SetActive(false);

        whiteTurnLabel.SetActive(false);
        blackTurnLabel.SetActive(false);
        whiteTurnLabel.SetActive(GameManager.Instance.CurrentPlayerColor == PlayerColor.WHITE);
        blackTurnLabel.SetActive(GameManager.Instance.CurrentPlayerColor == PlayerColor.BLACK);
    }

    private void ShowTieUI()
    {
        blur.SetActive(true);

        whiteTieLabel.SetActive(true);
        blackTieLabel.SetActive(true);
    }

    private void ShowEndGameUI()
    {
        blur.SetActive(true);

        whiteWinLabel.SetActive(GameManager.Instance.CurrentPlayerColor == PlayerColor.WHITE);
        blackWinLabel.SetActive(GameManager.Instance.CurrentPlayerColor == PlayerColor.BLACK);
        whiteLoseLabel.SetActive(GameManager.Instance.CurrentPlayerColor != PlayerColor.WHITE);
        blackLoseLabel.SetActive(GameManager.Instance.CurrentPlayerColor != PlayerColor.BLACK);
    }

    private void ShowNoMovesUI()
    {
        whiteNoMovesLabel.SetActive(GameManager.Instance.CurrentPlayerColor == PlayerColor.BLACK);
        blackNoMovesLabel.SetActive(GameManager.Instance.CurrentPlayerColor == PlayerColor.WHITE);
    }
}