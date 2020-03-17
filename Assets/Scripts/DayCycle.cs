using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour {

	public float speed = 1f;
	public static DayCycle main = null;
	public static float dayTime = 0f;
	public static bool dayStart = false;
	public static bool dayEnd = false;
	public static bool nightStart = false;
	public static bool nightEnd = false;

	void Start() {
		DayCycle.main = this;
	}

	void Update () {
		bool minus = Input.GetKeyDown("a");
		bool plus = Input.GetKeyDown("s");
		bool debug = Input.GetKeyDown("d");

		if (plus)
			this.speed += 0.5f;
		if (minus)
			this.speed -= 0.5f;

		if (debug)
			Debug.Log("Current day time: " + DayCycle.dayTime + " and cycle speed: " + this.speed);

		float deltaAngle = this.speed * Time.deltaTime;
		if ((this.transform.localEulerAngles.x < 315f && this.transform.localEulerAngles.x > 305f) || (this.transform.localEulerAngles.x < 55f && this.transform.localEulerAngles.x > 45f))
			deltaAngle += 90f;
		this.transform.Rotate(Vector3.left, deltaAngle);

		DayCycle.dayTime = this.calculateDayTime();
	}

	float calculateDayTime() {
		if (DayCycle.main == null)
			return 0f;

		float angle = DayCycle.main.gameObject.transform.eulerAngles.y;
		float normalizedAngle = ((45 + angle) % 360f) / 360f;
		float dayTime = 0f;

		DayCycle.dayStart = false;
		DayCycle.dayEnd = false;
		DayCycle.nightStart = false;
		DayCycle.nightEnd = false;

		if (normalizedAngle >= 0.75f ) {
			dayTime = -0.75f + normalizedAngle;
			DayCycle.dayStart = true;
		} else if (normalizedAngle < 0.25f) {
			dayTime = 0.25f - normalizedAngle;
		DayCycle.dayEnd = true;
		} else if (normalizedAngle >= 0.25f && normalizedAngle <= 0.5f) {
			dayTime = -1 * (normalizedAngle - 0.25f);
			DayCycle.nightStart = true;
		} else {
			dayTime = -1 * (0.75f - normalizedAngle);
			DayCycle.nightEnd = true;
		}
		return dayTime / 0.25f;
	}
}
