using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] float volume;

    public void PlaySound()
    {
        SoundManager.Instance.PlayOtherSFX(audioClips[Random.Range(0, audioClips.Length)], volume);
    }
}
