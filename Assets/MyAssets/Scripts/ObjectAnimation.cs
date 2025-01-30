using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnimation : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            print(other.gameObject.name);
            foreach (Rigidbody rb in transform.parent.GetComponentsInChildren<Rigidbody>())
            {
                rb.isKinematic = false;
            }
        }
    }
}
