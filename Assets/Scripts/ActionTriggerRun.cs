using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Collider))]
public class ActionTriggerRun : ActionRun
{
	public bool boardSlide = false;

	void Start() {
		this.GetComponent<Collider>().isTrigger = true;
	}

	public Vector3 getDirectionVector(Vector3 from, Direction directionTo, bool disableTrigger = true) {
		GameObject toObject = null;
		if (directionTo == Direction.Forward && base.goForwardTowards != null)
			toObject = base.goForwardTowards;
		else if (directionTo == Direction.Backward && base.goBackwardTowards != null)
			toObject = base.goBackwardTowards;
		else if (directionTo == Direction.Left && base.goLeftTowards != null)
			toObject = base.goLeftTowards;
		else if (directionTo == Direction.Right && base.goRightTowards != null)
			toObject = base.goRightTowards;
		else return Vector3.zero;

		if (toObject == null)
			return Vector3.zero;

		if (disableTrigger)
			this.DisableTrigger();

		return (toObject.transform.position - from).normalized;
	}

	public void DisableTrigger() {
		this.gameObject.GetComponent<BoxCollider>().isTrigger = false;
	}

	public Direction getDirection() {
		if (base.goForwardTowards != null)
			return Direction.Forward;
		else if (base.goBackwardTowards != null)
			return Direction.Backward;
		else if (base.goLeftTowards != null)
			return Direction.Left;
		else if (base.goRightTowards != null)
			return Direction.Right;
		else return Direction.None;
	}

    void OnRenderObject()
    {
       if(FindObjectOfType<DebugArrows>().DrawArrows)
       {
           if (this.goForwardTowards != null)
               DrawArrow.ForDebugTwoPoints(this.transform.position, this.goForwardTowards.transform.position, new Color(255, 0, 255), 0.25f, 20f, 1f);
           if (this.goLeftTowards != null)
               DrawArrow.ForDebugTwoPoints(this.transform.position, this.goLeftTowards.transform.position, new Color(255, 0, 255), 0.25f, 20f, 1f);
           if (this.goRightTowards != null)
               DrawArrow.ForDebugTwoPoints(this.transform.position, this.goRightTowards.transform.position, new Color(255, 0, 255), 0.25f, 20f, 1f);
           if (this.goBackwardTowards != null)
               DrawArrow.ForDebugTwoPoints(this.transform.position, this.goBackwardTowards.transform.position, new Color(255, 0, 255), 0.25f, 20f, 1f);
       }
       else
       {
           return;
       }
    }
}