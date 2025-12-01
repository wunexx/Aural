using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource weaponSource;
    [SerializeField] AudioSource hitSource;
    [SerializeField] AudioSource otherSource;
    [SerializeField] AudioSource enemyAttackSource;
    [SerializeField] AudioSource projectileDestroySource;

    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic(AudioClip newTrack, float volume = 0.5f, float fadeTime = 1.0f)
    {
        StartCoroutine(FadeAndPlayMusic(newTrack, fadeTime, volume));
    }

    private IEnumerator FadeAndPlayMusic(AudioClip newTrack, float fadeTime, float volume)
    {
        yield return StartCoroutine(FadeMusic(fadeTime));

        musicSource.clip = newTrack;
        musicSource.volume = volume;
        musicSource.Play();

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0f, volume, t / fadeTime);
            yield return null;
        }

        musicSource.volume = volume;

    }

    public IEnumerator FadeMusic(float fadeTime)
    {
        if (musicSource.isPlaying)
        {
            float startVolume = musicSource.volume;

            for (float t = 0; t < fadeTime; t += Time.deltaTime)
            {
                musicSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeTime);
                yield return null;
            }

            musicSource.volume = 0f;
            musicSource.Stop();
        }
    }
    public void PlayWeaponSFX(AudioClip clip, float volume = 0.1f)
    {
        PlaySFX(weaponSource, clip, volume);
    }

    public void PlayHitSFX(AudioClip clip, float volume = 0.1f)
    {
        PlaySFX(hitSource, clip, volume);
    }
    public void PlayOtherSFX(AudioClip clip, float volume = 0.1f)
    {
        PlaySFX(otherSource, clip, volume);
    }

    public void PlayEnemyAttackSFX(AudioClip clip, float volume = 0.1f)
    {
        PlaySFX(enemyAttackSource, clip, volume);
    }

    public void PlayProjectileDestroySFX(AudioClip clip, float volume = 0.1f)
    {
        PlaySFX(projectileDestroySource, clip, volume);
    }

    public void PlaySFX(AudioSource source, AudioClip clip, float volume = 0.1f)
    {
        source.volume = volume;
        source.pitch = Random.Range(0.9f, 1.1f);
        source.PlayOneShot(clip);
    }
}
