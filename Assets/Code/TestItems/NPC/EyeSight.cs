using System;
using System.Collections;
using UnityEngine;

public class EyeSight : MonoBehaviour
{
    public Action<Transform> iSenseTarget;
    private bool targetInPOV;
    void OnTriggerEnter(Collider other)
        { 
            if(other.CompareTag("Player"))
                {
                    targetInPOV = true;
                    StartCoroutine(CheckTick(other));
                }
        }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player")) targetInPOV = false;
    }

    private IEnumerator CheckTick(Collider other)
        {
            while (targetInPOV)
            {
                if (other == null || !other.gameObject.activeInHierarchy)
                    {
                        targetInPOV = false;
                        yield break;
                    }
                
                iSenseTarget?.Invoke(other.transform);
                yield return new WaitForSeconds(0.2f);
            }
        }

}
