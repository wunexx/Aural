using UnityEngine;

public class FinalScreenManager : MonoBehaviour
{
    [SerializeField] SceneTransition sceneTransition;

    public void OnRestartButton()
    {
        sceneTransition.TransitionToScene("Game");
    }

    public void OnMenuButton()
    {
        sceneTransition.TransitionToScene("Menu");
    }
}
