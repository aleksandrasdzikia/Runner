/*public class Level : MonoBehaviour
{
	private int maxLevels = 4;

	private List<GameObject> points = new List<GameObject>();
	private GameObject lastPoints;
	private int levelHeight = 5;

	public GameObject player;

	public float speedas = 0.5f;

	public bool generateVines = true;
	public bool generateGold = true;

	// Use this for initialization
	void Start() {
		Res.InitResources();
		this.PreLoadLevels();
	}

	void PreLoadLevels() {
		while (this.points.Count < maxLevels)
			this.CreateLevel();
	}

	void CreateLevel() {
		GameObject newPoints = MonoBehaviour.Instantiate(Res.GetRes("points"));
		this.points.Add(newPoints);

		string leftLevel = "l" + Random.Range(3, 5);
		string rightLevel = "r" + Random.Range(3, 5);
		// Debug.Log(leftLevel + " " + leftGold + " " + leftVine + " " + rightLevel + " " + rightGold + " " + rightVine);
		GameObject left = MonoBehaviour.Instantiate(Res.GetLevel(leftLevel));
		GameObject right = MonoBehaviour.Instantiate(Res.GetLevel(rightLevel));

		if (this.generateVines) {
			string leftVine = leftLevel + "v" + Random.Range(0, 3);
			string rightVine = rightLevel + "v" + Random.Range(0, 3);
			GameObject leftVineO = MonoBehaviour.Instantiate(Res.GetVine(leftVine));
			GameObject rightVineO = MonoBehaviour.Instantiate(Res.GetVine(rightVine));
			leftVineO.transform.parent = left.transform;
			rightVineO.transform.parent = right.transform;
		}

		if (this.generateGold) {
			string leftGold = leftLevel + "g" + Random.Range(0, 4);
			string rightGold = rightLevel + "g" + Random.Range(0, 4);
			GameObject leftGoldO = MonoBehaviour.Instantiate(Res.GetGold(leftGold));
			GameObject rightGoldO = MonoBehaviour.Instantiate(Res.GetGold(rightGold));
			leftGoldO.transform.parent = left.transform;
			rightGoldO.transform.parent = right.transform;
		}

		left.transform.parent = newPoints.transform;
		right.transform.parent = newPoints.transform;

		if (this.lastPoints != null) {
			newPoints.transform.position = this.lastPoints.transform.position;
			newPoints.transform.Translate(Vector3.up * levelHeight);
		}

		this.lastPoints = newPoints;
	}

	// Update is called once per frame
	void Update() {
		if (Vector3.Distance(this.player.transform.position, this.lastPoints.transform.position) < this.speedas * this.levelHeight)
			this.CreateLevel();

		if (this.player.transform.position.x < this.points[0].transform.position.x && Vector3.Distance(this.player.transform.position, this.points[0].transform.position) > this.levelHeight * 3f) {
			MonoBehaviour.Destroy(this.points[0]);
			this.points.RemoveAt(0);
		}
	}
}*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
	private int maxLevels = 2;

	private List<GameObject> points = new List<GameObject>();
	private GameObject lastPoints;
	public static int levelHeight = 9;

	public GameObject player;

	private float speedas = 1.5f; //Default 0.3f

    //public bool generateVines = false; ----------------COMENTED
    //public bool generateGold = false; ----------------COMENTED

    // Use this for initialization

    void Start() {
		Res.InitResources();
		this.PreLoadLevels();
	}

	public void reset() {
		foreach (GameObject go in this.points)
			Destroy(go);
		Destroy(this.lastPoints);
		this.lastPoints = null;
		this.points = new List<GameObject>();
		this.PreLoadLevels();
	}

	void PreLoadLevels() {
		while (this.points.Count < maxLevels)
			this.CreateLevel();
	}

	void CreateLevel() {
		GameObject newPointsWrapper = new GameObject();
		GameObject newPoints = MonoBehaviour.Instantiate(Res.GetRes("points"), newPointsWrapper.transform);
		newPoints.transform.parent = newPointsWrapper.transform;
		this.points.Add(newPointsWrapper);

        string centerLevel = "c" + Random.Range(0, 5); //[IMA 0 ir 3]

        GameObject center = MonoBehaviour.Instantiate(Res.GetLevel(centerLevel));
        center.transform.parent = newPoints.transform;

        //string leftLevel = "l" + Random.Range(3, 5); ----------------COMENTED
        //string rightLevel = "r" + Random.Range(3, 5); ----------------COMENTED
        // Debug.Log(leftLevel + " " + leftGold + " " + leftVine + " " + rightLevel + " " + rightGold + " " + rightVine);



        //GameObject left = MonoBehaviour.Instantiate(Res.GetLevel(leftLevel));----------------COMENTED
        //GameObject right = MonoBehaviour.Instantiate(Res.GetLevel(rightLevel));----------------COMENTED

        /*if (this.generateVines) { ----------------COMENTED WHOLE STATEMENT
			string leftVine = leftLevel + "v" + Random.Range(0, 3);
			string rightVine = rightLevel + "v" + Random.Range(0, 3);
			GameObject leftVineO = MonoBehaviour.Instantiate(Res.GetVine(leftVine));
			GameObject rightVineO = MonoBehaviour.Instantiate(Res.GetVine(rightVine));
			leftVineO.transform.parent = left.transform;
			rightVineO.transform.parent = right.transform;
		}

		if (this.generateGold) {  ----------------COMENTED WHOLE STATEMENT
			string leftGold = leftLevel + "g" + Random.Range(0, 4);
			string rightGold = rightLevel + "g" + Random.Range(0, 4);
			GameObject leftGoldO = MonoBehaviour.Instantiate(Res.GetGold(leftGold));
			GameObject rightGoldO = MonoBehaviour.Instantiate(Res.GetGold(rightGold));
			leftGoldO.transform.parent = left.transform;
			rightGoldO.transform.parent = right.transform;
		}
        */



        //left.transform.parent = newPoints.transform;----------------COMENTED
        //right.transform.parent = newPoints.transform;----------------COMENTED

        newPointsWrapper.transform.position = newPoints.transform.position;

		if (this.lastPoints != null) {
			newPointsWrapper.transform.position = this.lastPoints.transform.position;
			newPointsWrapper.transform.Translate(Vector3.up * Level.levelHeight);
		}

		this.lastPoints = newPointsWrapper;
	}

	// Update is called once per frame
	void Update() {
		if (Vector3.Distance(this.player.transform.position, this.lastPoints.transform.position + new Vector3(0, Level.levelHeight, 0)) < this.speedas * Level.levelHeight)
			this.CreateLevel();

		if (this.player.transform.position.x < this.points[0].transform.position.x && Vector3.Distance(this.player.transform.position, this.points[0].transform.position) > Level.levelHeight * 3f) {
			MonoBehaviour.Destroy(this.points[0]);
			this.points.RemoveAt(0);
		}
	}
}

