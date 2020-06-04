using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum AudioChannel
    {
        Master,
        SFX,
        Music
    }

    public float masterVolPercent {get; private set;}
    public float sfxVolPercent { get; private set; }
    public float musicVolPercent { get; private set; }

    AudioSource[] musicSources;
    int activeMusicSourcesIndex;

    public static AudioManager instance;

    AudioSource sfx2DSource;

    Transform audioListener;
    public Transform playerT;

    SoundLibrary library;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            library = GetComponent<SoundLibrary>();

            musicSources = new AudioSource[2];
            for (int i = 0; i < 2; i++)
            {
                GameObject newMusicSource = new GameObject("Music Source " + (i + 1));
                musicSources[i] = newMusicSource.AddComponent<AudioSource>();
                newMusicSource.transform.parent = transform;
            }
            GameObject newSfx2DSource = new GameObject("2D SFX Source");
            sfx2DSource = newSfx2DSource.AddComponent<AudioSource>();
            newSfx2DSource.transform.parent = transform;

            if (FindObjectOfType<Player>() != null)
            {
                playerT = FindObjectOfType<Player>().transform;
            }

            masterVolPercent = PlayerPrefs.GetFloat("MasterVol", masterVolPercent);
            sfxVolPercent = PlayerPrefs.GetFloat("SFXVol", sfxVolPercent);
            musicVolPercent = PlayerPrefs.GetFloat("MusicVol", musicVolPercent);

            audioListener = FindObjectOfType<AudioListener>().transform;
        }
    }

    private void Update()
    {
        if (playerT != null)
        {
            audioListener.position = playerT.position;
        }
    }

    public void SetVolume(float volPercent, AudioChannel channel)
    {
        switch (channel)
        {
            case AudioChannel.Master:
                masterVolPercent = volPercent;
                break;
            case AudioChannel.SFX:
                sfxVolPercent = volPercent;
                break;
            case AudioChannel.Music:
                musicVolPercent = volPercent;
                break;
        }

        musicSources[0].volume = musicVolPercent * masterVolPercent;
        musicSources[1].volume = musicVolPercent * masterVolPercent;

        PlayerPrefs.SetFloat("MasterVol", 1);
        PlayerPrefs.SetFloat("SFXVol", 1);
        PlayerPrefs.SetFloat("MusicVol", 1);
        PlayerPrefs.Save();
    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {
        if (clip != null)
        {
            activeMusicSourcesIndex = 1 - activeMusicSourcesIndex;
            musicSources[activeMusicSourcesIndex].clip = clip;
            musicSources[activeMusicSourcesIndex].Play();

            StartCoroutine(AnimateMusicCrossFade(fadeDuration));
        }
    }

    public void PlaySound(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos, sfxVolPercent * masterVolPercent);
        }
    }

    public void PlaySound(string soundName, Vector3 pos)
    {
        PlaySound(library.GetClipFromName(soundName), pos);
    }

    public void PlaySound2D(string soundName)
    {
        sfx2DSource.PlayOneShot(library.GetClipFromName(soundName), sfxVolPercent * masterVolPercent);
    }

    IEnumerator AnimateMusicCrossFade(float duration)
    {
        float percent = 0;

        while(percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            musicSources[activeMusicSourcesIndex].volume = Mathf.Lerp(0, musicVolPercent * masterVolPercent, percent);
            musicSources[1 - activeMusicSourcesIndex].volume = Mathf.Lerp(musicVolPercent * masterVolPercent, 0, percent);
            yield return null;
        }
    }
}
