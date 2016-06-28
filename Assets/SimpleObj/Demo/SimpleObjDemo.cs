/* SimpleOBJ 1.4                        */
/* august 18, 2015                      */
/* By Orbcreation BV                    */
/* Richard Knol                         */
/* info@orbcreation.com                 */
/* games, components and freelance work */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;

public class SimpleObjDemo : MonoBehaviour {
	public GameObject cameraGameObject;
	public Texture2D defaultTexture;
	public GameObject rulerIndicatorPrototype;
	public Color[] demoColors;

	public Text text;

	private string url = "http://orbcreation.com/SimpleObj/Ayumi.obj";

//	Use this when you want to use your own local files
//  	private string url = "file:///ontwikkel/AssetStore/SimpleObj/objfiles/teapot2.obj";

	private string downloadedPath = "";
	private float importScale = 0.2f;
	private float importedScale = 10f;
	private Vector3 importTranslation = new Vector3(0,0,0);
	private Vector3 importedTranslation = new Vector3(0,0,0);
	private Vector3 importRotation = new Vector3(0,0,0);
	private Vector3 importedRotation = new Vector3(0,0,0);
	private bool gameObjectPerGroup = false;
	private bool subMeshPerGroup = false;
	private bool usesRightHanded = true;

	private string logMsgs = "";

	private GameObject targetObject;
	private Bounds overallBounds;
	private float cameraMovement = 0f;

	private GameObject body;
	private Vector3 bodyPosition;
	Quaternion bodyQuate = Quaternion.identity;
	string bodyURL;

	private GameObject head;
	private Vector3 headPosition;
	Quaternion headQuate = Quaternion.identity;
	string headURL;

	private GameObject hair;
	private Vector3 hairPosition;
	Quaternion hairQuate = Quaternion.identity;
	string hairURL;


	private string modelInfo = "";
	private GUIStyle rightAligned = null;
	private int screenshotCounter = 0;

	void Start() {
		overallBounds = new Bounds(Vector3.zero, Vector3.zero);



		Application.ExternalCall( "InitUnity", "Init" );

		// set up ruler
		//Init ("http://60.250.133.80/3Doll/head/me_151221.obj|http://60.250.133.80/3Doll/hair/MelHair001L.obj|http://60.250.133.80/3Doll/1/BasketballPoseA.obj|17.36/0/0|0/17.68/0.24|17.36/0/0|0/17.68/0.24");
	}

	void Update() {
		// slowly rotate model

	}

	public void Init(string param){

		string[] informs = param.Split('|');
		headURL = informs [0];
		hairURL = informs [1];
		bodyURL = informs [2];

		string[] headR = informs [3].Split('/');
		float x = float.Parse (headR[0]);
		float y = float.Parse (headR[1]);
		float z = float.Parse (headR[2]);
		headQuate.eulerAngles = new Vector3( x , y, z);
		string[] hairR = informs [5].Split('/');

		x = float.Parse (hairR[0]);
		y = float.Parse (hairR[1]);
		z = float.Parse (hairR[2]);
		hairQuate.eulerAngles = new Vector3(x, y, z);

		string[] headP = informs [4].Split('/');
		x = float.Parse (headP[0]);
		y = float.Parse (headP[1]);
		z = float.Parse (headP[2]);

		headPosition = new Vector3 (x*0.2f,y*0.2f,z*0.2f);
		string[] hairP = informs [6].Split('/');
		x = float.Parse (hairP[0]);
		y = float.Parse (hairP[1]);
		z = float.Parse (hairP[2]);
		hairPosition = new Vector3 (x*0.2f,y*0.2f,z*0.2f);


		StartCoroutine(GetALL());

	}

	public void GetHair(string param){

		text.text = "GetHair Start";
		
		string[] informs = param.Split('|');
		hairURL = informs [0];
		
		
		StartCoroutine(IEHair());
		
	}

	public void GetBody(string param){

		text.text = "Body Start";
		
		string[] informs = param.Split('|');
		bodyURL = informs [0];

		string[] headR = informs [1].Split('/');
		float x = float.Parse (headR[0]);
		float y = float.Parse (headR[1]);
		float z = float.Parse (headR[2]);
		headQuate.eulerAngles = new Vector3( x , y, z);
		string[] hairR = informs [3].Split('/');
		
		x = float.Parse (hairR[0]);
		y = float.Parse (hairR[1]);
		z = float.Parse (hairR[2]);
		hairQuate.eulerAngles = new Vector3(x, y, z);
		
		string[] headP = informs [2].Split('/');
		x = float.Parse (headP[0]);
		y = float.Parse (headP[1]);
		z = float.Parse (headP[2]);
		
		headPosition = new Vector3 (x*0.2f,y*0.2f,z*0.2f);
		string[] hairP = informs [4].Split('/');
		x = float.Parse (hairP[0]);
		y = float.Parse (hairP[1]);
		z = float.Parse (hairP[2]);
		hairPosition = new Vector3 (x*0.2f,y*0.2f,z*0.2f);
		
		
		StartCoroutine(IEBody());
		
	}

	private IEnumerator IEHair(){
		yield return StartCoroutine(DownloadAndImportFile(hairURL, Quaternion.Euler(importRotation), new Vector3(importScale, importScale, importScale), importTranslation, gameObjectPerGroup, subMeshPerGroup, usesRightHanded,false));

		Destroy (hair);

		hair = targetObject;
		hair.transform.localPosition = hairPosition;
		hair.transform.rotation = hairQuate;


		text.text = "GetHair fin";
	}

	private IEnumerator IEBody(){
		yield return StartCoroutine(DownloadAndImportFile(bodyURL, Quaternion.Euler(importRotation), new Vector3(importScale, importScale, importScale), importTranslation, gameObjectPerGroup, subMeshPerGroup, usesRightHanded,false));
		Destroy (body);
		body = targetObject;
		body.transform.localPosition = new Vector3 (0, 0, 0);

		hair.transform.localPosition = hairPosition;
		hair.transform.rotation = hairQuate;
		head.transform.localPosition = headPosition;
		head.transform.rotation = headQuate;


		text.text = "GetBody fin";
	}




	private IEnumerator GetALL(){

		Debug.Log (bodyURL);
		Debug.Log (hairURL);
		Debug.Log (headURL);

		yield return StartCoroutine(DownloadAndImportFile(bodyURL, Quaternion.Euler(importRotation), new Vector3(importScale, importScale, importScale), importTranslation, gameObjectPerGroup, subMeshPerGroup, usesRightHanded,false));

		body = targetObject;
		body.transform.localPosition = new Vector3 (0, 0, 0);

		Debug.Log ("fin");

		yield return StartCoroutine(DownloadAndImportFile(headURL, Quaternion.Euler(importRotation), new Vector3(importScale, importScale, importScale), importTranslation, gameObjectPerGroup, subMeshPerGroup, usesRightHanded,false)); 

		head = targetObject;
		head.transform.localScale = new Vector3 (0.011f,0.011f,0.011f);
		head.transform.localPosition = headPosition;
		head.transform.rotation = headQuate;

		yield return StartCoroutine(DownloadAndImportFile(hairURL, Quaternion.Euler(importRotation), new Vector3(importScale, importScale, importScale), importTranslation, gameObjectPerGroup, subMeshPerGroup, usesRightHanded,false));
	
		hair = targetObject;
		hair.transform.localPosition = hairPosition;
		hair.transform.rotation = hairQuate;

		text.text = "init fin";
	}

	/* ------------------------------------------------------------------------------------- */
	/* ------------------------------- Downloading files  ---------------------------------- */

	private IEnumerator DownloadAndImportFile(string url, Quaternion rotate, Vector3 scale, Vector3 translate, bool gameObjectPerGrp, bool subMeshPerGrp, bool usesRightHanded , bool head) {
		string objString = null;
		string mtlString = null;
		Hashtable textures = null;

		yield return StartCoroutine (DownloadFile (url, retval => objString = retval));
		yield return StartCoroutine (DownloadFile (url.Substring (0, url.Length - 4) + ".mtl", retval => mtlString = retval));
		if (mtlString != null && mtlString.Length > 0) {
			string path = url;
			int lastSlash = path.LastIndexOf ('/', path.Length - 1);
			if (lastSlash >= 0)
				path = path.Substring (0, lastSlash + 1);
			Hashtable[] mtls = ObjImporter.ImportMaterialSpecs (mtlString);
			for (int i=0; i<mtls.Length; i++) {
				if (mtls [i].ContainsKey ("mainTexName")) {
					Texture2D texture = null;
					string texUrl = path + mtls [i] ["mainTexName"];
					yield return StartCoroutine (DownloadTexture (texUrl, retval => texture = retval));
					if (texture != null) {
						if (textures == null)
							textures = new Hashtable ();
						textures [mtls [i] ["mainTexName"]] = texture;
					}
				}
			}
		}

		if (objString != null && objString.Length > 0) {
			yield return StartCoroutine (ObjImporter.ImportInBackground (objString, mtlString, textures, rotate, scale, translate, retval => targetObject = retval, gameObjectPerGrp, subMeshPerGrp, usesRightHanded));
//			targetObject = ObjImporter.Import(objString, mtlString, textures, rotate, scale, translate);
			AddToLog ("Done importing model");
			if (targetObject != null) {
				if (mtlString == null || mtlString.Length <= 0) {
					SetDftTextureInAllMaterials (targetObject, defaultTexture);
					SetDftColorInAllMaterials (targetObject, defaultTexture);
				}
				// rename the object if needed
				if (targetObject.name == "Imported OBJ file") {
					string[] path = url.Split (new char[] {'/', '.'});
					if (path.Length > 1)
						targetObject.name = path [path.Length - 2];
				}

				// place the bottom on the floor
				overallBounds = GetBounds (targetObject);
				targetObject.transform.position = new Vector3 (0, overallBounds.min.y * -1f, 0);

				modelInfo = GetModelInfo (targetObject, overallBounds);

				Debug.Log ("tttt");

				MeshRenderer mt = targetObject.GetComponent<MeshRenderer> ();
				//ReverseNormals rn = targetObject.AddComponent<ReverseNormals>();

				if (mt != null) {

					mt.sharedMaterial.shader = Shader.Find ("Standard");
					mt.sharedMaterial.shader = Shader.Find ("Mobile/Bumped Diffuse");
					Debug.Log ("material");
				}
			}
		}

		if (head == true) {


		} else {



		}
	}

	private IEnumerator DownloadFile(string url, System.Action<string> result) {
		AddToLog("Downloading "+url);
        WWW www = new WWW(url);
        yield return www;
        if(www.error!=null) {
        	AddToLog(www.error);
        } else {
        	AddToLog("Downloaded "+www.bytesDownloaded+" bytes");
        }
       	result(www.text);
	}
	private IEnumerator DownloadTexture(string url, System.Action<Texture2D> result) {
		AddToLog("Downloading "+url);
        WWW www = new WWW(url);
        yield return www;
        if(www.error!=null) {
        	AddToLog(www.error);
        } else {
        	AddToLog("Downloaded "+www.bytesDownloaded+" bytes");
        }
       	result(www.texture);
	}

	private void SetDftTextureInAllMaterials(GameObject go, Texture2D texture) {
		if(go!=null) {
			Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
	        foreach (Renderer r in renderers) {
	        	foreach (Material m in r.sharedMaterials) {
	        		m.mainTexture = texture;
	        	}
	        }
	    }
	}

	private void SetDftColorInAllMaterials(GameObject go, Texture2D texture) {
		int i=0;
		Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers) {
        	foreach (Material m in r.sharedMaterials) {
        		m.color = demoColors[i++%demoColors.Length];
        	}
        }
	}

	private string GetModelInfo(GameObject go, Bounds bounds) {
		string infoString = "";
		int meshCount = 0;
		int subMeshCount = 0;
		int vertexCount = 0;
		int triangleCount = 0;

		MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>();
		if(meshFilters!=null) meshCount = meshFilters.Length;
        foreach (MeshFilter mf in meshFilters) {
        	Mesh mesh = mf.mesh;
        	subMeshCount += mesh.subMeshCount;
        	vertexCount += mesh.vertices.Length;
        	triangleCount += mesh.triangles.Length / 3;
        }
        infoString = infoString + meshCount + " mesh(es)\n";
        infoString = infoString + subMeshCount + " sub meshes\n";
        infoString = infoString + vertexCount + " vertices\n";
        infoString = infoString + triangleCount + " triangles\n";
        infoString = infoString + bounds.size + " meters";
        return infoString;
	}
	/* ------------------------------------------------------------------------------------- */


	/* ------------------------------------------------------------------------------------- */
	/* --------------------- Position camera to include entire model ----------------------- */
	private Bounds GetBounds(GameObject go) {
		Bounds goBounds = new Bounds(Vector3.zero, Vector3.zero);
		Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers) {
			Bounds bounds = r.bounds;
			goBounds.Encapsulate(bounds);
        }
        return goBounds;
    }

//	private void ResetCameraPosition() {
//		Vector3 camPos = new Vector3(0, 0, cameraGameObject.GetComponent<Camera>().nearClipPlane * -1.5f);
//		camPos.y = (overallBounds.size.magnitude)*0.3f;
//		if(camPos.y<=0f) camPos.y = cameraGameObject.GetComponent<Camera>().nearClipPlane * 1.5f;
//		cameraGameObject.transform.position = camPos;
//		cameraMovement = 0f;
//		QualitySettings.shadowDistance = 10f;
//	}

	private void PositionCameraToShowTargetObject() {
		if(cameraGameObject!=null && targetObject!=null) {
			cameraGameObject.transform.rotation = Quaternion.LookRotation(targetObject.transform.TransformPoint(overallBounds.center) - cameraGameObject.transform.position);
			Vector3 p1 = cameraGameObject.GetComponent<Camera>().WorldToViewportPoint(targetObject.transform.TransformPoint(overallBounds.min)); 
			Vector3 p2 = cameraGameObject.GetComponent<Camera>().WorldToViewportPoint(targetObject.transform.TransformPoint(overallBounds.max));
			float diff = 0f;
			if(p1.z<0f) diff = Mathf.Max(p1.z*-0.04f, diff);
			if(p2.z<0f) diff = Mathf.Max(p2.z*-0.04f, diff);
			if(p1.x<0.05f) diff = Mathf.Max(0.05f-p1.x, diff);
			if(p2.x<0.05f) diff = Mathf.Max(0.05f-p2.x, diff);
			if(p1.x>0.95f) diff = Mathf.Max(p1.x-0.95f, diff);
			if(p2.x>0.95f) diff = Mathf.Max(p2.x-0.95f, diff);
			if(p1.y<0.05f) diff = Mathf.Max(0.05f-p1.y, diff);
			if(p2.y<0.05f) diff = Mathf.Max(0.05f-p2.y, diff);
			if(p1.y>0.95f) diff = Mathf.Max(p1.y-0.95f, diff);
			if(p2.y>0.95f) diff = Mathf.Max(p2.y-0.95f, diff);
			if(diff>0f) {
				cameraMovement += diff * (overallBounds.size.magnitude) * 0.1f * Time.deltaTime;
				Vector3 camPos = cameraGameObject.transform.position;
				camPos.z -= cameraMovement;
				cameraGameObject.transform.position = camPos;

				QualitySettings.shadowDistance = Mathf.Max(10f, camPos.z * -2.2f);
			} else cameraMovement=0f;
		}
	}

	/* ------------------------------------------------------------------------------------- */


	/* ------------------------------------------------------------------------------------- */
	/* ------------------------------- Logging functions  ---------------------------------- */

	private void AddToLog(string msg) {
		Debug.Log(msg+"\n"+DateTime.Now.ToString("yyy/MM/dd hh:mm:ss.fff"));

		// for some silly reason the Editor will generate errors if the string is too long
		int lenNeeded = msg.Length + 1;
		if(logMsgs.Length + lenNeeded>4096) logMsgs = logMsgs.Substring(0,4096-lenNeeded);

		logMsgs = logMsgs + "\n" + msg;
	}

    private string TruncateStringForEditor(string str) {
    	// for some silly reason the Editor will generate errors if the string is too long
		if(str.Length>4096) str = str.Substring(0,4000)+"\n .... display truncated ....\n";
		return str;
    }
	/* ------------------------------------------------------------------------------------- */

	// To make the screenshots used for the Asset Store submission
	private IEnumerator Screenshot() {
		yield return new WaitForEndOfFrame(); // wait for end of frame to include GUI

		Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		screenshot.Apply(false);

		if(Application.platform==RuntimePlatform.OSXPlayer || Application.platform==RuntimePlatform.WindowsPlayer && Application.platform!=RuntimePlatform.LinuxPlayer || Application.isEditor) {
			byte[] bytes = screenshot.EncodeToPNG();
			FileStream fs = new FileStream("Screenshot"+screenshotCounter+".png", FileMode.OpenOrCreate);
			BinaryWriter w = new BinaryWriter(fs);
			w.Write(bytes);
			w.Close();
			fs.Close();
		}
		screenshotCounter++;

	}


}

