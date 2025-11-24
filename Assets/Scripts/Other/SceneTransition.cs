using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    Animator animator;
    Image image;

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
        if (!string.IsNullOrEmpty(targetScene)) SceneManager.LoadScene(targetScene);
    }
}
