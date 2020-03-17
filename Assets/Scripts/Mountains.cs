using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mountains : MonoBehaviour {

	private Camera camera;
	private Vector3 lastCameraPosition = Vector3.zero;
	private Vector3 startingMountainsPosition = Vector3.zero;
	public Vector3 newMountainsOffset = new Vector3(0f, 1f, 0f);

	public float speed = 0.05f;
	public float newMountainsAt = 5f;
	private float totalMoveBy = 0f;

	void Start () {
		this.camera = this.transform.parent.GetComponent<Camera>();
		this.lastCameraPosition = this.camera.transform.position;
		this.startingMountainsPosition = this.transform.GetChild(0).position;
	}

	void Update () {
		Vector3 cameraVelocity = this.camera.transform.position - this.lastCameraPosition;
		float moveBy = cameraVelocity.y * this.speed;
		this.totalMoveBy += moveBy;
		if (this.totalMoveBy > this.newMountainsAt) {
			this.totalMoveBy = 0f;
        	GameObject newMountains = MonoBehaviour.Instantiate(Res.GetRes("mountains"), this.transform);
        	newMountains.transform.position = this.transform.GetChild(this.transform.childCount - 1).position + this.newMountainsOffset;
        	newMountains.transform.Translate(new Vector3(0f, 0f, 0.5f - Random.value));
		}
		foreach (Transform child in this.transform) {
			child.transform.position += -Vector3.up * moveBy;
		}

		this.lastCameraPosition = this.camera.transform.position;
	}
}
