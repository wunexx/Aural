using TMPro;
using UnityEngine;

public class FinalScreenManager : MonoBehaviour
{
    [SerializeField] SceneTransition sceneTransition;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject[] objectsToDisable;

    [SerializeField] bool isDeathWinScreen;

    private void Start()
    {
        if(scoreText != null)
            scoreText.text = $"Score: { PlayerScore.Instance.GetScore()}";

        if(objectsToDisable.Length > 0)
            DisableObjects();

        if(isDeathWinScreen)
            PauseManager.Instance.OnDeathOrWin();
    }

    public void OnRestartButton()
    {
        sceneTransition.TransitionToScene("Game");
    }

    public void OnMenuButton()
    {
        sceneTransition.TransitionToScene("Menu");
    }

    void DisableObjects()
    {
        foreach (GameObject obj in objectsToDisable)
            obj.SetActive(false);
    }
}
