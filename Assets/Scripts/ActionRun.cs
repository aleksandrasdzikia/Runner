using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Collider))]
public class ActionRun : MonoBehaviour, IAction
{
	public GameObject goForwardTowards; // top left
	public float forwardSpeedMultiplier = 1f;
	public float forwardMaxSpeedMultiplier = 1f;
	public GameObject goLeftTowards; // bottom left
	public float leftSpeedMultiplier = 1f;
	public float leftMaxSpeedMultiplier = 1f;
	public GameObject goRightTowards; // top right
	public float rightSpeedMultiplier = 1f;
	public float rightMaxSpeedMultiplier = 1f;
	public GameObject goBackwardTowards; // bottom right
	public float backwardSpeedMultiplier = 1f;
	public float backwardMaxSpeedMultiplier = 1f;
	public Vector3 specialDirection = Vector3.zero;
	public string specialAnimation = null;

	private Direction triggerDirection = Direction.None;
	private BoxCollider triggerCollider;

	public GameObject safetyNet;

    //public bool Arrows;

	public Vector3 getDirectionVector(Vector3 from, Direction directionTo, bool disableTrigger = true) {
		GameObject toObject = null;
		switch (directionTo) {
			case Direction.Forward:
				toObject = this.goForwardTowards;
				break;
			case Direction.Backward:
				toObject = this.goBackwardTowards;
				break;
			case Direction.Left:
				toObject = this.goLeftTowards;
				break;
			case Direction.Right:
				toObject = this.goRightTowards;
				break;
			default:
				return Vector3.zero;
		}

		if (toObject == null)
			return Vector3.zero;

		if (disableTrigger)
			this.DisableTrigger();

		return (toObject.transform.position - from).normalized;
	}

	public float getSpeedMultiplier(Direction directionTo) {
		switch (directionTo) {
			case Direction.Forward:
				return this.forwardSpeedMultiplier;
			case Direction.Backward:
				return this.backwardSpeedMultiplier;
			case Direction.Left:
				return this.leftSpeedMultiplier;
			case Direction.Right:
				return this.rightSpeedMultiplier;
			default:
				return 0f;
		}

		//return 0f;
	}

	public float getMaxSpeedMultiplier(Direction directionTo) {
		switch (directionTo) {
			case Direction.Forward:
				return this.forwardMaxSpeedMultiplier;
			case Direction.Backward:
				return this.backwardMaxSpeedMultiplier;
			case Direction.Left:
				return this.leftMaxSpeedMultiplier;
			case Direction.Right:
				return this.rightMaxSpeedMultiplier;
			default:
				return 0f;
		}

		return 0f;
	}

	public Vector3 getSpecialDirection() {
		return this.specialDirection;
	}

	public string getSpecialAnimation() {
		return this.specialAnimation;
	}

	public void LinkTo(GameObject go, Direction dir) {
		switch (dir) {
			case Direction.Forward:
				this.goForwardTowards = go;
				break;
			case Direction.Backward:
				this.goBackwardTowards = go;
				break;
			case Direction.Left:
				this.goLeftTowards = go;
				break;
			case Direction.Right:
				this.goRightTowards = go;
				break;
			default:
				break;
		}
	}

	public bool EnableTrigger(Direction directionTo) {
		GameObject toObject = null;
		switch (directionTo) {
			case Direction.Forward:
				toObject = this.goForwardTowards;
				break;
			case Direction.Backward:
				toObject = this.goBackwardTowards;
				break;
			case Direction.Left:
				toObject = this.goLeftTowards;
				break;
			case Direction.Right:
				toObject = this.goRightTowards;
				break;
			default:
				return false;
		}

		if (toObject == null) {
			this.DisableTrigger();
			return false;
		}

		this.triggerCollider = this.gameObject.AddComponent<BoxCollider>();
		this.triggerCollider.size = new Vector3(0.7f, 0.7f, 0.7f);
		this.triggerCollider.isTrigger = true;
		this.triggerDirection = directionTo;

		return true;
	}

	public Direction GetTriggerDirection(bool disableTrigger = true) {
		Direction returnDirection = this.triggerDirection;
		if (disableTrigger)
			this.DisableTrigger();
		return returnDirection;
	}

	public void DisableTrigger() {
		if (this.triggerCollider != null) {
			MonoBehaviour.Destroy(this.triggerCollider);
			this.triggerCollider = null;
		}
		this.triggerDirection = Direction.None;
	}

    void OnRenderObject()
    {
       if(FindObjectOfType<DebugArrows>().DrawArrows)
       {
           if (this.goForwardTowards != null)
               DrawArrow.ForDebugTwoPoints(this.transform.position, this.goForwardTowards.transform.position, Color.red, 0.25f, 20f, 1f);
           if (this.goLeftTowards != null)
               DrawArrow.ForDebugTwoPoints(this.transform.position, this.goLeftTowards.transform.position, Color.red, 0.25f, 20f, 1f);
           if (this.goRightTowards != null)
               DrawArrow.ForDebugTwoPoints(this.transform.position, this.goRightTowards.transform.position, Color.red, 0.25f, 20f, 1f);
           if (this.goBackwardTowards != null)
               DrawArrow.ForDebugTwoPoints(this.transform.position, this.goBackwardTowards.transform.position, Color.red, 0.25f, 20f, 1f);
       }
       else
       {
           return;
       }
    }
}