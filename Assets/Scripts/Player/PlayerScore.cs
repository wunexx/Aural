using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    int score = 0;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject winScreen;

    public void AddScore(int amount)
    {
        score += amount;

    }

    public void OnWin()
    {
        winScreen.SetActive(true);
    }

    public void UpdateUI()
    {
        scoreText.text = $"Score: {score}";
    }
}
