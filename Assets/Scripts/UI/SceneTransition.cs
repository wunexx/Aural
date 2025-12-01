using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    Animator animator;

    string targetScene = "";
    

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TransitionToScene(string sceneName)
    {
        targetScene = sceneName;
        animator.SetTrigger("FadeIn");
    }

    public void OnAnimationEnd()
    {
        Time.timeScale = 1;

        if (!string.IsNullOrEmpty(targetScene)) SceneManager.LoadScene(targetScene);
        else animator.SetTrigger("FadeOut");
    }

    public void OnDungeonGenerate()
    {
        animator.SetTrigger("FadeIn");
    }
}
