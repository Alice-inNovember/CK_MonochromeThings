using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS {
    public class WallJumpDetector : MonoBehaviour
    {
        public LayerMask wallLayer;

        [SerializeField] private Transform orientation;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private GroundDetector gd;

        public float wallJumpUpForce = 5f;
        public float wallJumpSideForce = 4f;

        public float wallCheckDistance = 2f;
        private RaycastHit leftWallHit;
        private RaycastHit rightWallHit;
        private List<int> prevWallHit_Hash = new List<int>();

        [SerializeField] private bool leftWall;
        [SerializeField] private bool rightWall;


        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            orientation = GetComponent<Transform>();
            gd = GetComponent<GroundDetector>();    
        }

        private void Update()
        {
            CheckWall();
            if (gd.DetectGround())
            {
                prevWallHit_Hash.Clear();
            }

    }

        public bool IsWallExist()
        {

            return leftWall || rightWall ? true : false;
        }


        public void WallJump()
        {
            Debug.Log("WallJump()");
            Vector3 wallNormal = rightWall ? rightWallHit.normal : leftWallHit.normal;
            prevWallHit_Hash.Add(rightWall ? rightWallHit.transform.GetHashCode() : leftWallHit.transform.GetHashCode());

            Vector3 forceToApply = orientation.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

            // reset y velocity and add force
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(forceToApply, ForceMode.Impulse);
        }

        private void CheckWall()
        {
            leftWall = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, wallLayer);
            rightWall = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, wallLayer);


            foreach (var item in prevWallHit_Hash)
            {
                if (leftWall && leftWallHit.transform.GetHashCode() == item)
                {
                    leftWall = false;
                }
                if (rightWall && rightWallHit.transform.GetHashCode() == item)
                {
                    rightWall = false;
                }                

            }


        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            if (orientation != null)
            {
                Gizmos.DrawLine(orientation.position, orientation.position + (orientation.right* wallCheckDistance));
                Gizmos.DrawLine(orientation.position, orientation.position - (orientation.right* wallCheckDistance));
            }
        }


    }

}