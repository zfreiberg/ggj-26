using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public enum MaskType
{
    Red,
    Green,
    Blue
}

public class MaskControl : MonoBehaviour
{
    public static bool hasGreenMask = false;
    public static bool hasRedMask = false;
    public static bool hasBlueMask = false;

    public GameObject maskAffectLayer;
    public Button maskOnButton;
    public MaskType maskType;

    void Awake()
    {
    }

    void Start()
    {
        maskOnButton.onClick.AddListener(ToggleMaskLayer);
        maskAffectLayer.SetActive(false);

        GameControl.AllMaskButtons.Add(maskOnButton);
    }

    void ToggleMaskLayer()
    {
        maskAffectLayer.SetActive(!maskAffectLayer.activeSelf);
        UpdateButton();
        ApplyMaskEffect();
    }

    void UpdateButton()
    {
        var buttonText = maskOnButton.GetComponentInChildren<TMP_Text>();
        if (maskAffectLayer.activeSelf)
            buttonText.text = "Mask\nOn";
        else
            buttonText.text = "Mask\nOff";
    }

    void ApplyMaskEffect()
    {
        switch (maskType)
        {
            case MaskType.Red:
                hasRedMask = maskAffectLayer.activeSelf;
                break;
            case MaskType.Green:
                hasGreenMask = maskAffectLayer.activeSelf;
                break;
            case MaskType.Blue:
                hasBlueMask = maskAffectLayer.activeSelf;
                break;
        }
        GameControl.SwapColorBlindMode(hasRedMask, hasGreenMask, hasBlueMask);
        foreach (var enemy in EnemyControl.AllEnemies)
        {
            enemy.BecomeLarger(hasGreenMask);
        }
    }
}
