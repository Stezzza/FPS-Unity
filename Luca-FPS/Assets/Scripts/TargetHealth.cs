using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHealth : MonoBehaviour
{
    public int maxHealth = 1; // each hit destroys
    public int points = 1; // hit points

    private int currentHealth;

    [HideInInspector]
    public GameManager gameManager; // assign from GameManager

    void OnEnable()
    {
        currentHealth = maxHealth; // set health
    }

    private void DisableTarget()
    {
        Debug.Log("target hit!");
        if (gameManager != null)
        {
            gameManager.AddScore(points);
        }
        else
        {
            Debug.LogWarning("missing gameManager reference");
        }
        gameObject.SetActive(false);
    }

    // damage the target
    public void Damage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            DisableTarget();
        }
    }

    // hit with mouse
    private void OnMouseDown()
    {
        Damage(1);
    }
}
