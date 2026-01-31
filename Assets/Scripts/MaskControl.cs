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

[System.Serializable]
public class MaskControlItems
{
    public MaskType maskType;
    public GameObject maskAffectLayer;
    public Button maskOnButton;
}

public class MaskControl : MonoBehaviour
{
    public static bool hasGreenMask = false;
    public static bool hasRedMask = false;
    public static bool hasBlueMask = false;

    public List<MaskControlItems> maskControlItems;

    void Start()
    {
        foreach (var item in maskControlItems)
        {
            item.maskOnButton.onClick.AddListener(() => ToggleMaskLayer(item));
            item.maskAffectLayer.SetActive(false);
            GameControl.AllMaskButtons.Add(item.maskOnButton);
        }
    }

    void ToggleMaskLayer(MaskControlItems item)
    {
        item.maskAffectLayer.SetActive(!item.maskAffectLayer.activeSelf);
        UpdateButton(item);
        ApplyMaskEffect(item);
    }

    void UpdateButton(MaskControlItems item)
    {
        var buttonText = item.maskOnButton.GetComponentInChildren<TMP_Text>();
        if (item.maskAffectLayer.activeSelf)
            buttonText.text = "Mask\nOn";
        else
            buttonText.text = "Mask\nOff";
    }

    void ApplyMaskEffect(MaskControlItems item)
    {
        var maskType = item.maskType;
        switch (maskType)
        {
            case MaskType.Red:
                hasRedMask = item.maskAffectLayer.activeSelf;
                break;
            case MaskType.Green:
                hasGreenMask = item.maskAffectLayer.activeSelf;
                break;
            case MaskType.Blue:
                hasBlueMask = item.maskAffectLayer.activeSelf;
                break;
        }
        GameControl.SwapColorBlindMode(hasRedMask, hasGreenMask, hasBlueMask);
        foreach (var enemy in EnemyControl.AllEnemies)
        {
            enemy.BecomeLarger(hasGreenMask);
        }
    }
}
