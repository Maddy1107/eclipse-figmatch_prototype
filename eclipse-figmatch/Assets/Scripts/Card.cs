using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int cardID;

    [SerializeField] private GameObject frontImage;
    [SerializeField] private GameObject backImage;

    private bool isFlipped = false;
    private bool isMatched = false;

    public void Setup(int id, Sprite frontSprite)
    {
        cardID = id;
        frontImage.GetComponent<Image>().sprite = frontSprite;
        FlipBack(); // start face down
    }

    public void OnClick()
    {
        if (isFlipped || isMatched) return;

        Flip();
        //GameManager.Instance.CardRevealed(this);
    }

    public void Flip()
    {
        isFlipped = true;
        frontImage.SetActive(true);
        backImage.SetActive(false);
    }

    public void FlipBack()
    {
        isFlipped = false;
        frontImage.SetActive(false);
        backImage.SetActive(true);
    }

    public void SetMatched()
    {
        isMatched = true;
    }
}
