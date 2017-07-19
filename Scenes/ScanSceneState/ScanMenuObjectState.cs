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

public class ScanMenuObjectState:ScanSceneState
{
	private PopMenuItem item;
	private AudioSource audio;

	public ScanMenuObjectState(){
		name = "menuobject";
	}

	public override void OnEnter (Hashtable args = null)
	{
		item = args ["item"] as PopMenuItem;
		ScanSceneController.instant.subtitle.Play (item.subtitlePath);
		item.threeDObject.SetActive (true);
		audio = item.threeDObject.GetComponent<AudioSource> ();
		if (audio != null)
			audio.Play ();
	}

	public override void OnExit ()
	{
		if (audio != null)
			audio.Stop ();
		item.threeDObject.SetActive (false);
		base.OnExit ();
	}

	public override void OnBackClick(){

		ScanSceneController.instant.SetState("menu4", new Hashtable(){{"showImmediate", true}});

	}
}
