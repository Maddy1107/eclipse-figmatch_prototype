using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour, IScorable
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text turnText;

    [SerializeField] private int totalPairs = 2;

    private ICard firstCard;
    private ICard secondCard;
    private int matchedPairs = 0;
    private bool isBusy = false;
    private int score = 0;
    private int turns = 0;

    public bool IsBusy => isBusy;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        ResetScore();
        ResetTurns();
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

            if (matchedPairs >= totalPairs)
            {
                Debug.Log("🎉 Game Over!");
            }
        }
        else
        {
            firstCard.FlipBack();
            secondCard.FlipBack();
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

}
