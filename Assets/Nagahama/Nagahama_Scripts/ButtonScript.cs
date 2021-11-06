using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    private SoundManager sm;

    private void Start()
    {
        sm = SoundManager.Instance;
    }

    public void OnClickSE()
    {
        sm.PlaySystemSE(SystemSE.Decide);
    }

    public void OnCancelSE()
    {
        //sm.PlaySystemSE(SystemSE.Decide);
    }

    public void OnSelectSE()
    {
        sm.PlaySystemSE(SystemSE.Cursor);
    }
}
