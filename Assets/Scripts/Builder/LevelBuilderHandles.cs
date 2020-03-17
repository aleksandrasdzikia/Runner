#if (UNITY_EDITOR)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor (typeof(LevelBuilderRoot))]
public class LevelBuilderRootHandles : Editor {

	void OnSceneGUI() {
		LevelBuilderCommon.ParseEvent(Event.current);

		LevelBuilderRoot root = target as LevelBuilderRoot;
		LevelBuilder levelBuilder = LevelBuilder.lastBuilder;

		LevelBuilderCommon.Default5ActionButton(root.transform.position + new Vector3(0, 1.5f, 0));
	}
}

[CustomEditor (typeof(BuildingBlock))]
public class LevelBuilderArrow : Editor {

	void OnSceneGUI() {
		LevelBuilderCommon.ParseEvent(Event.current);

		Transform transform = (target as MonoBehaviour).GetComponent<Transform>() as Transform;

		float size = HandleUtility.GetHandleSize(transform.position);
		float snap = LevelBuilder.levelUnit;
		Vector3 translateVector = new Vector3(-1f, 1f, -1f);

		EditorGUI.BeginChangeCheck();
		Vector3 newTargetPosition = Handles.Slider(transform.position, translateVector, size, Handles.ArrowHandleCap, snap);
		if (EditorGUI.EndChangeCheck() && (newTargetPosition - transform.position).magnitude > 1f) {
			Undo.RegisterUndo(transform, "Change Look At Target Position");
			transform.Translate((newTargetPosition.x > transform.position.x ? -1 : 1) * translateVector);
		}
	}
}

[CustomEditor (typeof(ActionRun))]
public class LevelBuilderActionRunHandles : Editor {

	void OnSceneGUI() {
		ActionRun action = target as ActionRun;
		int directions
			= (action.goForwardTowards == null ? 0x1 : 0)
			| (action.goLeftTowards == null ? 0x2 : 0)
			| (action.goRightTowards == null ? 0x4 : 0)
			| (action.goBackwardTowards == null ? 0x8 : 0);

		LevelBuilderCommon.DirectionalButtons(action, directions);
		if (action.safetyNet == null)
			LevelBuilderCommon.SafetyNetButton(action);
		LevelBuilderCommon.DeleteButton(action);
	}
}

[CustomEditor (typeof(ActionTriggerRun))]
public class LevelBuilderActionTriggerRunHandles : Editor {

	void OnSceneGUI() {
		ActionTriggerRun action = target as ActionTriggerRun;
		int directions
			= (action.goForwardTowards == null ? 0x1 : 0)
			| (action.goLeftTowards == null ? 0x2 : 0)
			| (action.goRightTowards == null ? 0x4 : 0)
			| (action.goBackwardTowards == null ? 0x8 : 0);

		LevelBuilderCommon.DirectionalButtons(action, directions);
		LevelBuilderCommon.DeleteButton(action);
	}
}

[CustomEditor (typeof(ActionTranslate))]
public class LevelBuilderActionTranslateHandles : Editor {

	void OnSceneGUI() {
		ActionTranslate action = target as ActionTranslate;

		if (action.translateToObject == null)
			LevelBuilderCommon.Default5ActionButton(action.transform.position + new Vector3(0, 1.5f, 0), action, -1);
		LevelBuilderCommon.DeleteButton(action);
	}
}

[CustomEditor (typeof(ActionDeath))]
public class LevelBuilderActionDeathHandles : Editor {

	void OnSceneGUI() {
		ActionDeath action = target as ActionDeath;

		LevelBuilderCommon.Default5ActionButton(action.transform.position + new Vector3(0, 1.5f, 0), action);
		LevelBuilderCommon.DeleteButton(action);
	}
}

[CustomEditor (typeof(ActionSafetyNetForRun))]
public class LevelBuilderActionSafetyNetForRunHandles : Editor {

	void OnSceneGUI() {
		ActionSafetyNetForRun action = target as ActionSafetyNetForRun;
		LevelBuilderCommon.DeleteButton(action);
	}
}

[CustomEditor (typeof(ActionNone))]
public class LevelBuilderActionNoneHandles : Editor {

	void OnSceneGUI() {
		ActionNone action = target as ActionNone;

		LevelBuilderCommon.Default5ActionButton(action.transform.position + new Vector3(0, 1.5f, 0), action);
		LevelBuilderCommon.DeleteButton(action);
	}
}

[CustomEditor (typeof(ActionSlide))]
public class LevelBuilderActionSlideHandles : Editor {

	void OnSceneGUI() {
		ActionSlide action = target as ActionSlide;

		LevelBuilderCommon.Default5ActionButton(action.transform.position + new Vector3(0, 1.5f, 0), action);
		LevelBuilderCommon.DeleteButton(action);
	}
}

[CustomEditor (typeof(ActionJump))]
public class LevelBuilderActionJumpHandles : Editor {

	void OnSceneGUI() {
		ActionJump action = target as ActionJump;

		LevelBuilderCommon.Default5ActionButton(action.transform.position + new Vector3(0, 1.5f, 0), action);
		LevelBuilderCommon.DeleteButton(action);
	}
}

public static class LevelBuilderCommon {

	public const float button_size = 0.3f;
	public static int page = 0;
	private static bool pageSwitch = false;

	public static void ScrollPage(int direction) {
		LevelBuilderCommon.page = Mathf.Abs(LevelBuilderCommon.page + direction) % 2;
	}

	public static void ParseEvent(Event evt) {
		if (evt.character == 'Q' && evt.shift == true) {
			if (!LevelBuilderCommon.pageSwitch) {
				LevelBuilderCommon.pageSwitch = true;
				LevelBuilderCommon.ScrollPage(1);
			}
		} else LevelBuilderCommon.pageSwitch = false;
	}



	public static void DeleteButton(IAction action) {
		Transform transform = (action as MonoBehaviour).GetComponent<Transform>() as Transform;

		LevelBuilder builder = LevelBuilder.lastBuilder;
		if (builder) {
			Handles.color = new Color(0, 0, 0, .1f);
			if (Handles.Button(transform.position + new Vector3(0, -1.5f, 0), Quaternion.identity, LevelBuilderCommon.button_size, LevelBuilderCommon.button_size, Handles.SphereHandleCap)) {
				GameObject go = transform.gameObject;
				Undo.DestroyObjectImmediate(go);
			}
		}
	}

	public static void SafetyNetButton(IAction action) {
		Transform transform = (action as MonoBehaviour).GetComponent<Transform>() as Transform;

		LevelBuilder builder = LevelBuilder.lastBuilder;
		if (builder) {
			Handles.color = new Color(255, 0, 255, .5f);
			if (Handles.Button(transform.position + new Vector3(0, 1f, 0), Quaternion.identity, LevelBuilderCommon.button_size, LevelBuilderCommon.button_size, Handles.SphereHandleCap))
				LevelBuilderCommon.BuilderCreateAction(ActionType.SafetyNetForRun, action);
		}
	}

	public static void DirectionalButtons(IAction action, int sidesToRender = -1) {
		Transform transform = (action as MonoBehaviour).GetComponent<Transform>() as Transform;

		LevelBuilder levelBuilder = LevelBuilder.lastBuilder;

		if (levelBuilder != null) {
			if ((sidesToRender & 0x4) != 0)
				LevelBuilderCommon.Default5ActionButton(transform.position + new Vector3(1.5f, 0, 0), action, -1, Direction.Right);
			if ((sidesToRender & 0x2) != 0)
				LevelBuilderCommon.Default5ActionButton(transform.position + new Vector3(-1.5f, 0, 0), action, -1, Direction.Left);
			if ((sidesToRender & 0x1) != 0)
				LevelBuilderCommon.Default5ActionButton(transform.position + new Vector3(0, 0, 1.5f), action, -1, Direction.Forward);
			if ((sidesToRender & 0x8) != 0)
				LevelBuilderCommon.Default5ActionButton(transform.position + new Vector3(0, 0, -1.5f), action, -1, Direction.Backward);
		}
	}

	public static void Default5ActionButton(Vector3 position, IAction action = null, int buttonsToRender = -1, Direction dir = Direction.None) {
		if (LevelBuilderCommon.page == 0) {
			if ((buttonsToRender & 0x1) != 0)
				LevelBuilderCommon.DefaultActionButton(position + new Vector3(-.5f, 0, -.5f), ActionType.Run, action, dir);
			if ((buttonsToRender & 0x2) != 0)
				LevelBuilderCommon.DefaultActionButton(position + new Vector3(-.5f, 0, .5f), ActionType.TriggerRun, action, dir);
			if ((buttonsToRender & 0x4) != 0)
				LevelBuilderCommon.DefaultActionButton(position + new Vector3(.5f, 0, -.5f), ActionType.Translate, action, dir);
			if ((buttonsToRender & 0x8) != 0)
				LevelBuilderCommon.DefaultActionButton(position + new Vector3(.5f, 0, .5f), ActionType.Death, action, dir);
			if ((buttonsToRender & 0x16) != 0)
				LevelBuilderCommon.DefaultActionButton(position + new Vector3(0, 0, 0), ActionType.None, action, dir);
		} else {
			if ((buttonsToRender & 0x32) != 0)
				LevelBuilderCommon.DefaultActionButton(position + new Vector3(-.5f, 0, -.5f), ActionType.Jump, action, dir);
			if ((buttonsToRender & 0x64) != 0)
				LevelBuilderCommon.DefaultActionButton(position + new Vector3(.5f, 0, -.5f), ActionType.Slide, action, dir);
		}
	}

	public static IAction BuilderCreateAction(ActionType type, IAction creatingFrom, Direction dir = Direction.None) {
		LevelBuilder levelBuilder = LevelBuilder.lastBuilder;
		if (levelBuilder)
			return levelBuilder.CreateAction(type, creatingFrom, dir);
		return null;
	}

	public static IAction DefaultActionButton(Vector3 position, ActionType type, IAction action = null, Direction dir = Direction.None) {
		return (LevelBuilderCommon.ActionButton(position, type)) ? LevelBuilderCommon.BuilderCreateAction(type, action, dir) : null;
	}

	public static bool ActionButton(Vector3 position, ActionType type) {
		switch (type) {
			case ActionType.Run: Handles.color = Color.white; break;
			case ActionType.SafetyNetForRun: Handles.color = Color.gray; break;
			case ActionType.TriggerRun: Handles.color = Color.yellow; break;
			case ActionType.Translate: Handles.color = Color.cyan; break;
			case ActionType.Death: Handles.color = Color.black; break;
			case ActionType.Jump: Handles.color = Color.blue; break;
			case ActionType.Slide: Handles.color = new Color(1f, .5f, 0f); break;
			case ActionType.None: Handles.color = Color.red; break;
			default: Handles.color = Color.white; break;
		}
		return LevelBuilderCommon.CreateActionButton(position);
	}

	private static bool CreateActionButton(Vector3 position) {
		return Handles.Button(position, Quaternion.identity, LevelBuilderCommon.button_size, LevelBuilderCommon.button_size, Handles.CubeHandleCap);
	}
}

#endif