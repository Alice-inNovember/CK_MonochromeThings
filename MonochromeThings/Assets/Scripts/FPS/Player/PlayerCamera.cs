using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClassTemp;
using DG.Tweening;



public class PlayerCamera : MonoBehaviourSingleton<PlayerCamera>
{

    [Header("Camera Objects")]
    public GameObject playerCamGO;
    public Camera playerCam;

    [Header("Camera Origin Stats")]
    public float originFov;
    [SerializeField] private float rotSpeed = 200f;

    private float mx;
    public float get_mx
    {
        get { return mx; }
    }

    private float my;


    [Header("Weapon")]
    public Transform hand;

    private void Start()
    {

        if (playerCam == null)
        {
            playerCam = Camera.main;
        }

        if (playerCamGO == null)
        {
            playerCamGO = Camera.main.gameObject;
        }

        if (hand == null)
        {
            hand = playerCamGO.GetComponentInParent<Transform>();
        }

        if (originFov == 0.0f)
            originFov = playerCam.fieldOfView;

    }

    private void Update()
    {
        //camRotate

        float mX = Input.GetAxis("Mouse X");
        float mY = Input.GetAxis("Mouse Y");

        mx += mX * rotSpeed * Time.deltaTime;
        my += mY * rotSpeed * Time.deltaTime;


        my = Mathf.Clamp(my, -90, 90);



        playerCamGO.transform.localEulerAngles = new Vector3(-my, 0, 0);        
        //플레이어 돌리기
        //transform.localEulerAngles = new Vector3(0, mx, 0);


    }

    public void ResetFov()
    {
        playerCam.DOFieldOfView(originFov, 0.25f);
    }

}
