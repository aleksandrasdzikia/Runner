using UnityEngine;
using System.Collections;

public class FreezeTapCollision : MonoBehaviour
{

    private GameObject Player;

    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        // tap = Player.GetComponent<Player>().tapMove();
    }


    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("inTriger");
        //touchingSafetyJump = true;
        Player.GetComponent<Player>().freezeEnable = false;
    }

    void OnTriggerExit(Collider other)
    {
        //Debug.Log("outTriger");
        //touchingSafetyJump = false;
        Player.GetComponent<Player>().freezeEnable = true;
    }

}
