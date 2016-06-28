using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class drag : MonoBehaviour {



	public Camera mainCamera;

	public Vector3 lastPosition;

	public Vector3 currentPosition;

	public bool isMoving = false;

	public Text text;


	// Use this for initialization
	void Start () {

		StartCoroutine (testWWW());
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator testWWW(){


		WWW www = new WWW ("http://60.250.133.80/3Doll/1/test3D.php");

		yield return www;

		//text.text = www.text;

		//Debug.Log (www.text);

	}	

	void OnMouseDown(){

		lastPosition = new Vector3(Input.mousePosition.x, 1, Input.mousePosition.y);

		if(Input.GetMouseButtonDown(0))
		{
			isMoving = true;

			Debug.Log("to true");
		}

		Debug.Log ("enter");

	}

	void OnMouseUp(){


		isMoving = false;
	}

	void OnMouseDrag(){

		if(isMoving == true) {

			currentPosition = new Vector3 (Input.mousePosition.x, 1, Input.mousePosition.y);
			Vector3 curPosition = currentPosition - lastPosition;

			lastPosition = currentPosition;

			mainCamera.transform.RotateAround (Vector3.zero, Vector3.up, 10 * curPosition.x * Time.deltaTime);
			Debug.Log ("dragging");
			if (Input.GetMouseButtonUp (0)) {
				isMoving = false;

				Debug.Log("to false");
			}
		}

	}
}
