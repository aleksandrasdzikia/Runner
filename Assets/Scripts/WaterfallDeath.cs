using UnityEngine;
using System.Collections;

public class WaterfallDeath : MonoBehaviour
{

    private GameObject Player;
    private bool inCollsion;
    public bool inAir = false;

    void Start()
    {

        Player = GameObject.FindWithTag("Player");
        //isItInAir = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Jump") || GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Run->Jump");
    }

    void Update()
    {
        AnimatorStateInfo asi = Player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        AnimatorTransitionInfo ati = Player.GetComponent<Animator>().GetAnimatorTransitionInfo(0);
        this.inAir = asi.IsName("Jump") || ati.IsName("Run -> Jump");

        if (inCollsion)
        {
            if (inAir)
            {
                Debug.Log("Death");
                //death
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        this.inCollsion = true;
        //Player.GetComponent<Player>().isInAir();
        //inCollision = true;
        //Debug.Log("inTriger");
        //touchingSafetyJump = true;
        //Player.GetComponent<AnimatorStateInfo>().IsName("Jump");
    }

    void OnTriggerExit(Collider other)
    {
        this.inCollsion = false;
        //inCollision = false;
        //Debug.Log("outTriger");
        //touchingSafetyJump = false;
        //Player.GetComponent<Player>().freezeEnable = true;
    }

}
