using UnityEngine;
using System.Collections;

public class RotateMesh : MonoBehaviour
{
    public bool pressedButton = false;
    public bool collisionOne = false;
    public bool collisionTwo = false;
    public Collider coll1;
    public Collider coll2;

    private float speed = 10f;
    private Vector3 angle;
    private Vector3 targetAngle270 = new Vector3(0f, 270f, 0f);
    private Vector3 targetAngle180 = new Vector3(0f, 180f, 0f);
    //Vector3 to = new Vector3(0, -90, 0);


    // Use this for initialization
    void Start()
    {
        angle = transform.eulerAngles;
        //run = GetComponent<Player2V>().running;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            pressedButton = true;
        }

        Debug.Log(this.transform.eulerAngles.y);

        if (pressedButton)
        {
            if (collisionOne)
            {
                angle = new Vector3(
                Mathf.LerpAngle(angle.x, targetAngle270.x, Time.deltaTime * speed),
                Mathf.LerpAngle(angle.y, targetAngle270.y, Time.deltaTime * speed),
                Mathf.LerpAngle(angle.z, targetAngle270.z, Time.deltaTime * speed));
                transform.eulerAngles = angle;

                if (this.transform.eulerAngles.y >= 269 && pressedButton)
                {
                    collisionOne = false;
                    collisionTwo = true;
                    pressedButton = false;
                }
            }

            if (collisionTwo)
            {
                angle = new Vector3(
                Mathf.LerpAngle(angle.x, targetAngle270.x, Time.deltaTime * speed),
                Mathf.LerpAngle(angle.y, targetAngle270.y, Time.deltaTime * speed),
                Mathf.LerpAngle(angle.z, targetAngle270.z, Time.deltaTime * speed));
                transform.eulerAngles = angle;

                if (this.transform.eulerAngles.y <= 181 && pressedButton)
                {
                    collisionOne = true;
                    collisionTwo = false;
                    pressedButton = false;
                }
            }

            else
            {
                return; //pressedButton = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (coll1.isTrigger)
        {
            collisionOne = true;
        }
        if (coll2.isTrigger)
        {
            collisionTwo = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        collisionOne = false;
        collisionTwo = false;
    }
}
