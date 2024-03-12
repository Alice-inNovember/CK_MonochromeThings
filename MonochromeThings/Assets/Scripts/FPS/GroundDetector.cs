using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class GroundDetector : MonoBehaviour
    {
        [SerializeField]
        public LayerMask targetLayer;
        Ray ray = new Ray();

        [SerializeField]
        private float distance = 2;

        private void Start()
        {
            CapsuleCollider col;
            if (TryGetComponent<CapsuleCollider>(out col))
            {
                distance = col.height / 2;
            }


        }


        public bool DetectGround()
        {

            ray.origin = transform.position;
            ray.direction = -transform.up;

            if (Physics.Raycast(ray, distance + 0.1f, targetLayer.value))
            {
                return true;
            }
            else
            {
                return  false;
            }            

        }


    }
}