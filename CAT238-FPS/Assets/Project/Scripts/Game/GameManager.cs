using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game")]
    public Player player;
    public GameObject enemySpawner;
    [Header("UI")]
    public Text ammoText;
    public Text healthText;
    public Text enemyText;
    public Text infoText;

    // Start is called before the first frame update
    void Start()
    {
        infoText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        int numberEnemies = enemySpawner.GetComponentsInChildren<Enemy>().Length;
        ammoText.text = "Ammo: " + player.Ammo;
        healthText.text = "Health: " + player.Health;
        enemyText.text = "Enemies: " + numberEnemies;

        int killedEnemies = 0;
        foreach(Enemy enemy in enemySpawner.GetComponentsInChildren<Enemy>()) // We are going to iterate over every enemy
        {
            if(enemy.Killed == true)
            {
                killedEnemies++;
                enemyText.text = "Enemies: " + (numberEnemies-killedEnemies);
            }

            // Win condition
            if(killedEnemies == enemySpawner.GetComponentsInChildren<Enemy>().Length)
            {
                infoText.text = "YOU WIN";
                infoText.gameObject.SetActive(true);
            }
            // Lose condition
            if(player.Killed == true)
            {
                infoText.text = "YOU LOSE";
                infoText.gameObject.SetActive(true);
            }
        }
    }
}
