﻿ using System.Collections;
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

public class ScanVideoState:ScanSceneState{

	public ScanVideoState(){
		name = "video";
	}
		
	public override void OnEnter (Hashtable args = null)
	{
		//base.OnEnter (args);
		//scene.title.text = I18n.Translate (scene.prevSceneName+"_scan_title");
		//scene.description.text = I18n.Translate (scene.prevSceneName+"_scan_desc");
		base.OnEnter(args);
		string path = ScanSceneController.currentTrackableObject.GetComponent<CustomTrackableEventHandler>().videoPath;
		//scene.mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, path, true); 
		//VideoController.instant._videoSeekSlider.gameObject.SetActive(true);
		//scene.mediaPlayer.Rewind(false);
		//scene.mediaPlayer.Play ();
		//VideoController.instant._videoSeekSlider.gameObject.SetActive(true);
		VideoController.instant.OpenAndPlay(path);
		scene.description.gameObject.SetActive (false);
	}

	public override void OnExit(){
		base.OnExit ();
		scene.mediaPlayer.m_AutoStartTriggered = true;
		scene.mediaPlayer.Stop ();
	} 
}
