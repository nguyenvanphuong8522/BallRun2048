using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(other.gameObject.GetComponentInParent<MovementPlayer>().canMove)
            {
                GameManager.Instance.ResetLevel();
            }
        }
    }
}
