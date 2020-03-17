using UnityEngine;
using System.Collections;

public class DisableInput : MonoBehaviour
{
    public bool touchedCollision;

    void OnTriggerEnter (Collider other)
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
