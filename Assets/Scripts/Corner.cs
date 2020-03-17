using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corner : MonoBehaviour
{

    public ParticleSystem corner;
    private bool touchedCollision;
    private bool inKey;

    // Start is called before the first frame update
    private void Awake()
    {
        corner.Stop();
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            inKey = true;
        }

        if (inKey && touchedCollision)
        {
            this.corner.Play();
            StartCoroutine(stopParticles());
            inKey = false;
        }
    }

    IEnumerator stopParticles()
    {
        yield return new WaitForSeconds(.5f);
        this.corner.Stop();
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("In Triger");
        touchedCollision = true;
    }

    public void OnTriggerExit(Collider other)
    {
        //Debug.Log("Out Triger");
        touchedCollision = false;
    }

}
