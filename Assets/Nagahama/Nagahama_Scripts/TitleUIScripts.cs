using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUIScripts : MonoBehaviour
{
    private bool isButtonPush;


    public void ChangeMainGameScene()
    {
        if (isButtonPush) return;

        isButtonPush = true;
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        if (isButtonPush) return;

        isButtonPush = true;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#elif UNITY_STANDALONE
        UnityEngine.Application.Quit();
#endif
    }
}
