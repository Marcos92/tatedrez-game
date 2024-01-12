using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TileRenderer : MonoBehaviour
{
    [Header("Tile")]
    [SerializeField] private Image tileImage;
    [SerializeField] private Color lightColor;
    [SerializeField] private Color darkColor;

    [Header("Highlight")]
    [SerializeField] private Image hoverImage;
    [SerializeField] private Image validMoveImage;
    [SerializeField] private float highlightFadeInSpeed;

    public void SetColor(bool value)
    {
        tileImage.color = value ? lightColor : darkColor;
    }

    private IEnumerator FadeIn(Image image)
    {
        while (image.color.a < 0.5f)
        {
            float alpha = image.color.a;
            alpha += highlightFadeInSpeed * Time.deltaTime;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return null;
        }
    }

    public void FadeInHover()
    {
        StartCoroutine(FadeIn(hoverImage));
    }

    public void FadeInValidMove()
    {
        StartCoroutine(FadeIn(validMoveImage));
    }

    private void FadeOut(Image image)
    {
        StopAllCoroutines();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);
    }

    public void FadeOutHover()
    {
        FadeOut(hoverImage);
    }

    public void FadeOutValidMove()
    {
        FadeOut(validMoveImage);
    }
}
