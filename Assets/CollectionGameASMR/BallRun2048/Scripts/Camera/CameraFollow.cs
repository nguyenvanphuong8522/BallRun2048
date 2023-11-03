using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //[HideInInspector]
    public GameObject player;

    private Vector3 offset;
    private Vector3 targetPos;

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        targetPos = player.transform.position + offset;
        if (player.transform.position.x < 3.5f && player.transform.position.x > -3.5f)
        {
            targetPos = new Vector3(Mathf.Lerp(transform.position.x, 0, Time.deltaTime), transform.position.y, targetPos.z);
        }
        else
        {
            targetPos = new Vector3(Mathf.Lerp(transform.position.x, targetPos.x, Time.deltaTime), transform.position.y, targetPos.z);
            Mathf.Clamp(targetPos.x, -4, 4);
        }
        transform.position = targetPos;
    }
}
