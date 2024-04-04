using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }
    public GameObject pauseMenuPanel;
    public GameObject WinUI;
    public GameObject FoodDrop;
    bool Isdead;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        Isdead = false;
        pauseMenuPanel.SetActive(false);
        WinUI.SetActive(false);
        FoodDrop.SetActive(false);
    }
    void Update()
    {
        if (!Isdead)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePauseMenu();
            }
        }
        
    }

    // 显示或隐藏暂停菜单
    void TogglePauseMenu()
    {
        pauseMenuPanel.SetActive(!pauseMenuPanel.activeSelf);
        Time.timeScale = pauseMenuPanel.activeSelf ? 0 : 1; 
    }

    // 关联到"重来"按钮
    public void RestartLevel()
    {
        // 重新加载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1; 
    }

    // 关联到"返回"按钮
    public void ReturnToMainMenu()
    {
        // 加载主菜单场景
        SceneManager.LoadScene("CHOOSE"); 
        Time.timeScale = 1; 
    }
    public void NextButton()
    {      
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        Time.timeScale = 1;
    }
    public void Dead()
    {
        Isdead = true;
        //Time.timeScale = 0;
        pauseMenuPanel.SetActive(true);
    }
    public void Win()
    {
        Time.timeScale = 0;
        WinUI.SetActive(true);
    }
    public void OnQuitButtonClick()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public void FoodBeDrop()
    {
        FoodDrop.SetActive(true);
    }
}