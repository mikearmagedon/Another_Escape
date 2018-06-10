using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static int score;

    Text scoreText;
    const string COINS = "Coins: ";

    void Start()
    {
        scoreText = GetComponent<Text>();
        scoreText.text = COINS + score.ToString();
    }

    void Update()
    {
        scoreText.text = COINS + score.ToString();
    }
}
