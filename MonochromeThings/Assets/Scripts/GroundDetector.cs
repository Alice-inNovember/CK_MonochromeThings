using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GroundDetector : MonoBehaviour
{
    [SerializeField]
    public LayerMask targetLayer;
    private bool isDetected;
    Ray ray = new Ray();

    [SerializeField]
    private float distance = 2;

    private void Start()
    {
        CapsuleCollider col;
        if (TryGetComponent<CapsuleCollider>(out col))
        {
            distance = col.height/2;
        }


    }


    public bool DetectGround()
    {
         
        ray.origin = transform.position;
        ray.direction = -transform.up;

        if (Physics.Raycast(ray, distance + 0.1f, targetLayer.value))
        {
            isDetected = true;
        }
        else
        {
            isDetected = false;
        }

        return isDetected;

    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, -transform.up *(distance+ 0.1f));
    }


}
