using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : GameBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up*100);
        }
    }
}
