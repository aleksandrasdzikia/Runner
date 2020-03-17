#if (UNITY_EDITOR)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class LevelBuilder : MonoBehaviour {

	public static LevelBuilder lastBuilder;
	public static float levelUnit = 1f;

	public GameObject newLevel = null;
	public GameObject currentSelection = null;
	public GameObject rootObject = null;

	private GameObject lastSelection = null;
	private bool isCurrentSelectionInLevel = false;
	private List<GameObject> objectsInLevel = new List<GameObject>();

	private bool specialAction;

	// Use this for initialization
	void Start () {
		Debug.Log("starting", this);
		LevelBuilder.lastBuilder = this;
	}

	// Update is called once per frame
	void Update () {
		LevelBuilder.lastBuilder = this;
		this.CheckForLevel();
		this.CheckForRoot();
		this.CheckForSelection(Selection.activeGameObject);
		SceneView.RepaintAll();
	}

	void Reset() {
		if (this.newLevel != null)
			MonoBehaviour.DestroyImmediate(this.newLevel);
		this.newLevel = new GameObject("New Level");
		this.objectsInLevel = new List<GameObject>();
		this.currentSelection = null;
		this.lastSelection = null;
		this.isCurrentSelectionInLevel = false;
	}

	void CheckForLevel() {
		if (this.newLevel == null)
			this.Reset();
	}

	void CheckForRoot() {
		if (this.rootObject != null)
			return;

		if (this.newLevel == null)
			return;

		this.rootObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
		this.rootObject.transform.position = new Vector3(0f, 1.5f * LevelBuilder.levelUnit, 0f);
		this.rootObject.GetComponent<MeshRenderer>().GetComponent<MeshRenderer>().sharedMaterial = AssetDatabase.LoadAssetAtPath("Assets/Materials/Builder/Builder.mat", typeof(Material)) as Material;
		this.rootObject.transform.localScale = new Vector3(.25f, .25f, .25f);
		this.rootObject.name = "Root";
		this.AddToLevel(this.rootObject);
		this.rootObject.AddComponent<LevelBuilderRoot>();
	}

	void CheckForSelection(GameObject selectObject) {
		if (selectObject != this.currentSelection) {
			this.lastSelection = this.currentSelection;
			this.isCurrentSelectionInLevel = selectObject != null && this.objectsInLevel.IndexOf(selectObject) != -1;
		}
		this.currentSelection = selectObject;
	}

	void AddSelectionToLevel() {
		if (this.nosel())
			return;

		int index = this.objectsInLevel.IndexOf(this.currentSelection);
		if (!this.isCurrentSelectionInLevel) {
			this.objectsInLevel.Add(this.currentSelection);
			this.isCurrentSelectionInLevel = true;
		}

		if (this.currentSelection.transform.parent != this.newLevel) {
			this.currentSelection.transform.parent = this.newLevel.transform;
			this.currentSelection.transform.rotation = Quaternion.identity;
		}
	}

	void AddToLevel(GameObject go) {
		if (go.transform.parent != this.newLevel)
			go.transform.parent = this.newLevel.transform;
	}

	void RemoveSelectionFromLevel() {
		if (this.nosel())
			return;

		if (this.isCurrentSelectionInLevel) {
			int index = this.objectsInLevel.IndexOf(this.currentSelection);
			this.objectsInLevel.RemoveAt(index);
			this.isCurrentSelectionInLevel = false;

			if (this.currentSelection.transform.parent == this.newLevel)
				this.currentSelection.transform.parent = null;
		}
		else this.DestroyCurrentSelection();
	}

	void DestroyCurrentSelection() {
		if (this.nosel())
			return;

		MonoBehaviour.DestroyImmediate(this.currentSelection);
		this.currentSelection = null;
		this.isCurrentSelectionInLevel = false;
		// this.CheckForSelection(this.lastSelection);
	}


	void RotateSelection(float angle) {
		this.currentSelection.transform.Rotate(Vector3.up, angle);
	}

	GameObject DuplicateSelection() {
		if (this.noselinl())
			return null;

		GameObject newObject = this.DuplicateObject(this.currentSelection);
		Selection.activeGameObject = newObject;
		return newObject;
	}

	GameObject DuplicateObject(GameObject reference) {
		if (reference == null)
			return null;

		GameObject newObject = Object.Instantiate(reference, this.newLevel.transform);
		newObject.name = reference.name.Substring(0, 3) + this.objectsInLevel.Count;
		this.objectsInLevel.Add(newObject);
		return newObject;
	}

	/* selection? */
	bool sel() {
		return this.currentSelection != null;
	}

	/* selection and in level? */
	bool selinl() {
		return this.sel() && this.isCurrentSelectionInLevel;
	}

	bool nosel() {
		return !this.sel();
	}

	bool noselinl() {
		return !this.selinl();
	}


	private void LinkActions(IAction linkThis, IAction toThis, Direction linkingDirection = Direction.None) {
		if (linkThis is ActionRun)
			(linkThis as ActionRun).LinkTo((toThis as MonoBehaviour).gameObject, linkingDirection);
		if (linkThis is ActionTranslate)
			(linkThis as ActionTranslate).LinkTo((toThis as MonoBehaviour).gameObject);
		if (linkThis is ActionSlide && toThis is ActionSlide) {
			ActionSlide slideRef = linkThis as ActionSlide;
			slideRef.slideEnd.Add((toThis as MonoBehaviour).gameObject);
			slideRef.slideStart = true;
			(toThis as ActionSlide).slideStart = false;
		}
		if (linkThis is ActionJump) {
			(linkThis as ActionJump).LinkTo((toThis as MonoBehaviour).gameObject);
		}
	}

	public IAction CreateAction(ActionType type, IAction actionRef = null, Direction actionDirection = Direction.None) {
		if (type == null)
			return null;

		GameObject actionObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
		actionObject.AddComponent<BuildingBlock>();
		actionObject.name = type.ToString();
		this.AddToLevel(actionObject);

		actionObject.transform.position = Vector3.zero;
		actionObject.transform.rotation = Quaternion.identity;

		IAction ret = null;
		switch (type) {
			case ActionType.Run: {
				actionObject.GetComponent<MeshRenderer>().sharedMaterial = AssetDatabase.LoadAssetAtPath("Assets/Materials/Builder/Run.mat", typeof(Material)) as Material;
				ret = actionObject.AddComponent<ActionRun>();
				break;
			}
			case ActionType.SafetyNetForRun: {
				actionObject.GetComponent<MeshRenderer>().sharedMaterial = AssetDatabase.LoadAssetAtPath("Assets/Materials/Builder/SafetyNetForRun.mat", typeof(Material)) as Material;
				ret = actionObject.AddComponent<ActionSafetyNetForRun>();
				(ret as ActionSafetyNetForRun).actionRun = (ActionRun) actionRef;
				((ActionRun) actionRef).safetyNet = actionObject;
				actionObject.transform.localScale = ActionSafetyNetForRun.boxSize;
				break;
			}
			case ActionType.Death: {
				actionObject.GetComponent<MeshRenderer>().sharedMaterial = AssetDatabase.LoadAssetAtPath("Assets/Materials/Builder/Death.mat", typeof(Material)) as Material;
				ret = actionObject.AddComponent<ActionDeath>();
				break;
			}
			case ActionType.Slide: {
				actionObject.GetComponent<MeshRenderer>().sharedMaterial = AssetDatabase.LoadAssetAtPath("Assets/Materials/Builder/Slide.mat", typeof(Material)) as Material;
				ret = actionObject.AddComponent<ActionSlide>();
				actionObject.transform.localScale = new Vector3(.25f, .25f, .25f);
				break;
			}
			case ActionType.Jump: {
				actionObject.GetComponent<MeshRenderer>().sharedMaterial = AssetDatabase.LoadAssetAtPath("Assets/Materials/Builder/Jump.mat", typeof(Material)) as Material;
				ret = actionObject.AddComponent<ActionJump>();
				break;
			}
			case ActionType.TriggerRun: {
				actionObject.GetComponent<MeshRenderer>().sharedMaterial = AssetDatabase.LoadAssetAtPath("Assets/Materials/Builder/TriggerRun.mat", typeof(Material)) as Material;
				ret = actionObject.AddComponent<ActionTriggerRun>();
				break;
			}
			case ActionType.Translate: {
				actionObject.GetComponent<MeshRenderer>().sharedMaterial = AssetDatabase.LoadAssetAtPath("Assets/Materials/Builder/Translate.mat", typeof(Material)) as Material;
				ret = actionObject.AddComponent<ActionTranslate>();
				break;
			}
			case ActionType.None: {
				actionObject.GetComponent<MeshRenderer>().sharedMaterial = AssetDatabase.LoadAssetAtPath("Assets/Materials/Builder/Direction.mat", typeof(Material)) as Material;
				ret = actionObject.AddComponent<ActionNone>();
				actionObject.transform.localScale = new Vector3(.25f, .25f, .25f);
				break;
			}
			default:
				break;
		}

		if (actionRef != null) {
			if (ret != null)
				this.LinkActions(actionRef, ret, actionDirection);
			actionObject.transform.position = (actionRef as MonoBehaviour).transform.position + DirectionVector.getVector(actionDirection);
		}

		Selection.activeGameObject = actionObject;
		return ret;
	}
}

#endif