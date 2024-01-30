using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GroundDetector : MonoBehaviour
{
    [SerializeField]
    private bool _isDetected;

    Ray ray = new Ray();

    public bool IsDeteted
    {
        get { return _isDetected; }   
    }

    [SerializeField]
    private float distance = 2;

    private void Start()
    {
        CapsuleCollider col;
        if (TryGetComponent<CapsuleCollider>(out col))
        {
            distance = col.height;
        }


    }


    private void Update()
    {
        ray.origin = transform.position;
        ray.direction = transform.position + (Vector3.down*3f);
        
        
        
        if (Physics.Raycast(ray, distance+0.2f))
        {
            _isDetected = true;
        }
        else
        {
            _isDetected = false;
        }

     
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down*(distance+ 0.1f)));
    }


}
