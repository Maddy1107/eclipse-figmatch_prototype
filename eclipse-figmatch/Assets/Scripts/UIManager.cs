using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI Screens")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject statsPanel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start() => ShowMainMenu();

    public void ShowMainMenu() => SetActivePanel(mainMenuPanel);
    public void ShowGame() => SetActivePanel(gamePanel);
    public void ShowGameOver() => gameOverPanel.SetActive(true);
    public void ShowStats() => statsPanel.SetActive(true);

    private void SetActivePanel(GameObject activePanel)
    {
        mainMenuPanel.SetActive(false);
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        activePanel.SetActive(true);
    }
}
