using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpDetector : MonoBehaviour
{
    public LayerMask wallLayer;

    [SerializeField]private Transform orientation;
    [SerializeField] private Rigidbody rb;

    public float wallCheckDistance;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool leftWall;
    private bool rightWall;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        orientation = GetComponent<Transform>();
    }



    private void CheckWall()
    {
        leftWall = Physics.Raycast(transform.position, -orientation.right,out leftWallHit , wallCheckDistance, wallLayer);
        rightWall = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, wallLayer);
    }



}
