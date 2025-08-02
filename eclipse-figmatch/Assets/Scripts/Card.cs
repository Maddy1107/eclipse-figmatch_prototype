using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private GameObject front;
    [SerializeField] private GameObject back;

    private bool isFlipped = false;

    private void OnMouseDown()
    {
        if (isFlipped) return;

        Flip();
    }

    public void Flip()
    {
        isFlipped = true;
        front.SetActive(true);
        back.SetActive(false);
    }

    public void FlipBack()
    {
        isFlipped = false;
        front.SetActive(false);
        back.SetActive(true);
    }
}
