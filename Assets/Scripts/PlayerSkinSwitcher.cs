using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkinSwitcher : MonoBehaviour
{
    public static PlayerSkinSwitcher Inst { get; private set; }
    [SerializeField] private Animator targetAnimator;
    [SerializeField] private RuntimeAnimatorController[] skins = new RuntimeAnimatorController[8];

    int current = -1;

    void Awake()
    {
        Inst = this;
        if (!targetAnimator)
            targetAnimator = GetComponentInChildren<Animator>(true);
    }

    void Start()
    {
        GameControl.SwapColorBlindMode(false, false, false);
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // if (Keyboard.current.digit1Key.wasPressedThisFrame) { SetSkin(0); GameControl.SwapColorBlindMode(false, false, false); }
        // if (Keyboard.current.digit2Key.wasPressedThisFrame) { SetSkin(1); GameControl.SwapColorBlindMode(true, false, false); }
        // if (Keyboard.current.digit3Key.wasPressedThisFrame) { SetSkin(2); GameControl.SwapColorBlindMode(false, true, false); }
        // if (Keyboard.current.digit4Key.wasPressedThisFrame) { SetSkin(3); GameControl.SwapColorBlindMode(false, false, true); }
        // if (Keyboard.current.digit5Key.wasPressedThisFrame) { SetSkin(4); GameControl.SwapColorBlindMode(true, true, false); }
        // if (Keyboard.current.digit6Key.wasPressedThisFrame) { SetSkin(5); GameControl.SwapColorBlindMode(true, false, true); }
        // if (Keyboard.current.digit7Key.wasPressedThisFrame) { SetSkin(6); GameControl.SwapColorBlindMode(false, true, true); }
        // if (Keyboard.current.digit8Key.wasPressedThisFrame) { SetSkin(7); GameControl.SwapColorBlindMode(true, true, true); }
    }

    public void SetSkin(int index)
    {
        if (targetAnimator == null) return;
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
