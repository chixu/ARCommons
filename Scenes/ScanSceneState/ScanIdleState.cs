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

public class ScanIdleState: ScanSceneState
{
	public override void OnEnter (Hashtable args = null)
	{
		//base.OnEnter (args);
		ScanSceneController.currentTrackableObject = null;
		scene.title.text = I18n.Translate (scene.prevSceneName+"_scan_title");
		scene.description.text = I18n.Translate (scene.prevSceneName+"_scan_desc");
		VideoController.instant._videoSeekSlider.gameObject.SetActive(true);
		scene.description.gameObject.SetActive (false);
	}
}
