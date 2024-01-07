using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class TestCameraMove : MonoBehaviour
{
    [SerializeField] private Vector3 pos1;
    [SerializeField] private Vector3 pos2;
    [SerializeField] private float duration;
    [SerializeField] private GameObject cameraObj;
    public void Pos1()
    {
        cameraObj.transform.DOPause();
        cameraObj.transform.DOMove(pos1, duration).SetEase(Ease.OutSine);
    }

    public void Pos2()
    {
        cameraObj.transform.DOPause();
        cameraObj.transform.DOMove(pos2, duration).SetEase(Ease.OutSine);
    }
}
