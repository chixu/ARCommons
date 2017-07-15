 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using Vuforia;
using RenderHeads.Media.AVProVideo;
using UnityEngine.SceneManagement;
using System.Xml.Linq;

public class ScanSceneController : MonoBehaviour
{
	//public string remoteUrl = "";
	public string fileName = "myassets.dlc";
	//public string configName = "config.json";
	//public Text text;
	public MediaPlayer mediaPlayer;
	public Material videoMaterial;
	public Text title;
	public Text description;
	public GameObject planePrefab;
	public static ScanSceneController instant;
	public static GameObject currentTrackableObject;
	public ScanSceneState state;

	//	private Config localConfig;
	//	private Config remoteConfig;
	private Mappings mappings;
	private string dataSetName = "trackings.xml";
	private Dictionary<string, UnityEngine.Object> loadedAssets;
	private Dictionary<string, string> ConfigDict = new Dictionary<string, string> ();
	public string prevSceneName;
	private XElement itemInfos;


	[System.Serializable]
	public class Config
	{
		public string version = "";
		public string[] files;
	}


	[System.Serializable]
	public struct Mappings
	{
		[System.Serializable]
		public struct Mapping
		{
			public string key;
			public string name;
		}

		public Mapping[] mappings;
	}


	string GetAssetsPath (string str, bool isFile = false)
	{
		return (isFile ? "file:///" : "") + Application.persistentDataPath + "/" + prevSceneName + "/" + str;
	}

	IEnumerator StartGame ()
	{
		loadedAssets = new Dictionary<string, UnityEngine.Object> ();

		SetState ("idle");

		yield return Request.ReadPersistent (prevSceneName + "/iteminfos.xml", str => itemInfos = XDocument.Parse (str).Root);

		AssetBundle bundle = null;
		if (!AssetBundleManager.bundles.ContainsKey (prevSceneName)) {
			WWW www = new WWW (GetAssetsPath (fileName, true));
			yield return www;
			if (www.error == null) {
				bundle = www.assetBundle;
				AssetBundleManager.bundles.Add (prevSceneName, bundle);
			}
		} else {
			bundle = AssetBundleManager.bundles [prevSceneName];
		}
		if (bundle != null) {
			string[] assetNames;
			try {
				assetNames = bundle.GetAllAssetNames ();
				foreach (string name in assetNames) {
					string simpleName = Path.GetFileNameWithoutExtension (name);
					//Log (simpleName);
					//Instantiate(bundle.LoadAsset());
					loadedAssets.Add (simpleName, bundle.LoadAsset (simpleName));
				}
//				Log ("===================scenePath==============");
//				string[] scenePath = bundle.GetAllScenePaths();
//				foreach (string path in scenePath) {
//					Log (path);
//				}

			} catch (Exception e) {
				//Log (e.StackTrace);
			}

		}
		StartCoroutine(LoadDataSet ());
	}

	public void SetState(string name, Hashtable args = null){
		Logger.Log ("scene: " + name, "green");
		if(state!=null)state.OnExit ();
		state = ScanSceneState.GetState (name);
		state.scene = this;
		state.OnEnter (args);
	}

	void Awake(){
		instant = this;
		prevSceneName = SceneManagerExtension.GetSceneArguments () ["name"].ToString ();

	}

	void Start ()
	{
		StartCoroutine (StartGame ());
	}

	IEnumerator LoadDataSet ()
	{

		ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker> ();

		DataSet dataSet = objectTracker.CreateDataSet ();
		//Debug.Log (Application.persistentDataPath + "/" + dataSetName);
		if (dataSet.Load (GetAssetsPath (dataSetName), VuforiaUnity.StorageType.STORAGE_ABSOLUTE)) {
			//if (dataSet.Load (dataSetName)) {             
			objectTracker.Stop ();  // stop tracker so that we can add new dataset

			if (!objectTracker.ActivateDataSet (dataSet)) {
				// Note: ImageTracker cannot have more than 100 total targets activated
				Debug.Log ("<color=yellow>Failed to Activate DataSet: " + dataSetName + "</color>");
			}

			if (!objectTracker.Start ()) {
				Debug.Log ("<color=yellow>Tracker Failed to Start.</color>");
			}

			//int counter = 0;

			IEnumerable<TrackableBehaviour> tbs = TrackerManager.Instance.GetStateManager ().GetTrackableBehaviours ();
			foreach (TrackableBehaviour tb in tbs) {



				//if (loadedAssets.ContainsKey (tb.TrackableName)) {
				//Log ("DynamicImageTarget-" + tb.TrackableName);
				//				if (tb.name == "New Game Object") {
				//				 
				//					// change generic name to include trackable name
				tb.gameObject.name = "AR-" + tb.TrackableName;

				XElement info = Xml.GetChildByAttribute (itemInfos, "id", tb.TrackableName);
				if (info == null)
					continue;
				string objType = Xml.Attribute (info, "type");
				//				 
				//					// add additional script components for trackable
				tb.gameObject.AddComponent<DefaultTrackableEventHandler> ();
				tb.gameObject.AddComponent<CustomTrackableEventHandler> ();
				tb.gameObject.AddComponent<TurnOffBehaviour> ();
				CustomTrackableEventHandler cte = tb.gameObject.GetComponent<CustomTrackableEventHandler> ();
				cte.type = objType;
				UnityEngine.Object asset = null;
				if (objType == "object") {
					asset = loadedAssets [tb.TrackableName];
				} else if (objType == "video"){
					asset = planePrefab;
					Renderer render = (planePrefab).GetComponent<Renderer> ();
					render.material = videoMaterial;

					cte.videoPath = GetAssetsPath (Xml.Attribute(info, "videosrc"));
					//cte.mediaPlayer = mediaPlayer;
				}	else if (objType == "menu4"){
					//asset = planePrefab;
					Renderer render = (planePrefab).GetComponent<Renderer> ();
					render.material = videoMaterial;
					//CustomTrackableEventHandler cte = tb.gameObject.GetComponent<CustomTrackableEventHandler> ();
					//cte.videoPath = GetAssetsPath (tb.TrackableName + ".mp4");
					cte.mediaPlayer = mediaPlayer;


					tb.gameObject.AddComponent<PopMenu> ();
					PopMenu popmenu = tb.gameObject.GetComponent<PopMenu> ();
					popmenu.menuItems = new List<PopMenuItem> ();
					popmenu.playerMateral = videoMaterial;

					var menuNodes = info.Elements ();
					//XElement res = null;
					int index = 0;
					foreach(XElement n in menuNodes){
						GameObject planeItem = GameObject.Instantiate(Resources.Load("Prefabs/PlaneItem4")) as GameObject;
						PopMenuItem pmi  = planeItem.GetComponent<PopMenuItem> ();
						popmenu.menuItems.Add (pmi);
						pmi.floatSpeed = 5f;
						pmi.floatAmplitude = 0.03f;
						pmi.index = index;
						pmi.menu = popmenu;
						pmi.trackableHandler = cte;
						planeItem.transform.SetParent (tb.gameObject.transform, false);
						Vector3 position = planeItem.transform.localPosition;
						if (index == 1) {
							planeItem.transform.localPosition = position.SetX (-position.x);
						} else if (index == 2) {
							planeItem.transform.localPosition = position.SetZ (-position.z);
						} else if (index == 3) {
							planeItem.transform.localPosition = new Vector3 (-position.x, position.y, -position.z);
						}
						pmi.Initiate ();

						string itemSrc = Xml.Attribute (n, "src");
						string videoPath = Xml.Attribute (n, "videosrc");
						if(!string.IsNullOrEmpty(videoPath))
							pmi.videoPath = GetAssetsPath(videoPath);

						WWW www = new WWW (GetAssetsPath (itemSrc, true));
						yield return www;
						Logger.Log (GetAssetsPath (itemSrc, true) + " " + www.texture.ToString ());
						pmi.material.mainTexture = www.texture;
						//Logger.Log (planeItem.transform.localPosition.x.ToString() + " " +planeItem.transform.localPosition.y+ " " + planeItem.transform.localPosition.z, "blue");
						index++;
					}

				}
				//GameObject obj = (GameObject)GameObject.Instantiate (asset);
				if (asset != null) {
					GameObject prefab = asset as GameObject;
					GameObject obj = (GameObject)GameObject.Instantiate (prefab, prefab.transform.position, prefab.transform.rotation);

					obj.transform.SetParent (tb.gameObject.transform, false);
					ApplyItemInfo (obj, Xml.GetChildByAttribute (itemInfos, "id", tb.TrackableName));
				}
				//obj.gameObject.SetActive (true);
			}
		} else {
			Debug.LogError ("<color=yellow>Failed to load dataset: '" + dataSetName + "'</color>");
		}
	}

	void ApplyItemInfo(GameObject obj, XElement info){
//		if (itemInfos == null)
//			return;
//		XElement info = Xml.GetChildByAttribute (itemInfos, "id", name);
		if (info == null)
			return;
		Vector3 scale = obj.transform.localScale;
		Vector3 pos = obj.transform.localPosition;
		obj.transform.localScale = new Vector3 (scale.x * Xml.Float (info, "scalex", 1), scale.y * Xml.Float (info, "scaley", 1), scale.z * Xml.Float (info, "scalez", 1));
		obj.transform.localPosition = new Vector3 (pos.x + Xml.Float (info, "x"), pos.y + Xml.Float (info, "y"), pos.z + Xml.Float (info, "z"));
	}


	public void OnBackClick ()
	{
		state.OnBackClick ();
	}
	//	void Log (string str)
	//	{
	//		if (!String.IsNullOrEmpty (str))
	//			text.text += "\n" + str;
	//	}
//	void Update(){
//		description.gameObject.SetActive (!VideoController.instant._videoSeekSlider.gameObject.activeSelf);
//	}
}
