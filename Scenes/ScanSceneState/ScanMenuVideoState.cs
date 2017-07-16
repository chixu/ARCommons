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
using DG.Tweening;

public class ScanMenuVideoState:ScanVideoState{
	private PopMenuItem item;

	public override void OnEnter (Hashtable args = null)
	{
		item = args ["item"] as PopMenuItem;
		//string path = ScanSceneController.currentTrackableObject.GetComponent<CustomTrackableEventHandler>().videoPath;


		item.gameObject.SetActive (true);
		item.transform.DOLocalMove (new Vector3 (0, item.origPosition.y * 2, 0), 0.3f).SetEase (Ease.OutQuad);
		item.transform.DOScale (item.origScale * 2, 0.3f).SetEase (Ease.OutQuad).OnComplete (PlayVideo);
	}

	private void PlayVideo(){
		scene.mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, item.videoPath, true); 
		item.meshRenderer.material = ScanSceneController.instant.videoMaterial;
		VideoController.instant._videoSeekSlider.gameObject.SetActive(true);
		scene.description.gameObject.SetActive (false);
		//scene.mediaPlayer.Rewind(false);
		scene.mediaPlayer.Play ();
	}

	public override void OnExit(){
		if (item)
			item.transform.DOKill ();
		scene.mediaPlayer.Stop ();
	}

	public override void OnBackClick(){

		ScanSceneController.instant.SetState("menu4", new Hashtable(){{"showImmediate", true}});

	}
}
