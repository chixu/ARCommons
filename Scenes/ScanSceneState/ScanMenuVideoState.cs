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

public class ScanMenuVideoState:ScanVideoState{

//	public override void OnEnter (Hashtable args = null)
//	{
//		base.OnEnter (args);
//
//	}

	public override void OnBackClick(){

		ScanSceneController.currentTrackableObject.GetComponent<PopMenu> ().ShowMenu ();
	}
}
