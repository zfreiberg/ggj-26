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
    public GameObject maskBGLayer;
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
            item.maskBGLayer.SetActive(false);
            GameControl.AllMaskButtons.Add(item.maskOnButton);
        }
    }

    void ToggleMaskLayer(MaskControlItems item)
    {
        var isCurrentMaskActive = false;
        switch (item.maskType)
        {
            case MaskType.Red:
                hasRedMask = !hasRedMask;
                isCurrentMaskActive = hasRedMask;
                break;
            case MaskType.Green:
                hasGreenMask = !hasGreenMask;
                isCurrentMaskActive = hasGreenMask;
                break;
            case MaskType.Blue:
                hasBlueMask = !hasBlueMask;
                isCurrentMaskActive = hasBlueMask;
                break;
        }
        item.maskAffectLayer.SetActive(isCurrentMaskActive);
        item.maskBGLayer.SetActive(isCurrentMaskActive);
        UpdateButton(item, isCurrentMaskActive);
        ApplyMaskEffect(item, isCurrentMaskActive);
    }

    void UpdateButton(MaskControlItems item, bool isCurrentMaskActive)
    {
        var buttonText = item.maskOnButton.GetComponentInChildren<TMP_Text>();
        if (isCurrentMaskActive)
            buttonText.text = "Mask\nOn";
        else
            buttonText.text = "Mask\nOff";
    }

    void ApplyMaskEffect(MaskControlItems item, bool isCurrentMaskActive)
    {
        GameControl.SwapColorBlindMode(hasRedMask, hasGreenMask, hasBlueMask);
        foreach (var enemy in EnemyControl.AllEnemies)
        {
            enemy.BecomeLarger(hasGreenMask);
        }
    }
}
