using UnityEngine;
using System.Collections;

public class JumpAllow : MonoBehaviour
{
    private bool touchMoveEnabled = true;
    public bool touchingSafetyJump = false;
    private GameObject Player;
    private  float touchDuration = 0f;
    private Vector2 currentTouchMove = Vector2.zero;

    void Start()
    {
        Player = GameObject.FindWithTag("Player");
       // tap = Player.GetComponent<Player>().tapMove();
    }

    // Update is called once per frame
    void Update()
    {
        //if (this.GetComponent<JumpAllow>().touchingSafetyJump)
        //{
        //    if (Input.GetKeyDown("space"))
        //    {
        //        Player.GetComponent<Player>().freezeEnable = false;
        //        Player.GetComponent<Animator>().SetTrigger("StartJump");
        //        //Player.GetComponent<Player>().freezeEnable = true;
        //    }
        //}

        if (this.GetComponent<JumpAllow>().touchingSafetyJump)
            if (Input.touchCount < 2)
            {
                if(Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (this.touchMoveEnabled)
                    {
                        Debug.Log("Touch");
                        Player.GetComponent<Animator>().SetTrigger("StartJump");
                        this.touchDuration = 0f;
                        this.touchMoveEnabled = false;
                        this.currentTouchMove = Vector2.zero;
                        return;
                    }
                }

            }
            else
            {
                if (this.touchDuration > 0f)
                {
                    this.touchDuration = 0f;
                    Player.GetComponent<Animator>().SetTrigger("StartJump");
                }
                this.touchMoveEnabled = true;
            }
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("inTriger");
        touchingSafetyJump = true;
        Player.GetComponent<Player>().tapEnable = false;
    }

    void OnTriggerExit(Collider other)
    {
        //Debug.Log("outTriger");
        touchingSafetyJump = false;
        Player.GetComponent<Player>().tapEnable = true;
    }

}
