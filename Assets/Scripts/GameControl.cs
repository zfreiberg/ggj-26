using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameControl : MonoBehaviour
{
    public static GameControl Inst { get; private set; }

    public GameObject CanvasUI;
    public Button resetButton;
    public static List<Button> AllMaskButtons { get; private set; }
    public static List<ColorModeControl> AllColorSwappingItems { get; private set; }

    public AudioSource maskOnAudioSource;
    public AudioSource pickupMaskAudioSource;
    public AudioSource deathAudioSource;
    public AudioSource enemyShootingAudioSource;
    public AudioSource enemyFlyingAudioSource;
    public AudioSource enemyWalkingAudioSource;

    public void PlayAudioMaskOn() => maskOnAudioSource.Play();
    public void PlayAudioPickupMask() => pickupMaskAudioSource.Play();
    public void PlayAudioDeath() => deathAudioSource.Play();
    public void PlayAudioEnemyShooting() => enemyShootingAudioSource.Play();

    void Awake()
    {
        Inst = this;
        AllMaskButtons = new List<Button>();
        AllColorSwappingItems = new List<ColorModeControl>();
        CanvasUI.SetActive(true);
    }

    void Start()
    {
        InitUI();
    }

    public static void SwapColorBlindMode(bool isR, bool isG, bool isB)
    {
        foreach (var item in AllColorSwappingItems)
        {
            item.SetColorBlindType(isR, isG, isB);
        }
    }

    public static GameObject CreateBullet(GameObject bulletPrefab, Vector3 pos, Vector2 dir, float spd)
    {
        GameObject bullet = Instantiate(bulletPrefab);
        EnemyBulletControl ebc = bullet.GetComponent<EnemyBulletControl>();
        ebc.Setup(pos, dir, spd);
        return bullet;
    }
    
    public void ReloadCurrentScene()
    {
        MaskControl.Inst.TurnOffAllMasks();

        // hide all masks buttons`
        foreach( var btn in AllMaskButtons)
            btn.gameObject.SetActive(false);
        // reset time scale
        Time.timeScale = 1f;
        GameControl.SwapColorBlindMode(false, false, false);

        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    void InitUI()
    {
        resetButton.onClick.AddListener(ReloadCurrentScene);
        // show all masks buttons as off at start
        foreach( var btn in AllMaskButtons)
            btn.gameObject.SetActive(true);
    }

    public void OnGameOver()
    {
        // hide all masks buttons`
        foreach( var btn in AllMaskButtons)
            btn.gameObject.SetActive(false);
        // reset time scale
        Time.timeScale = 1f;
        GameControl.SwapColorBlindMode(false, false, false);

        StartCoroutine(GameControl.Inst.OnGameOverCo());
    }

    public IEnumerator OnGameOverCo()
    {
        yield return new WaitForSeconds(2f);
        ReloadCurrentScene();
    }
}
