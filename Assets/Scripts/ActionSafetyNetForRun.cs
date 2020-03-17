using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Collider))]
public class ActionSafetyNetForRun : MonoBehaviour, IAction
{
	public static Vector3 boxSize = new Vector3(1.5f, 1.5f, 1.5f);
	public ActionRun actionRun;

	public bool EarlyMove(Direction direction) {
		if (actionRun == null || direction == Direction.None)
			return false;

		return actionRun.EnableTrigger(direction);
	}
}