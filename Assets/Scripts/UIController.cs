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

    // ��ʾ��������ͣ�˵�
    void TogglePauseMenu()
    {
        pauseMenuPanel.SetActive(!pauseMenuPanel.activeSelf);
        Time.timeScale = pauseMenuPanel.activeSelf ? 0 : 1; 
    }

    // ������"����"��ť
    public void RestartLevel()
    {
        // ���¼��ص�ǰ����
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1; 
    }

    // ������"����"��ť
    public void ReturnToMainMenu()
    {
        // �������˵�����
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