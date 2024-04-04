using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public CanvasGroup imageCanvasGroup; 
    public CanvasGroup buttonsCanvasGroup; 
    public CanvasGroup buttonsCanvasGroup2;
    public float imageFadeDuration = 2.0f; // 图片淡入的时间
    public float buttonsDelay = 3.0f; // 按钮延迟显示的时间

    private void Start()
    {
        imageCanvasGroup.alpha = 0; 
        buttonsCanvasGroup.alpha = 0;
        buttonsCanvasGroup2.alpha = 0;
        buttonsCanvasGroup.interactable = false;
        buttonsCanvasGroup2.interactable = false;
        StartCoroutine(FadeInImage());
    }

    private IEnumerator FadeInImage()
    {
        float elapsedTime = 0f;
        while (elapsedTime < imageFadeDuration)
        {
            imageCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / imageFadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        imageCanvasGroup.alpha = 1; 
        yield return new WaitForSeconds(buttonsDelay);
        buttonsCanvasGroup.alpha = 1;
        buttonsCanvasGroup2.alpha = 1;
        buttonsCanvasGroup.interactable = true;
        buttonsCanvasGroup2.interactable = true;
    }

    public void OnBeginButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}