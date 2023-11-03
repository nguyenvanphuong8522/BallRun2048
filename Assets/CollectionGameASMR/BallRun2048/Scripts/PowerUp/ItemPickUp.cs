using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public int id;
    [SerializeField] public Rigidbody rb;
    [SerializeField] public float force;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            rb.AddForce((collision.transform.position - transform.position).normalized * -force, ForceMode.Impulse);
        }
        else if(collision.gameObject.CompareTag("Trap"))
        {

        }
    }
}
