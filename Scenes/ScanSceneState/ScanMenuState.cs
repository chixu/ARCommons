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

public class ScanMenuState:ScanSceneState{

	public override void OnEnter (Hashtable args = null)
	{
		scene.title.text = I18n.Translate (scene.prevSceneName+"_scan_title");
		scene.description.text = I18n.Translate (scene.prevSceneName+"_scan_desc");
		PopMenu menu = ScanSceneController.currentTrackableObject.GetComponent<PopMenu> ();
		bool showImmediate = args != null;
		if (showImmediate)
			menu.ShowMenu ();
		else
			menu.Show ();
	}

	public override void OnExit(){
		PopMenu menu = ScanSceneController.currentTrackableObject.GetComponent<PopMenu> ();
		menu.Hide ();
	}
}
