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

    private List<int> lastGeneratedCardIDs = new();

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

    public void CheckSaveandStart()
    {
        if (SaveSystem.HasSave())
        {
            Debug.Log("Resuming saved game...");
            StartCoroutine(ResumeGrid());
        }
        else
        {
            Debug.Log("Starting new game...");
            GenerateGrid();
        }
    }

    public void GenerateGrid()
    {
        ClearGrid();

        timeTaken = 0f;
        isTimerRunning = true;

        StartCoroutine(SetupGridAndSpawn());
    }

    private void ClearGrid()
    {
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);
    }

    private IEnumerator SetupGridAndSpawn()
    {
        yield return new WaitForEndOfFrame();

        SetupGridLayout();

        var generator = new CardGridGenerator(rows, columns);
        List<int> cardIDs = generator.CardIDs;
        lastGeneratedCardIDs = cardIDs;

        SpawnCards(cardIDs, new List<int>());

        LayoutRebuilder.ForceRebuildLayoutImmediate(gridParent.GetComponent<RectTransform>());
        SetTotalPairs(cardIDs.Count / 2);
    }

    private IEnumerator ResumeGrid()
    {
        yield return new WaitForEndOfFrame();

        ClearGrid();
        SetupGridLayout();

        List<int> cardIDs = SaveSystem.GetShuffledIDs();
        List<int> matchedIDs = SaveSystem.GetMatchedIDs();

        if (cardIDs == null || cardIDs.Count == 0)
        {
            Debug.LogWarning("⚠️ No saved shuffled data. Falling back to new game.");
            GenerateGrid();
            yield break;
        }

        lastGeneratedCardIDs = new List<int>(cardIDs);
        SpawnCards(cardIDs, matchedIDs);

        LayoutRebuilder.ForceRebuildLayoutImmediate(gridParent.GetComponent<RectTransform>());
        SetTotalPairs(cardIDs.Count / 2);
        isTimerRunning = true;
    }

    private void SetupGridLayout()
    {
        GridLayoutGroup grid = gridParent.GetComponent<GridLayoutGroup>();
        RectTransform rect = gridParent.GetComponent<RectTransform>();

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;
        grid.spacing = new Vector2(spacing, spacing);

        float width = rect.rect.width;
        float height = rect.rect.height;
        float cardWidth = (width - spacing * (columns - 1)) / columns;
        float cardHeight = (height - spacing * (rows - 1)) / rows;

        grid.cellSize = new Vector2(cardWidth, cardHeight);
    }

    private void SpawnCards(List<int> cardIDs, List<int> matchedIDs)
    {
        foreach (int id in cardIDs)
        {
            GameObject cardGO = Instantiate(cardPrefab, gridParent);
            ICard card = cardGO.GetComponent<ICard>();

            int spriteIndex = id % cardFrontSprites.Length;
            card.Setup(id, cardFrontSprites[spriteIndex]);

            if (matchedIDs.Contains(id))
                card.SetMatched();
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

            SaveProgress();
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

    private void SaveProgress()
    {
        List<int> matchedIDs = new();
        foreach (Transform child in gridParent)
        {
            var c = child.GetComponent<Card>();
            if (c.IsMatched) matchedIDs.Add(c.CardID);
        }

        SaveSystem.SaveProgress(score, turns, timeTaken, rows, columns, comboCount, matchedIDs, lastGeneratedCardIDs);
    }

    private void LoadSavedProgress()
    {
        score = SaveSystem.GetScore();
        turns = SaveSystem.GetTurns();
        timeTaken = SaveSystem.GetTime();
        comboCount = SaveSystem.GetCombo();
        rows = SaveSystem.GetRows();
        columns = SaveSystem.GetCols();

        scoreText.text = score.ToString();
        turnText.text = turns.ToString();
        comboText.text = comboCount > 1 ? $"Combo x{comboCount}" : "";
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
