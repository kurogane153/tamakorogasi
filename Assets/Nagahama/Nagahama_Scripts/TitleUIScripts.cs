using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUIScripts : MonoBehaviour
{
    [SerializeField] private int _gotoSceneIndex;

    private bool isButtonPush;

    public void ChangeMainGameScene()
    {
        if (isButtonPush) return;

        isButtonPush = true;
        SceneManager.LoadScene(_gotoSceneIndex);
    }

    private void Update()
    {
        if (Input.anyKey) {
            SoundManager.Instance.PlaySystemSE(SystemSE.Decide);
            SceneManager.LoadScene(_gotoSceneIndex);
        }
                
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
