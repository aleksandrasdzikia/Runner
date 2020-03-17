using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;

    private Camera camera;
    private Vector3 velocity = Vector3.zero;
    private Vector3 tempPosition = Vector3.zero;

    private Vector2 Vektorius = Vector3.zero;
    private bool function = false;
    private float timing = 0f;
    private float speed = 1.2f;
    private float slowDownSpeed = 1.2f;

    private Vector3 startPosition = Vector3.zero;

    void Start()
    {
        this.camera = this.GetComponent<Camera>();
        this.startPosition = this.transform.position;

        function = true;
        Vektorius = new Vector2(this.transform.position.x, this.transform.position.z);
    }

    void Update()
    {
        bool r = Input.GetKeyDown("r");
        if (r)
            this.transform.position = this.startPosition;
        Vector3 playerScreenPosition = camera.WorldToScreenPoint(player.transform.position);
        float centerWidth = camera.pixelWidth / 2f;
        float centerHeight = camera.pixelHeight / 3f; //Default 2.5
        Vector3 middlePosition = camera.ScreenToWorldPoint(new Vector3(centerWidth, centerHeight, playerScreenPosition.z));
        // Debug.DrawLine(middlePosition, player.transform.position, Color.red, 1f);
        Vector3 distance = player.transform.position - middlePosition;
        distance = Quaternion.AngleAxis(35, -this.transform.right) * distance;

        Vector3 targetPosition = this.transform.position + distance;
        // Debug.Log(Vector3.Distance(targetPosition, this.transform.position) + " " + Vector3.Distance(middlePosition, player.transform.position) + " " + (Vector3.Distance(targetPosition, this.transform.position) == Vector3.Distance(middlePosition, player.transform.position)));
        // Debug.DrawLine(this.transform.position, targetPosition, new Color(255, 0, 255), 1f);


        Vector2 smth = new Vector2(targetPosition.x, targetPosition.z);
        Vector2 direction = smth - Vektorius;
        float magnitude = direction.magnitude;
        if (magnitude > timing)
            speed = timing / (magnitude * slowDownSpeed);
        else speed = 1f;

        Vector3 targetPositionUp = new Vector3(this.transform.position.x, targetPosition.y, this.transform.position.z);
        this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPositionUp, ref this.velocity, 0.5f, 4f);
        Vector3 targetPositionSide = new Vector3(targetPosition.x, this.transform.position.y, targetPosition.z);
        this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPositionSide, ref this.velocity, 0.5f, speed);
    }

    public void reset() {
        this.transform.position = this.startPosition;
    }
}
