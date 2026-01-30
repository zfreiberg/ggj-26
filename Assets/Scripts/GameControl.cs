using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameControl : MonoBehaviour
{
    public static GameControl Inst { get; private set; }

    public Button resetButton;
    public static List<Button> AllMaskButtons { get; private set; }

    void Awake()
    {
        Inst = this;
        AllMaskButtons = new List<Button>();
    }

    void Start()
    {
        InitUI();
    }
    
    public void ReloadCurrentScene()
    {
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
    }
}
