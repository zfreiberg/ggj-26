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
    public static MaskControl Inst { get; private set; }
    public float RedMaskEnemySpeedModifer = 2f;

    public static bool hasGreenMask = false;
    public static bool hasRedMask = false;
    public static bool hasBlueMask = false;

    public List<MaskControlItems> maskControlItems;

    void Awake()
    {
        Inst = this;
    }

    void Start()
    {
        foreach (var item in maskControlItems)
        {
            item.maskOnButton.onClick.AddListener(() => ToggleMaskLayer(item));
            item.maskAffectLayer.SetActive(false);
            GameControl.AllMaskButtons.Add(item.maskOnButton);
        }
    }

    public float GetEnemySpeedModifier()
    {
        return hasRedMask ? RedMaskEnemySpeedModifer : 1f;
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
        if (hasBlueMask)
            PlayerControl.Inst.SetPlayerFriction(0f);
        else
            PlayerControl.Inst.ResetPlayerFriction();
        if (!hasBlueMask && !hasRedMask && !hasGreenMask)
            PlayerSkinSwitcher.Inst.SetSkin(0);
        else if (hasRedMask && !hasGreenMask && !hasBlueMask)
            PlayerSkinSwitcher.Inst.SetSkin(1);
        else if (!hasRedMask && hasGreenMask && !hasBlueMask)
            PlayerSkinSwitcher.Inst.SetSkin(2);
        else if (!hasRedMask && !hasGreenMask && hasBlueMask)
            PlayerSkinSwitcher.Inst.SetSkin(3);
        else if (hasRedMask && !hasGreenMask && hasBlueMask)
            PlayerSkinSwitcher.Inst.SetSkin(4);
        else if (!hasRedMask && hasGreenMask && hasBlueMask)
            PlayerSkinSwitcher.Inst.SetSkin(5);
        else if (hasRedMask && hasGreenMask && !hasBlueMask)
            PlayerSkinSwitcher.Inst.SetSkin(6);
        else if (hasRedMask && hasGreenMask && hasBlueMask)
            PlayerSkinSwitcher.Inst.SetSkin(7);
    }
}
