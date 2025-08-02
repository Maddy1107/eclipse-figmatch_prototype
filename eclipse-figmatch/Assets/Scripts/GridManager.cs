using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int rows = 2;
    [SerializeField] private int columns = 2;
    [SerializeField] private float spacing = 10f;

    [Header("Card Setup")]
    [SerializeField] private GameObject cardPrefab; // must have CardUI (implements ICard)
    [SerializeField] private Sprite[] cardFrontSprites;
    [SerializeField] private Transform cardGridParent; // should be a UI Panel with GridLayoutGroup

    private List<int> cardIDs = new();

    private void Start()
    {
        SetupGridLayout();
        GenerateCardIDs();
        Shuffle(cardIDs);
        SpawnCards();
    }

    private void SetupGridLayout()
    {
        GridLayoutGroup grid = cardGridParent.GetComponent<GridLayoutGroup>();
        if (grid == null) return;

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;
        grid.spacing = new Vector2(spacing, spacing);

        // Optional: calculate size based on parent rect size
        RectTransform parentRect = cardGridParent.GetComponent<RectTransform>();
        float cardWidth = (parentRect.rect.width - (spacing * (columns - 1))) / columns;
        float cardHeight = (parentRect.rect.height - (spacing * (rows - 1))) / rows;
        grid.cellSize = new Vector2(cardWidth, cardHeight);
    }

    private void GenerateCardIDs()
    {
        int pairCount = (rows * columns) / 2;

        for (int i = 0; i < pairCount; i++)
        {
            cardIDs.Add(i);
            cardIDs.Add(i); // Add a pair of each ID
        }
    }

    private void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }

    private void SpawnCards()
    {
        for (int i = 0; i < cardIDs.Count; i++)
        {
            GameObject cardGO = Instantiate(cardPrefab, cardGridParent);
            ICard card = cardGO.GetComponent<ICard>();

            if (card == null)
            {
                Debug.LogError("Card prefab must implement ICard.");
                continue;
            }

            int id = cardIDs[i];
            Sprite sprite = cardFrontSprites[id];

            card.Setup(id, sprite);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetTotalPairs(cardIDs.Count / 2);
        }
    }
}
