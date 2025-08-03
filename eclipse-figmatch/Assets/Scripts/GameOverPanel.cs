using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    public static GameOverPanel Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private TMP_Text timeTakenText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text turnsTakenText;

    [Header("Buttons")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        restartButton.onClick.AddListener(() =>
        {
            GameManager.Instance.ResetGame();
            GameManager.Instance.GenerateGrid();
            gameObject.SetActive(false);
        });

        menuButton.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowMainMenu();
            GameManager.Instance.ResetGame();
            gameObject.SetActive(false);
        });
    }

    public void Show(int score, int turns, float timeTaken)
    {
        int minutes = Mathf.FloorToInt(timeTaken / 60f);
        int seconds = Mathf.FloorToInt(timeTaken % 60f);

        scoreText.text = $"Score: {score}";
        turnsTakenText.text = $"Turns Taken: {turns}";
        timeTakenText.text = $"Time Taken: {(minutes > 0 ? $"{minutes}m {seconds}s" : $"{seconds}s")}";

        ShowPanel();
    }

    private void ShowPanel()
    {
        gameObject.SetActive(true);
    }
}
