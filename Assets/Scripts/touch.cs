using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touch : MonoBehaviour
{
    private float touchDuration = 0f;
    public bool touchMoveEnabled = true;
    private Vector2 currentTouchMove = Vector2.zero;
    public float angleLeft;
    public float angleRight;
    public float magnitude;

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (this.touchMoveEnabled)
            {
                Debug.Log("Tap");
                //if (this.touchDuration >= 0.5f) {
                //this.touchDuration = 0f;
                //this.tapMove();
                //this.touchMoveEnabled = false;
                //this.currentTouchMove = Vector2.zero;
                //return;
                //}

                //----------------------------------------------------------------
                //this.touchDuration += Time.deltaTime;
                //if (touch.phase == TouchPhase.Moved)
                //{
                //    this.currentTouchMove += touch.deltaPosition;
                //    if (this.currentTouchMove.magnitude > magnitude)
                //    {
                //        float angle = Vector2.Angle(Vector2.up, this.currentTouchMove) * Mathf.Sign(this.currentTouchMove.x);
                //        Debug.Log(angle);
                //        //if (angle >= angleLeft && angle <= angleRight) //if (angle >= 45f && angle <= 135f)
                //        {
                //            //Debug.Log("Right");
                //            //Debug.Log(angle);
                //        }
                //        //else if (angle >= angleLeft || angle <= angleRight) //(angle >= 135f || angle <= -135f)
                //        //{
                //        //    Debug.Log("Roll");
                //        //}
                //        //else if (angle >= angleLeft && angle <= angleRight) //(angle >= -135f && angle <= -45f)
                //        //{
                //        //    Debug.Log("Left");
                //        //}
                //        //if (angle <= angleLeft && angle >= angleRight) //(angle <= 0f && angle >= -45f)
                //        //{
                //        //    Debug.Log("Jump");
                //        //}

                //        this.touchDuration = 0f;
                //        this.currentTouchMove = Vector2.zero;
                //        this.touchMoveEnabled = false;
                //    }
                //}

                //------------------------------------------------------------------------------------
            }
        }
        else
        {
            if (this.touchDuration > 0f)
            {
                this.touchDuration = 0f;
                //this.tapMove();
            }
            this.touchMoveEnabled = true;
        }
    }
}
