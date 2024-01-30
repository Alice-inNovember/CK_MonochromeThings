using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 dir;
    static float InitBulletSpeed = 5.0f;
    float bulletSpeed = 5.0f;
    
    static private float eliminateTime = 10.0f;
    private float timer = eliminateTime;



    LayerMask targetLayer;

    private void FixedUpdate()
    {
        this.transform.Translate(dir*Time.fixedDeltaTime* bulletSpeed);
        if (timer<= 0)
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
        else
        {
            timer-=Time.fixedDeltaTime;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter(Collider other).Bullet");
        if (other.gameObject.layer.Equals(targetLayer))
        {
            Destroy(other.gameObject);
        }
        Destroy(this.gameObject);
    }

    public void Shoot(Vector3 tDir, float tSpeed = 0.0f , int targetLayer = 20)
    {
        dir = tDir;

        if (tSpeed < 0.0f)
        {
            bulletSpeed = InitBulletSpeed;
        }
        else
        {
            bulletSpeed = tSpeed;
        }

        this.targetLayer = targetLayer;

    }

}
