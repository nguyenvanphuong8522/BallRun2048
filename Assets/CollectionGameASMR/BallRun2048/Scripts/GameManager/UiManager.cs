using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    private void Awake()
    {
        instance = this;
    }
    public GameObject uiWin;


    public void SetUiWin()
    {
        uiWin.SetActive(true);
    }
}
