using UnityEngine;

public class PieceMovement : MonoBehaviour
{
    private Transform oldParent;

    public void StartMove()
    {
        oldParent = transform.parent;

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void MoveTo(Vector2 target)
    {
        transform.position = target;
    }

    public void EndMove()
    {
        PlaceOnParent();
    }

    public void Drop(Transform transform)
    {
        oldParent = transform;
        PlaceOnParent();
    }

    private void PlaceOnParent()
    {
        transform.SetParent(oldParent);
        transform.localPosition = Vector2.zero;
    }
}