using UnityEngine;

public class PauseManager : MonoBehaviour
{

    [SerializeField, Space(10)] Menu _pausePanel;
    [SerializeField] Menu[] menus;    

    //private AudioListener pauseAudioListener;
    
    void Update()
    {
        // Menuボタンでポーズ
        if (!Pauser.isCanNotPausing && Input.GetButtonDown("Pause")) {
            if (Pauser.isPaused) {
                Resume();
            } else {
                _pausePanel.Open();
                Pauser.Pause();
                SoundManager.Instance.PauseBGM();
                SoundManager.Instance.PauseSE();
                //pauseAudioListener.enabled = true;
            }
        }
    }

    public void Resume()
    {
        //pauseAudioListener.enabled = false;

        for (int i = 0; i < menus.Length; i++) {
            CloseMenu(menus[i]);
        }
        Pauser.Resume();
        SoundManager.Instance.UnPauseBGM();
        SoundManager.Instance.UnPauseSE();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
}