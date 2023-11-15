using UnityEngine;
using UnityEngine.UI;

public class Blur : MonoBehaviour
{
    private float color;
    public float targetColor;
    private float blur;
    public float targetBlur;
    private float currentAnimationTime;
    public float animationTime;
    public AnimationCurve animationCurve;
    public Image image;

    void OnEnable()
    {
        color = 1.0f;
        blur = 0;
        image.material.SetColor("_MultiplyColor", new Color(color, color, color, 1.0f));
        image.material.SetFloat("_Size", blur);
    }

    void Update()
    {
        currentAnimationTime += Time.deltaTime;
        float percentage = animationCurve.Evaluate(currentAnimationTime / animationTime);
        color = 1.0f - Mathf.Lerp(0, targetColor, percentage);
        blur = Mathf.Lerp(0, targetBlur, percentage);
        image.material.SetColor("_MultiplyColor", new Color(color, color, color, 1.0f));
        image.material.SetFloat("_Size", blur);
    }
}
