using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour, IScorable
{
    public static GameManager Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text turnText;
    [SerializeField] private TMP_Text comboText;

    [Header("Card Grid Settings")]
    [SerializeField] private int rows = 2;
    [SerializeField] private int columns = 2;
    [SerializeField] private float spacing = 10f;

    [Header("References")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Sprite[] cardFrontSprites;
    [SerializeField] private Transform gridParent;

    [Header("Game State")]
    private int totalPairs = 0;
    private int matchedPairs = 0;
    private int score = 0;
    private int turns = 0;
    private float timeTaken = 0f;

    private bool isBusy = false;
    private bool isTimerRunning = false;

    private int comboCount = 0;

    private ICard firstCard;
    private ICard secondCard;

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

        GridLayoutGroup grid = gridParent.GetComponent<GridLayoutGroup>();
        RectTransform rect = gridParent.GetComponent<RectTransform>();

        float width = rect.rect.width;
        float height = rect.rect.height;

        if (grid == null || width <= 0 || height <= 0)
        {
            Debug.LogError($"⚠️ Grid or Rect not ready. Width/Height: {width} / {height}");
            yield break;
        }

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;
        grid.spacing = new Vector2(spacing, spacing);

        float cardWidth = (width - spacing * (columns - 1)) / columns;
        float cardHeight = (height - spacing * (rows - 1)) / rows;
        grid.cellSize = new Vector2(cardWidth, cardHeight);

        CardGridGenerator generator = new CardGridGenerator(rows, columns);
        List<int> cardIDs = generator.CardIDs;

        SpawnCards(cardIDs);

        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        SetTotalPairs(cardIDs.Count / 2);
    }

    private void SpawnCards(List<int> cardIDs)
    {
        foreach (int id in cardIDs)
        {
            GameObject cardGO = Instantiate(cardPrefab, gridParent);
            ICard card = cardGO.GetComponent<ICard>();

            int spriteIndex = id % cardFrontSprites.Length;
            card.Setup(id, cardFrontSprites[spriteIndex]);
        }
    }

    public void OnCardSelected(ICard card)
    {
        if (isBusy || card == firstCard) return;

        if (firstCard == null)
        {
            firstCard = card;
        }
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
            int baseScore = 10;
            int bonus = comboCount > 1 ? baseScore * comboCount : baseScore;
            AddScore(bonus);

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

        firstCard = null;
        secondCard = null;
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
        scoreText.text = score.ToString();
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
        turnText.text = turns.ToString();
    }

    public void SetTotalPairs(int value)
    {
        totalPairs = value;
    }

    public void ResetGame()
    {
        matchedPairs = 0;
        ResetScore();
        ResetTurns();
        firstCard = null;
        secondCard = null;
        isBusy = false;
        timeTaken = 0f;
        isTimerRunning = false;
        comboCount = 0;
        comboText.text = "";
    }
}
