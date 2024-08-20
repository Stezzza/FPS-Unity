using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHealth : MonoBehaviour
{

    public int maxHealth = 3;
    public int points = 1;

    private int currentHealth;

    public GameManager gameManager;

    public GameManager Gamemanger {  get { return gameManager; } set { gameManager = value; } }

    void OnEnable()
    {
        //set current health to max health
        currentHealth = maxHealth;
    }

    private void DisableTarget()
    {
        Debug.Log("Pew");
        if (gameManager != null)
        {
            gameManager.AddScore(points);
        }
        gameObject.SetActive(false);
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;
        //if our health is 0 or less, call disable targets
        if(currentHealth <= 0)
        {
            DisableTarget();
        }
    }
}
