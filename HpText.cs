using UnityEngine;
using TMPro;

public class HpText : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;

    // Referencja do obiektu EnemyMovement
    public LevelManager levelManager;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();

        // Sprawd�, czy obiekt EnemyMovement jest przypisany
        if (levelManager == null)
        {
            Debug.LogError("Nie przypisano obiektu LevelManager. Przypisz go w Unity Inspector.");
        }
        
    }
    private void UpdateHealthText(int currentHealth)
    {
        // Ustaw tekst w TextMeshProUGUI
        textMeshPro.text = "Hp: " + currentHealth.ToString();
    }

    void Update()
    {
        // Sprawd�, czy obiekt EnemyMovement i textMeshPro nie s� null
        if (levelManager != null && textMeshPro != null)
        {
            // Pobierz aktualne zdrowie z EnemyMovement
            int currentHpText = levelManager.GetCurrentHp();

            // Ustaw tekst w TextMeshProUGUI
            textMeshPro.text = "Hp: " + currentHpText.ToString();
        }
    }
}
