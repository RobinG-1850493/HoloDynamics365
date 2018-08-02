using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoBehaviour : MonoBehaviour {

    private VideoPlayer videoPlayer;
    private VideoSource videoSource;
    private AudioSource audioSource;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    // Use this for initialization
    void Start () {
        //StartCoroutine(PlayVideo("http://www.quirksmode.org/html5/videos/big_buck_bunny.mp4"));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator PlayVideo(string url)
    {
        videoPlayer = gameObject.GetComponent<VideoPlayer>();
        audioSource = gameObject.GetComponent<AudioSource>();

        videoPlayer.playOnAwake = true;
        audioSource.playOnAwake = true;

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = url;

        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            Debug.Log("Preparing Video");
            yield return null;
        }

        videoPlayer.Play();
        audioSource.Play();
    }
}
