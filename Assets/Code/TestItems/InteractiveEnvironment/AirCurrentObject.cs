using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirCurrentObject : MonoBehaviour
{
    private HashSet<Rigidbody> liftingObjects = new HashSet<Rigidbody>();

    void OnTriggerEnter(Collider other)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null && !liftingObjects.Contains(rb))
            {
                liftingObjects.Add(rb);
                StartCoroutine(LiftUp(rb));
            }
        }

    void OnTriggerExit(Collider other)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null && liftingObjects.Contains(rb)) liftingObjects.Remove(rb);
        }

    IEnumerator LiftUp(Rigidbody rb)
        {
            while (rb != null && liftingObjects.Contains(rb))
            {
                Vector3 velocity = rb.velocity;
                velocity.y = 10f;
                rb.velocity = velocity;
                yield return null;
            }
        }
}
