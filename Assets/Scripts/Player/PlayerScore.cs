using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public static PlayerScore Instance;
    int score = 0;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject winScreen;

    private void OnDisable()
    {
        Destroy(gameObject);
    }

    private void Awake()
    {
        Instance = this;
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;

        UpdateUI();
    }

    public void OnWin()
    {
        winScreen.SetActive(true);
    }

    public int GetScore() => score;

    public void UpdateUI()
    {
        scoreText.text = $"Score: {score}";
    }
}
