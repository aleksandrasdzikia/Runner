using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private float speed = 120;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        this.transform.gameObject.SetActive(false);
        //run animation of coin dissapearing
    }
}
