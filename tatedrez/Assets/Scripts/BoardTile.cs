using System.Collections;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("Hover")]
    public SpriteRenderer hoverSprite;
    public float hoverFadeInSpeed;
    public float hoverFadeOutSpeed;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    void OnMouseEnter()
    {
        StopCoroutine(nameof(HideHover));
        StartCoroutine(nameof(ShowHover));
    }

    void OnMouseExit()
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
            hoverSprite.color = new Color(hoverSprite.color.r, hoverSprite.color.g, hoverSprite.color.b, alpha);
            yield return null;
        }
    }

    private IEnumerator HideHover()
    {
        float alpha = 1.0f;

        while (alpha > 0.0f)
        {
            alpha -= hoverFadeOutSpeed * Time.deltaTime;
            hoverSprite.color = new Color(hoverSprite.color.r, hoverSprite.color.g, hoverSprite.color.b, alpha);
            yield return null;
        }
    }
}
