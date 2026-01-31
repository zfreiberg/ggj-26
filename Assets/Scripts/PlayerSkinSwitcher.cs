using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkinSwitcher : MonoBehaviour
{
    [SerializeField] private Animator targetAnimator;
    [SerializeField] private RuntimeAnimatorController[] skins = new RuntimeAnimatorController[8];

    int current = -1;

    void Awake()
    {
        if (!targetAnimator)
            targetAnimator = GetComponentInChildren<Animator>(true);
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame) SetSkin(0);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) SetSkin(1);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) SetSkin(2);
        if (Keyboard.current.digit4Key.wasPressedThisFrame) SetSkin(3);
        if (Keyboard.current.digit5Key.wasPressedThisFrame) SetSkin(4);
        if (Keyboard.current.digit6Key.wasPressedThisFrame) SetSkin(5);
        if (Keyboard.current.digit7Key.wasPressedThisFrame) SetSkin(6);
        if (Keyboard.current.digit8Key.wasPressedThisFrame) SetSkin(7);
    }

    public void SetSkin(int index)
    {
        if (index < 0 || index >= skins.Length) return;
        if (skins[index] == null) return;
        if (current == index) return;

        var state = targetAnimator.GetCurrentAnimatorStateInfo(0);
        float t = state.normalizedTime;
        int hash = state.shortNameHash;

        targetAnimator.runtimeAnimatorController = skins[index];
        targetAnimator.Play(hash, 0, t);

        current = index;
    }
}
