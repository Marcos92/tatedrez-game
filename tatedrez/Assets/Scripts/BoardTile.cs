using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoardTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    private Image tileImage;
    private TMP_Text debugLabel;

    [Header("Highlight")]
    public Image hoverImage;
    public Image validMoveImage;
    public float highlightFadeInSpeed;

    private Piece piece;

    private bool interactable;

    void Awake()
    {
        tileImage = GetComponent<Image>();
        debugLabel = GetComponentInChildren<TMP_Text>();

        interactable = true;
    }

    public void SetColor(Color color)
    {
        tileImage.color = color;
    }

    public void SetLabel(string text)
    {
        if (debugLabel)
        {
            debugLabel.text = text;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!interactable)
            return;

        if (!HasPiece())
        {
            GameObject drop = eventData.pointerDrag;
            Piece p = drop.GetComponent<Piece>();
            p.SetNewParent(transform);
            piece = p;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!interactable)
            return;

        if (!HasPiece())
        {
            StartCoroutine(FadeInHighlight(hoverImage));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        FadeOutHighlight(hoverImage);
    }

    private IEnumerator FadeInHighlight(Image image)
    {
        while (image.color.a < 0.5f)
        {
            float alpha = image.color.a;
            alpha += highlightFadeInSpeed * Time.deltaTime;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return null;
        }
    }

    private void FadeOutHighlight(Image image)
    {
        StopAllCoroutines();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);
    }

    public bool HasPiece()
    {
        return piece != null;
    }

    public Piece GetPiece()
    {
        return piece;
    }

    public void RemovePiece()
    {
        piece = null;
    }

    public bool CheckMatch(BoardManager.PlayerColor color)
    {
        return HasPiece() && piece.playerColor == color;
    }

    public void ShowValidMove()
    {
        StartCoroutine(FadeInHighlight(validMoveImage));
    }

    public void HideValidMove()
    {
        FadeOutHighlight(validMoveImage);
    }

    public void ToggleInteraction(bool value)
    {
        interactable = value;
    }

    public void FadeOutAllHighlights()
    {
        FadeOutHighlight(hoverImage);
        FadeOutHighlight(validMoveImage);
    }
}
