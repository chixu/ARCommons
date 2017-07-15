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

public class ScanMenuObjectState:ScanObjectState
{
	public override void OnBackClick(){

		ScanSceneController.currentTrackableObject.GetComponent<PopMenu> ().ShowMenu ();
	}	
}
