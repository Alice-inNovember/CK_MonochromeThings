using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{

    public GameObject beforeGo;
    public GameObject afterGo;

    public ParticleSystem effect;


    private void Awake()
    {
        if (effect == null)
        {
            effect = GetComponent<ParticleSystem>();           
        }

    }

    private void Start()
    {
        Invoke(nameof(Destroy_Object),2.0f);
        
    }


    public void Destroy_Object()
    {
        effect.Play();       
    }

    IEnumerator IAttachRb()
    {
        Rigidbody rb = this.gameObject.AddComponent<Rigidbody>();               

        yield return new WaitForSeconds(0.1f);
        while (rb.velocity.magnitude > 0.1)
        {
            yield return null;
        }

        Destroy(rb);
    }

    private void ChangeGO()
    {
        beforeGo.SetActive(false);
        afterGo.SetActive(true);
    }

    private void OnParticleSystemStopped()
    {
        StartCoroutine(IAttachRb());
        ChangeGO();
    }


}
