using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerV2 : MonoBehaviour {

	private Animator animator;
	private Rigidbody rigidBody;
	private Vector3 additionalVector = Vector3.zero;

	private Vector3 lastPosition = Vector3.zero;

	// Use this for initialization
	void Start () {
		this.animator = GetComponent<Animator>();
		this.rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		bool idling = animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
		bool running = animator.GetCurrentAnimatorStateInfo(0).IsName("Run");
		bool space = Input.GetKeyDown("space");
		bool left = Input.GetKeyDown("left");
		bool right = Input.GetKeyDown("right");

		if (idling) {
			if (space)
			   animator.SetTrigger("Run");
		} else if (running) {
			if (left) {
				this.transform.Rotate(new Vector3(0, -90, 0));
				this.additionalVector = Quaternion.Euler(-90, 0, 0) * this.additionalVector;
			} else if (right) {
				this.transform.Rotate(new Vector3(0, 90, 0));
				this.additionalVector = Quaternion.Euler(90, 0, 0) * this.additionalVector;
			} else if (space)
			   animator.Play("Idle");
		}

		if (this.additionalVector != Vector3.zero) {
			Vector3 velocity = this.transform.position - lastPosition;
			Vector3 sideVelocity = velocity;
			sideVelocity.y = 0;
			this.transform.Translate(this.additionalVector * sideVelocity.magnitude);
		}

		lastPosition = this.transform.position;
	} 

	void OnTriggerEnter(Collider collider) {
		Debug.Log("Touched");
		ActionTriggerOld at = collider.GetComponent<ActionTriggerOld>();
		if (at) {
			string[] actions = at.getActionType().Split(':');
			foreach (string action in actions) {
				switch (action) {
					case "up":
						this.additionalVector += Vector3.up;
						break;
					case "down":
						this.additionalVector += -Vector3.up;
						break;
					case "stop":
						animator.SetTrigger("Stop");
						break;
					case "goto":
						this.transform.Translate(at.getGoToTranslate());
						break;	
					default: break;
				}
			}
		}
	}
}
