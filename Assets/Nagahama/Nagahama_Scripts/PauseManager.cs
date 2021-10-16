using UnityEngine;

public class PauseManager : MonoBehaviour
{

    [SerializeField, Space(10)] Menu _pausePanel;
    [SerializeField] Menu[] menus;    

    //private AudioListener pauseAudioListener;

    void Start()
    {
        

    }

    
    void Update()
    {
        // Menuボタンでポーズ
        if (!Pauser.isCanNotPausing && Input.GetButtonDown("Pause")) {
            if (Pauser.isPaused) {
                Resume();
            } else {
                _pausePanel.Open();
                Pauser.Pause();
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
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
}