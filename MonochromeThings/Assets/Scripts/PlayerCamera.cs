using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClassTemp;
using DG.Tweening;

public class PlayerCamera : Singleton<PlayerCamera>
{

    [Header("Camera Objects")]
    public GameObject playerCamGO;
    public Camera playerCam;

    [Header("Camera Origin Stats")]
    public float originFov;


    [Header("Weapon")]
    public Transform Hand;

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

        if (originFov == 0.0f)
            originFov = playerCam.fieldOfView;

    }

    public void ResetFov()
    {
        playerCam.DOFieldOfView(originFov, 0.25f);
    }

}
