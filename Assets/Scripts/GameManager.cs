using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private float health;
    [SerializeField] 
    private Image healthBar;

    private float maxHealth;

    [SerializeField]
    private TextMeshProUGUI timerText;

    private float gameTimer;

    public float GetHealth() => health;

    public void DecreaseHealth(int decreaseBy)
    {
        health -= decreaseBy;
        healthBar.fillAmount = health / maxHealth;
    }
    public void IncreaseHealth(int increaseBy) => health += increaseBy;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        maxHealth = GetHealth();
    }

    // Update is called once per frame
    void Update()
    {
        // Update the game timer
        gameTimer += Time.deltaTime;

        // Update the UI text
        UpdateTimerUI();

        if (health <= 0)
        {

        }
    }

    // Function to update the timer UI
    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Survived Time: " + Mathf.Floor(gameTimer).ToString();
        }
    }
}
