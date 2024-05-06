using System.Collections;
using UnityEngine;
using System;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;
    private int maxHP;
    private int currentHP;
    public int damageOnEndpoint = 10;

    private float baseSpeed;

   
    private void Start()
    {
        baseSpeed = moveSpeed;
        target = LevelManager.main.path[pathIndex];
        maxHP = LevelManager.main.MaxHealth;
        currentHP = maxHP;
        LevelManager.main.GetComponent<LevelManager>().healthSlider.maxValue = LevelManager.main.GetComponent<LevelManager>().MaxHealth;
        LevelManager.main.GetComponent<LevelManager>().healthSlider.value = LevelManager.main.GetComponent<LevelManager>().CurrentHealth;
        
    }

    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;
            if (pathIndex < LevelManager.main.path.Length)
            {
                target = LevelManager.main.path[pathIndex];
            }
            else
            {
                // Przeciwnik dotar³ do "EndPoint"
                HandleEnemyAtEndPoint();
            }
        }
    }

    private void FixedUpdate()
    {
        MoveEnemy();
    }

    private void MoveEnemy()
    {
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void HandleEnemyAtEndPoint()
    {
        // Odejmij punkty ¿ycia gracza
        LevelManager.main.CurrentHealth -= damageOnEndpoint;
        //Debug.Log($"Przeciwnik dotar³ do 'EndPoint'! Odejmowanie HP: {damageOnEndpoint}. Aktualne HP gracza: {LevelManager.main.CurrentHealth}");
        // Zniszcz przeciwnika
        LevelManager.main.UpdateHealthSlider();
        if (LevelManager.main.CurrentHealth <= 0)
        {
            LevelManager.main.GameOver();
        }

        HandleEnemyDestroyed();
        Destroy(gameObject);
    }

    private void UpdateHealthSlider()
    {
        // Aktualizuj wartoœæ na pasku zdrowia
        LevelManager.main.healthSlider.value = LevelManager.main.CurrentHealth;
    }
    private void HandleEnemyDestroyed()
    {
        EnemySpawner.onEnemyDestroy.Invoke();
    }

    public void UpdateSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public void ResetSpeed()
    {
        moveSpeed = baseSpeed;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            // Przeciwnik zosta³ pokonany
            HandleEnemyDestroyed();
            Destroy(gameObject);
        }
    }
  
}
