using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private string name;
    [SerializeField] private int damage;
    [SerializeField] private LayerMask targetLayerMask;


    public GameObject bullet;
    public float bulletSpeed = 0;
    [SerializeField] float shootCoolDown = 0.3f;

    public GameObject render;

    float shootTimer;

    public void Dequip()
    {
        StartCoroutine(IDequip());
    }

    IEnumerator IDequip()
    {        
        Rigidbody rb = this.gameObject.AddComponent<Rigidbody>();

        this.GetComponentInChildren<Collider>().enabled = true;
        rb.AddForce(transform.forward,ForceMode.Impulse);

        yield return new WaitForSeconds(0.1f);
        while (rb.velocity.magnitude > 0.1)
        {
            yield return null;
        }

        Destroy(rb);
    }
    

    public void Equip()
    {
        this.transform.GetComponentInChildren<Collider>().enabled = false;
        this.transform.parent = PlayerCamera.Instance.Hand;
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
    }


    public void Shoot()
    {
        //shoot
       
            if (shootTimer <= 0)
            {
                shootTimer = shootCoolDown;

                Quaternion axis = Camera.main.transform.rotation;
                Vector3 shootDir = Vector3.forward;
                //Debug.Log("Mouse Buttoned!");

                GameObject go = GameObject.Instantiate(bullet);
                go.transform.position = Camera.main.transform.position;
                go.transform.rotation = axis;

                go.GetComponent<Bullet>().Shoot(shootDir,bulletSpeed,targetLayerMask);

                shootTimer = shootCoolDown;

            }

    }


    // Update is called once per frame
    void Update()
    {
        shootTimer -= Time.deltaTime;
    }


}
