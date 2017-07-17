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
	private PopMenuItem item;

	public ScanMenuObjectState(){
		name = "menuobject";
	}

	public override void OnEnter (Hashtable args = null)
	{
		item = args ["item"] as PopMenuItem;
		item.threeDObject.SetActive (true);
	}

	public override void OnExit ()
	{
		item.threeDObject.SetActive (false);
	}

	public override void OnBackClick(){

		ScanSceneController.instant.SetState("menu4", new Hashtable(){{"showImmediate", true}});

	}
}
