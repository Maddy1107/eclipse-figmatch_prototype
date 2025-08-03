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

    [Header("Card Grid Settings")]
    [SerializeField] private int rows = 2;
    [SerializeField] private int columns = 2;
    [SerializeField] private float spacing = 10f;

    [Header("References")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Sprite[] cardFrontSprites;
    [SerializeField] private Transform gridParent;

    private int totalPairs = 0;
    private int matchedPairs = 0;
    private int score = 0;
    private int turns = 0;
    private bool isBusy = false;

    private ICard firstCard;
    private ICard secondCard;

    public bool IsBusy => isBusy;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        ResetGame(); // just resets internal state, does not spawn cards yet
    }

    public void GenerateGrid()
    {
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

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
            Debug.LogError("⚠️ Grid or Rect not ready. Width/Height: " + width + "/" + height);
            yield break;
        }

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;
        grid.spacing = new Vector2(spacing, spacing);

        float cardWidth = (width - (spacing * (columns - 1))) / columns;
        float cardHeight = (height - (spacing * (rows - 1))) / rows;
        grid.cellSize = new Vector2(cardWidth, cardHeight);

        var generator = new CardGridGenerator(rows, columns);
        var cardIDs = generator.CardIDs;

        for (int i = 0; i < cardIDs.Count; i++)
        {
            GameObject cardGO = Instantiate(cardPrefab, gridParent);
            ICard card = cardGO.GetComponent<ICard>();

            int id = cardIDs[i];
            int spriteIndex = id % cardFrontSprites.Length;
            card.Setup(id, cardFrontSprites[spriteIndex]);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        SetTotalPairs(cardIDs.Count / 2);
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

            AddScore(10);
            AudioManager.Instance?.PlayMatch();

            if (matchedPairs >= totalPairs)
            {
                Debug.Log("🎉 Game Over!");
                // TODO: Trigger GameOver screen or win UI
            }
        }
        else
        {
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
    }
}
