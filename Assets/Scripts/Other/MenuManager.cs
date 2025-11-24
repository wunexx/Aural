using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] SceneTransition sceneTransition; 

    public void OnPlayButton()
    {
        sceneTransition.TransitionToScene("Game");
    }
}
