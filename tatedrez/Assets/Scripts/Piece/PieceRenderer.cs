using UnityEngine;

public class PieceRenderer : MonoBehaviour
{
    [SerializeField] private GameObject glow;
    [SerializeField] private GameObject shadow;

    public void ToggleGlow(bool value)
    {
        glow.SetActive(value);
    }

    public void ToggleShadow(bool value)
    {
        shadow.SetActive(value);
    }
}