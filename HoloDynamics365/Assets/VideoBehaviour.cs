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
