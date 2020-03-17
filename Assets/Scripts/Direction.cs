using UnityEngine;

public enum Direction : short {
	Forward, Right, Backward, Left, None
};

static class DirectionVector {
	public static Vector3 getVector(Direction dir) {
		switch (dir) {
			case Direction.Forward:
				return Vector3.forward;
			case Direction.Right:
				return Vector3.right;
			case Direction.Backward:
				return -Vector3.forward;
			case Direction.Left:
				return -Vector3.right;
			default: return Vector3.zero;
		}
	}
}