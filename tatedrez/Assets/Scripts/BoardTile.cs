using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoardTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    private Image image;
    private TMP_Text debugLabel;

    [Header("Hover")]
    public Image hoverImage;
    public float hoverFadeInSpeed;
    public float hoverFadeOutSpeed;

    private Piece piece;

    void Awake()
    {
        image = GetComponent<Image>();
        debugLabel = GetComponentInChildren<TMP_Text>();
    }

    public void SetColor(Color color)
    {
        image.color = color;
    }

    public void SetLabel(string text)
    {
        debugLabel.text = text;
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject drop = eventData.pointerDrag;
        Piece p = drop.GetComponent<Piece>();
        p.SetNewParent(transform);
        piece = p;

        StartCoroutine(nameof(HideHover));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!HasPiece())
        {
            StopCoroutine(nameof(HideHover));
            StartCoroutine(nameof(ShowHover));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine(nameof(ShowHover));
        StartCoroutine(nameof(HideHover));
    }

    private IEnumerator ShowHover()
    {
        float alpha = hoverImage.color.a;

        while (alpha < 0.75f)
        {
            alpha += hoverFadeInSpeed * Time.deltaTime;
            hoverImage.color = new Color(hoverImage.color.r, hoverImage.color.g, hoverImage.color.b, alpha);
            yield return null;
        }
    }

    private IEnumerator HideHover()
    {
        float alpha = hoverImage.color.a;

        while (alpha > 0.0f)
        {
            alpha -= hoverFadeOutSpeed * Time.deltaTime;
            hoverImage.color = new Color(hoverImage.color.r, hoverImage.color.g, hoverImage.color.b, alpha);
            yield return null;
        }
    }

    public bool HasPiece()
    {
        return piece != null;
    }

    public void RemovePiece()
    {
        piece = null;
    }

    public bool CheckMatch(BoardManager.PlayerColor color)
    {
        return HasPiece() && piece.playerColor == color;
    }
}
