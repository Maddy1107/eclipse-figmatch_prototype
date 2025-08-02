using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public int totalPairs = 2; // set this in Inspector: 2 for 2x2, 8 for 4x4

    private Card firstCard;
    private Card secondCard;
    private bool isCheckingMatch = false;
    private int matchedPairs = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void CardRevealed(Card card)
    {
        if (isCheckingMatch || card == firstCard) return;

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
        isCheckingMatch = true;

        yield return new WaitForSeconds(0.5f);

        if (firstCard.cardID == secondCard.cardID)
        {
            firstCard.SetMatched();
            secondCard.SetMatched();
            matchedPairs++;
            Debug.Log($"Matched Pairs: {matchedPairs}/{totalPairs}");

            if (matchedPairs >= totalPairs)
            {
                GameOver();
            }
        }
        else
        {
            firstCard.FlipBack();
            secondCard.FlipBack();
        }

        firstCard = null;
        secondCard = null;
        isCheckingMatch = false;
    }

    private void GameOver()
    {
        Debug.Log("🎉 Game Over! All pairs matched.");
        // Optional: trigger a "You Win!" panel or restart
    }
}
