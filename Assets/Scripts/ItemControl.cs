using UnityEngine;
using UnityEngine.UI;

public class ItemControl : MonoBehaviour
{
    public Button maskButton;

    void Start()
    {
        // disable mask button at start
        maskButton.interactable = false;
    }

    // when item is collected, enable mask button
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            maskButton.interactable = true;
            Destroy(gameObject); // remove item from scene
        }
    }
}
