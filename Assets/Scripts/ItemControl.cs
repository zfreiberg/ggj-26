using UnityEngine;
using UnityEngine.UI;

public class ItemControl : MonoBehaviour
{
    public Button maskButton;
    public MaskType maskType;

    public static ItemControl Inst { get; private set; }

    void Awake()
    {
        Inst = this;
    }

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
            GameControl.Inst.PlayAudioPickupMask();
            maskButton.interactable = true;
            Destroy(gameObject); // remove item from scene

            switch (maskType)
            {
                case MaskType.Red:
                    GameControl.HasGotRedMask = true;
                    break;
                case MaskType.Green:
                    GameControl.HasGotGreenMask = true;
                    break;
                case MaskType.Blue:
                    GameControl.HasGotBlueMask = true;
                    break;
            }
        }
    }
}
