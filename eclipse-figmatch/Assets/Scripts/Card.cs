using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Card : MonoBehaviour, ICard
{
    [Header("Card Elements")]
    [SerializeField] private Image frontImage;
    [SerializeField] private GameObject frontObject;
    [SerializeField] private GameObject backObject;

    public int CardID { get; private set; }
    public bool IsFlipped { get; private set; } = false;
    public bool IsMatched { get; private set; } = false;

    private Button button;
    private Coroutine flipRoutine;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void Setup(int id, Sprite frontSprite)
    {
        CardID = id;
        frontImage.sprite = frontSprite;
        FlipBack(); // Always start face down
    }

    private void OnClick()
    {
        if (IsFlipped || IsMatched || GameManager.Instance.IsBusy) return;

        Flip();
        GameManager.Instance.OnCardSelected(this);
    }

    public void Flip()
    {
        if (flipRoutine != null) StopCoroutine(flipRoutine);
        flipRoutine = StartCoroutine(FlipAnimation(true));
        IsFlipped = true;
    }

    public void FlipBack()
    {
        if (flipRoutine != null) StopCoroutine(flipRoutine);
        flipRoutine = StartCoroutine(FlipAnimation(false));
        IsFlipped = false;
    }

    public void SetMatched()
    {
        IsMatched = true;
    }

    private IEnumerator FlipAnimation(bool showFront)
    {
        float duration = 0.15f;
        Vector3 startScale = transform.localScale;
        Vector3 midScale = new Vector3(0f, 1f, 1f);

        float t = 0;
        while (t < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, midScale, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        transform.localScale = midScale;

        frontObject.SetActive(showFront);
        backObject.SetActive(!showFront);

        t = 0;
        while (t < duration)
        {
            transform.localScale = Vector3.Lerp(midScale, Vector3.one, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.one;
    }
}
