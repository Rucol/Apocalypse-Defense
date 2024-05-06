using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    public Slider healthSlider;
    public int MaxHealth = 100;
    public int CurrentHealth = 100;

    public Transform startPoint;
    public Transform[] path;

    public int currency;

   
    private void Awake()
    {
        main = this;
        healthSlider = GameObject.Find("Slider").GetComponent<Slider>();
        if (healthSlider == null)
        {
            Debug.LogError("healthSlider not found or Slider component is missing.");
        }
    }
    public int GetCurrentHp()
    {
        return CurrentHealth;
    }

    public void Start()
    {
        currency = 400;
    }
    public void IncreaseCurrency(int amount)
    {
        currency += amount;
    }
    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            return true;
        }
        else
        {
            Debug.Log("You do not have enough to purchase this item");
            return false;
        }
    }
    public void UpdateHealthSlider()
    {
        
        healthSlider.value = CurrentHealth;
        if (CurrentHealth <= 0)
        {
            GameOver();
        }
    }
    public void GameOver()
    {
       
        SceneManager.LoadScene("GameOver");
    }
}
