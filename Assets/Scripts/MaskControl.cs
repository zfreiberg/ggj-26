using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MaskControl : MonoBehaviour
{
    public GameObject maskAffectLayer;
    public Button maskOnButton;

    void Start()
    {
        maskOnButton.onClick.AddListener(ToggleMaskLayer);
        maskAffectLayer.SetActive(false);
    }

    void ToggleMaskLayer()
    {
        maskAffectLayer.SetActive(!maskAffectLayer.activeSelf);
        UpdateButton();
    }

    void UpdateButton()
    {
        var buttonText = maskOnButton.GetComponentInChildren<TMP_Text>();
        if (maskAffectLayer.activeSelf)
            buttonText.text = "Mask\nOn";
        else
            buttonText.text = "Mask\nOff";
    }
}
