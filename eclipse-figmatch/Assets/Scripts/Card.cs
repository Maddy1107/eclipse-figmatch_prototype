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
        FlipBack();
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
        AudioManager.Instance.PlayFlip();
    }

    public void FlipBack()
    {
        if (flipRoutine != null) StopCoroutine(flipRoutine);
        flipRoutine = StartCoroutine(FlipAnimation(false));
        IsFlipped = false;
    }

    public void SetMatched()
    {
        if (flipRoutine != null) StopCoroutine(flipRoutine);
        StartCoroutine(HideAfterMatched());
        IsMatched = true;
    }

    private IEnumerator HideAfterMatched()
    {
        float duration = 0.2f;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;

        float t = 0;
        while (t < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        transform.localScale = endScale;

        CanvasGroup group = GetComponent<CanvasGroup>();
        if (group != null)
        {
            group.alpha = 0f;
            group.interactable = false;
            group.blocksRaycasts = false;
        }
    }

    public IEnumerator Shake()
    {
        float duration = 0.5f;
        float magnitude = 0.1f;

        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float xOffset = Random.Range(-magnitude, magnitude);
            float yOffset = Random.Range(-magnitude, magnitude);
            transform.localPosition = originalPosition + new Vector3(xOffset, yOffset, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }

    private IEnumerator FlipAnimation(bool showFront)
    {

        if (!showFront)
            StartCoroutine(Shake());

        yield return new WaitForSeconds(0.1f);

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
