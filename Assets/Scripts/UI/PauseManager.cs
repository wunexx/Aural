using UnityEngine;

public class PauseManager : MonoBehaviour, IUpdatable
{
    [SerializeField] GameObject[] panelObjects;

    public static PauseManager Instance;
    [SerializeField] GameObject pausePanel;
    [SerializeField] UpdateManager updateManager;
    [SerializeField] SceneTransition sceneTransition;

    bool isPaused = false;

    private void Awake()
    {
        Instance = this;

        updateManager.AddUpdatable(this);
    }

    private void OnDisable()
    { 
        updateManager.RemoveUpdatable(this); 
        Destroy(gameObject);
    }

    public void OnUpdate()
    {
        if(PlayerInputController.Instance.GetPauseInput() == 1)
        {
            OnPausePressed();
        }
    }

    public void OnPausePressed()
    {
        if (isPaused == false && CanPause() == false)
            return;

        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0 : 1;

        pausePanel.SetActive(isPaused);
    }

    public bool IsPaused() => isPaused;

    public void OnDeathOrWin()
    {
        pausePanel.SetActive(false);

        isPaused = true;

        Time.timeScale = 0;
    }

    bool CanPause()
    {
        foreach(GameObject obj in panelObjects)
        {
            if (obj.activeSelf == true)
                return false;
        }

        return true;
    }
}
