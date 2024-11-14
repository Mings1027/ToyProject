using UnityEngine;

public class SortingLayer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            spriteRenderer.sortingOrder = 1;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            spriteRenderer.sortingOrder = 0;
        }
    }
}
