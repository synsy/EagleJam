using UnityEngine;
using Unity.VisualScripting;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource flappingSource;
    public AudioClip flappingClip;
    public AudioClip musicClip;
    public AudioClip undeadMusicClip;
    public AudioClip hitClip;
    public AudioClip dieClip;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        PlayMusic(musicClip);

        
        PlayFlap(flappingClip);

    }

    void Update()
    {

    }

    public void PlayUndeadMusic(AudioClip undeadMusicClip)
    {
        musicSource.clip = undeadMusicClip;
        musicSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlayFlap(AudioClip flappingClip)
    {
        flappingSource.clip = flappingClip;
        flappingSource.Play();
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        sfxSource.clip = sfxClip;
        sfxSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void StopSFX()
    {
        sfxSource.Stop();
    }
}
