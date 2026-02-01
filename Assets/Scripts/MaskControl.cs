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
    public float BlueMaskPlayerVDamping = 0;
    public PhysicsMaterial2D playerOriginalPhysicsMaterial;
    public PhysicsMaterial2D blueMaskPhysicsMaterial;

    public static bool hasGreenMask = false;
    public static bool hasRedMask = false;
    public static bool hasBlueMask = false;

    public List<MaskControlItems> maskControlItems;

    private float playerOriginalFriction;
    private float playerOriginalDamping;

    void Awake()
    {
        Inst = this;
    }

    void Start()
    {
        if (playerOriginalPhysicsMaterial != null)
            playerOriginalFriction = playerOriginalPhysicsMaterial.friction;
        playerOriginalDamping = PlayerControl.Inst.GetPlayerVerticalDamping();
        foreach (var item in maskControlItems)
        {
            item.maskOnButton.onClick.AddListener(() => ToggleMaskLayer(item));
            item.maskAffectLayer.SetActive(false);
            GameControl.AllMaskButtons.Add(item.maskOnButton);
            UpdateButton(item, false);
        }
    }

    public float GetEnemySpeedModifier()
    {
        return hasRedMask ? RedMaskEnemySpeedModifer : 1f;
    }

    public void GetAllMaskControlItems(out List<MaskControlItems> items)
    {
        items = maskControlItems;
    }

    public void TurnOffAllMasks()
    {
        foreach (var item in maskControlItems)
        {
            TurnOffMaskLayer(item);
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
        if (isCurrentMaskActive)
            GameControl.Inst.PlayAudioMaskOn();
        item.maskAffectLayer.SetActive(isCurrentMaskActive);
        UpdateButton(item, isCurrentMaskActive);
        ApplyMaskEffect();
    }

    void TurnOffMaskLayer(MaskControlItems item)
    {
        switch (item.maskType)
        {
            case MaskType.Red:
                hasRedMask = false;
                break;
            case MaskType.Green:
                hasGreenMask = false;
                break;
            case MaskType.Blue:
                hasBlueMask = false;
                break;
        }
        item.maskAffectLayer.SetActive(false);
        UpdateButton(item, false);
        ApplyMaskEffect();
    }

    void UpdateButton(MaskControlItems item, bool isCurrentMaskActive)
    {
        var buttonText = item.maskOnButton.GetComponentInChildren<TMP_Text>();
        if (isCurrentMaskActive)
            buttonText.text = "On";
        else
            buttonText.text = "Off";
    }

    public void ApplyMaskEffect()
    {
        GameControl.SwapColorBlindMode(hasRedMask, hasGreenMask, hasBlueMask);
        foreach (var enemy in EnemyControl.AllEnemies)
        {
            if (enemy != null)
                enemy.BecomeLarger(hasGreenMask);
        }
        if ( PlayerControl.Inst != null)
        {
            if (hasBlueMask)
            {
                PlayerControl.Inst.SetPlayerFriction(BlueMaskPlayerVDamping);
                PlayerControl.Inst.SetPlayerPhysicsMaterial(blueMaskPhysicsMaterial);
            }
            else
            {
                PlayerControl.Inst.SetPlayerFriction(playerOriginalDamping);
                PlayerControl.Inst.SetPlayerPhysicsMaterial(playerOriginalPhysicsMaterial);
            }
        }
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
