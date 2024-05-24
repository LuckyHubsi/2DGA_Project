using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private int health;

    [SerializeField]
    private float gameTimer = 0f; // Add game timer

    public int GetHealth() => health;
    public void DecreaseHealth(int decreaseBy) => health -= decreaseBy;
    public void IncreaseHealth(int increaseBy) => health += increaseBy;

    public float GetGameTimer() => gameTimer;

    void Start()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        // Start the game timer
        StartCoroutine(UpdateGameTimer());
    }

    void Update()
    {
        //Debug.Log("Current Health: " + health);
    }

    IEnumerator UpdateGameTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            gameTimer += 1f;
            Debug.Log("Game Timer: " + gameTimer);
        }
    }
}
