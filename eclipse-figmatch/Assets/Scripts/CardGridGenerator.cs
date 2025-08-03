using System;
using System.Collections.Generic;

public class CardGridGenerator
{
    public int Rows { get; }
    public int Columns { get; }
    public List<int> CardIDs { get; private set; }

    public CardGridGenerator(int rows, int columns)
    {
        if (rows * columns % 2 != 0)
            throw new ArgumentException("Grid must have an even number of cards.");

        Rows = rows;
        Columns = columns;

        GenerateCardIDs();
        Shuffle(CardIDs);
    }

    private void GenerateCardIDs()
    {
        CardIDs = new List<int>();
        int pairCount = Rows * Columns / 2;

        for (int i = 0; i < pairCount; i++)
        {
            CardIDs.Add(i);
            CardIDs.Add(i);
        }
    }

    private void Shuffle(List<int> list)
    {
        Random rng = new();
        int n = list.Count;

        while (n > 1)
        {
            int k = rng.Next(n--);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}
