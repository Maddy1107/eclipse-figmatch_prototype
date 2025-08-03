using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    [SerializeField] private Button menuButton;
    [SerializeField] private Button restartButton;

    private void Awake()
    {
        menuButton.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowMainMenu();
            ResetGame();
        });

        restartButton.onClick.AddListener(ResetGame);
    }

    private void ResetGame()
    {
        GameManager.Instance.ResetGame();
        GameManager.Instance.GenerateGrid();
    }
}
