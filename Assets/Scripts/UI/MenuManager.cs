using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] SceneTransition sceneTransition;
    [SerializeField] AudioClip bgMusic;
    [SerializeField] float volume = 0.3f;

    private void Start()
    {
        SoundManager.Instance.PlayMusic(bgMusic, volume);
    }

    public void OnPlayButton()
    {
        StartCoroutine(SoundManager.Instance.FadeMusic(1f));
        sceneTransition.TransitionToScene("Game");
    }
}
