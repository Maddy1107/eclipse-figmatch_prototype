using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour, IScorable
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TMP_Text scoreText, turnText, comboText;

    [Header("Grid Settings")]
    [SerializeField] private int rows = 2, columns = 2;
    [SerializeField] private float spacing = 10f;

    [Header("References")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Sprite[] cardFrontSprites;
    [SerializeField] private Transform gridParent;

    private int totalPairs, matchedPairs, score, turns, comboCount;
    private float timeTaken;
    private bool isBusy, isTimerRunning;

    private ICard firstCard, secondCard;

    public bool IsBusy => isBusy;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        ResetGame();
    }

    private void Update()
    {
        if (isTimerRunning)
            timeTaken += Time.deltaTime;
    }

    public void GenerateGrid()
    {
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        timeTaken = 0f;
        isTimerRunning = true;

        StartCoroutine(SetupGridAndSpawn());
    }

    private IEnumerator SetupGridAndSpawn()
    {
        yield return new WaitForEndOfFrame();

        var grid = gridParent.GetComponent<GridLayoutGroup>();
        var rect = gridParent.GetComponent<RectTransform>();

        if (grid == null || rect.rect.width <= 0 || rect.rect.height <= 0)
        {
            Debug.LogError($"⚠️ Grid or Rect not ready. Width/Height: {rect.rect.width} / {rect.rect.height}");
            yield break;
        }

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;
        grid.spacing = new Vector2(spacing, spacing);
        grid.cellSize = new Vector2(
            (rect.rect.width - spacing * (columns - 1)) / columns,
            (rect.rect.height - spacing * (rows - 1)) / rows
        );

        var generator = new CardGridGenerator(rows, columns);
        SpawnCards(generator.CardIDs);

        yield return null;
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);

        SetTotalPairs(generator.CardIDs.Count / 2);
    }

    private void SpawnCards(List<int> cardIDs)
    {
        foreach (int id in cardIDs)
        {
            var cardGO = Instantiate(cardPrefab, gridParent);
            var card = cardGO.GetComponent<ICard>();
            cardGO.transform.localScale = Vector3.one;

            int spriteIndex = id % cardFrontSprites.Length;
            card.Setup(id, cardFrontSprites[spriteIndex]);
        }
    }

    public void OnCardSelected(ICard card)
    {
        if (isBusy || card == firstCard) return;

        if (firstCard == null) firstCard = card;
        else
        {
            secondCard = card;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        isBusy = true;
        yield return new WaitForSeconds(0.5f);
        AddTurn();

        if (firstCard.CardID == secondCard.CardID)
        {
            firstCard.SetMatched();
            secondCard.SetMatched();
            matchedPairs++;

            comboCount++;
            int points = comboCount > 1 ? 10 * comboCount : 10;
            AddScore(points);
            comboText.text = comboCount > 1 ? $"Combo x{comboCount}" : "";

            AudioManager.Instance?.PlayMatch();

            if (matchedPairs >= totalPairs)
            {
                isTimerRunning = false;
                UIManager.Instance.ShowGameOver();
                GameOverPanel.Instance.Show(score, turns, timeTaken);
            }
        }
        else
        {
            comboCount = 0;
            comboText.text = "";
            firstCard.FlipBack();
            secondCard.FlipBack();
            AudioManager.Instance?.PlayMismatch();
        }

        firstCard = secondCard = null;
        isBusy = false;
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = score.ToString();
    }

    public int GetScore() => score;

    public void ResetScore()
    {
        score = 0;
        scoreText.text = "0";
    }

    public void AddTurn()
    {
        turns++;
        turnText.text = turns.ToString();
    }

    public int GetTurns() => turns;

    public void ResetTurns()
    {
        turns = 0;
        turnText.text = "0";
    }

    public void SetTotalPairs(int value) => totalPairs = value;

    public void ResetGame()
    {
        matchedPairs = score = turns = comboCount = 0;
        firstCard = secondCard = null;
        isBusy = false;
        timeTaken = 0f;
        isTimerRunning = false;

        scoreText.text = "0";
        turnText.text = "0";
        comboText.text = "";
    }
}
