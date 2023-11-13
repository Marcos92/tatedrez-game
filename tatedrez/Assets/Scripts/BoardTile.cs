using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoardTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    private Image image;

    [Header("Hover")]
    public Image hoverImage;
    public float hoverFadeInSpeed;
    public float hoverFadeOutSpeed;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetColor(Color color)
    {
        image.color = color;
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject drop = eventData.pointerDrag;
        Piece piece = drop.GetComponent<Piece>();
        piece.SetNewParent(transform);

        Debug.Log(transform.gameObject.name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopCoroutine(nameof(HideHover));
        StartCoroutine(nameof(ShowHover));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine(nameof(ShowHover));
        StartCoroutine(nameof(HideHover));
    }

    private IEnumerator ShowHover()
    {
        float alpha = 0.0f;

        while (alpha < 1.0f)
        {
            alpha += hoverFadeInSpeed * Time.deltaTime;
            hoverImage.color = new Color(hoverImage.color.r, hoverImage.color.g, hoverImage.color.b, alpha);
            yield return null;
        }
    }

    private IEnumerator HideHover()
    {
        float alpha = 1.0f;

        while (alpha > 0.0f)
        {
            alpha -= hoverFadeOutSpeed * Time.deltaTime;
            hoverImage.color = new Color(hoverImage.color.r, hoverImage.color.g, hoverImage.color.b, alpha);
            yield return null;
        }
    }
}
