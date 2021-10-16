using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUIScripts : MonoBehaviour
{
    private bool isButtonPush;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ChangeMainGameScene()
    {
        if (isButtonPush) return;

        isButtonPush = true;
        Pauser.Resume();
        SceneManager.LoadScene(1);
    }

    public void ChangeTitleScene()
    {
        if (isButtonPush) return;

        isButtonPush = true;
        Pauser.Resume();
        SceneManager.LoadScene(0);
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
