using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlow : MonoBehaviour
{
    public string currentScene;
    public int index = 1;
    public void ResetLevel()
    {
        SceneManager.LoadScene(currentScene);
    }
    public void NextLevel()
    {
        Invoke(nameof(DelayLoadScene), 1);
    }
    public void DelayLoadScene()
    {
        index++;
    }
}
