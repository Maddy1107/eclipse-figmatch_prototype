using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Sprite[] cardSprites;
    public Transform cardGridParent;

    public int rows = 2;
    public int columns = 2;

    private List<int> cardIDs = new();

    private void Start()
    {
        GenerateCardIDs();
        Shuffle(cardIDs);
        SpawnCards();
    }

    void GenerateCardIDs()
    {
        int pairCount = (rows * columns) / 2;

        for (int i = 0; i < pairCount; i++)
        {
            cardIDs.Add(i);
            cardIDs.Add(i);
        }
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }

    void SpawnCards()
    {
        for (int i = 0; i < cardIDs.Count; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardGridParent);
            Card card = cardObj.GetComponent<Card>();
            card.Setup(cardIDs[i], cardSprites[cardIDs[i]]);
        }
    }
}
