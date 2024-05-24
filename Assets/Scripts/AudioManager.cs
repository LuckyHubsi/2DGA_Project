using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioSource enemySfxSource;

    [SerializeField]
    private List<AudioClip> musicClips;
    [SerializeField]
    private List<AudioClip> sfxClips;
    [SerializeField]
    private List<AudioClip> enemySfxClips;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string clipName)
    {
        AudioClip clip = musicClips.Find(x => x.name == clipName);
        if (clip != null)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void PlayEnemySFX(string clipName)
    {
        AudioClip clip = enemySfxClips.Find(x => x.name == clipName);
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayEnemySFX(string clipName, Vector3 position)
    {
        AudioClip clip = enemySfxClips.Find(x => x.name == clipName);
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, position);
        }
    }

    public void PlaySFX(string clipName)
    {
        AudioClip clip = sfxClips.Find(x => x.name == clipName);
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlaySFX(string clipName, Vector3 position)
    {
        AudioClip clip = sfxClips.Find(x => x.name == clipName);
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, position);
        }
    }
}
