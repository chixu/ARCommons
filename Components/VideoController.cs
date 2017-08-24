using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Video;
//-----------------------------------------------------------------------------
// Copyright 2015-2016 RenderHeads Ltd.  All rights reserverd.
//-----------------------------------------------------------------------------

public class VideoController : MonoBehaviour
{
	//public MediaPlayer	_mediaPlayer;
	public Slider _videoSeekSlider;
	private bool _wasPlayingOnScrub;
	private float _setVideoSeekSliderValue;
	public static VideoController instant;
	private string prevPath;
    public VideoPlayer videoPlayer;
	public AudioSource audioSource;
	private bool isDown = false;
	private bool isClick = false;
	private int _currentFrame;
	private bool frameChanged = false;

    //		public void OnMuteChange()
    //		{
    //			if (_mediaPlayer)
    //			{
    //				_mediaPlayer.Control.MuteAudio(_MuteToggle.isOn);
    //			}
    //		}

	public void Play(GameObject obj, string path)
    {
		isDown = false;
		obj.SetActive (false);
        //videoPlayer = obj.GetComponent<VideoPlayer>();
		StartCoroutine(LoadAndPlay(obj, path));
    }

	private IEnumerator LoadAndPlay(GameObject obj, string path){
		_videoSeekSlider.value = 0;
		if (path != prevPath)
		{
			//obj.AddComponent<VideoPlayer>();
			//videoPlayer = obj.GetComponent<VideoPlayer>();
			//videoPlayer.playOnAwake = false;
			//audioSource.playOnAwake = false;
			//audioSource.Pause();
			videoPlayer.source = VideoSource.Url;
			videoPlayer.url = path;

			//videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
			//videoPlayer.controlledAudioTrackCount = 1;
			//videoPlayer.EnableAudioTrack (0, true);
			//videoPlayer.SetTargetAudioSource (0, audioSource);
			//obj.AddComponent<AudioSource>();
			//AudioSource audio = obj.GetComponent<AudioSource>();
			//videoPlayer.SetTargetAudioSource (0, audio);
			videoPlayer.Prepare();
			while (!videoPlayer.isPrepared)
			{
				Logger.Log("Preparing Video " + path);
				yield return null;
			}
			videoPlayer.targetMaterialRenderer = obj.GetComponent<MeshRenderer>();
			audioSource.Play ();
			videoPlayer.Play ();
		}
		else
		{
			VideoController.instant.videoPlayer.targetMaterialRenderer = obj.GetComponent<MeshRenderer>();
			videoPlayer.Play();
		}
		obj.gameObject.SetActive (true);
		_videoSeekSlider.gameObject.SetActive(true);
		prevPath = path;
	}

    //public void OpenAndPlay(string path){
    //	if (prevPath != path) {
    //		_mediaPlayer.OpenVideoFromFile (MediaPlayer.FileLocation.AbsolutePathOrURL, path, true); 
    //		//item.meshRenderer.material = ScanSceneController.instant.videoMaterial;
    //	} else {
    //		_mediaPlayer.Rewind (false);
    //		_mediaPlayer.Play ();
    //	}
    //	prevPath = path;
    //	_videoSeekSlider.gameObject.SetActive(true);

    //}



	public void OnVideoSeekSlider ()
	{
        //if (_mediaPlayer && _videoSeekSlider && _videoSeekSlider.value != _setVideoSeekSliderValue) {
        //	_mediaPlayer.Control.Seek (_videoSeekSlider.value * _mediaPlayer.Info.GetDurationMs ());
        //}
		//if (!isDown)
		//	return;
			//isClick = true;
		int frame = Mathf.FloorToInt(_videoSeekSlider.value * videoPlayer.frameCount);
		if (Mathf.Abs (videoPlayer.frame - frame) > 3) {
			videoPlayer.frame = frame;
			frameChanged = true;
		}
		//Logger.Log("slide " + _videoSeekSlider.value.ToString(),"blue");
	}

	public void OnVideoSliderDown ()
	{
		//if (_mediaPlayer) {
		//	_wasPlayingOnScrub = _mediaPlayer.Control.IsPlaying ();
		//	if (_wasPlayingOnScrub) {
		//		_mediaPlayer.Control.Pause ();
		//		//					SetButtonEnabled( "PauseButton", false );
		//		//					SetButtonEnabled( "PlayButton", true );
		//	}
		//	OnVideoSeekSlider ();
		//}
		isDown = true;
		Logger.Log("down","blue");
	}

	public void OnVideoSliderUp ()
	{
		//if (_mediaPlayer && _wasPlayingOnScrub) {
		//	_mediaPlayer.Control.Play ();
		//	_wasPlayingOnScrub = false;

		//	//				SetButtonEnabled( "PlayButton", false );
		//	//				SetButtonEnabled( "PauseButton", true );
		//}			
		isDown = false;
		Logger.Log("up","blue");
	}

	public void OnRewindButton ()
	{
		//if (_mediaPlayer) {
		//	_mediaPlayer.Control.Rewind ();
		//}
	}

	void Awake ()
	{
		VideoController.instant = this;
		//if (_mediaPlayer) {
		//	_mediaPlayer.Events.AddListener (OnVideoEvent);
		//}
	}

	void Update ()
	{
        //if (_mediaPlayer && _mediaPlayer.Info != null && _mediaPlayer.Info.GetDurationMs () > 0f) {
        //	float time = _mediaPlayer.Control.GetCurrentTimeMs ();
        //	float d = time / _mediaPlayer.Info.GetDurationMs ();
        //	_setVideoSeekSliderValue = d;
        //	_videoSeekSlider.value = d;
        //}
		if (!_videoSeekSlider.gameObject.activeSelf)
			return;

		//Debug.Log("update");


		if (videoPlayer && !isDown) {

			float value = (float)videoPlayer.frame / videoPlayer.frameCount;
			if (Mathf.Abs(value - _videoSeekSlider.value) <= 3f / videoPlayer.frameCount)
				_videoSeekSlider.value = value;
			//_currentFrame = 
		}

    }

	// Callback function to handle events
	//public void OnVideoEvent (MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
	//{
	//	switch (et) {
	//	case MediaPlayerEvent.EventType.ReadyToPlay:
	//		break;
	//	case MediaPlayerEvent.EventType.Started:
	//		break;
	//	case MediaPlayerEvent.EventType.FirstFrameReady:
	//		break;
	//	case MediaPlayerEvent.EventType.FinishedPlaying:
	//		break;
	//	}

	//	Debug.Log ("Event: " + et.ToString ());
	//}

}
