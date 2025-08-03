using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        startButton.onClick.AddListener(OnStartButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }
    private void OnStartButtonClicked()
    {
        UIManager.Instance.ShowGame();
        GameManager.Instance.ResetGame();
        GameManager.Instance.GenerateGrid();
    }
    private void OnExitButtonClicked()
    {
        Application.Quit();
        Debug.Log("Application Quit");
    }
}
