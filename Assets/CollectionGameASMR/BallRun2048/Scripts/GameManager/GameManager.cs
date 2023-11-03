using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public SceneFlow sceneFlow;
    public GameObject[] winGrades;
    public GameObject target1;
    private void Awake()
    {
        Instance = this;
    }

    public CameraFollow CameraFollow;

    public void ResetLevel()
    {
        sceneFlow.ResetLevel();
    }
}
