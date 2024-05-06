using TMPro;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public EnemySpawner enemySpawner; 

    public TextMeshProUGUI textMeshPro;

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();

        if (enemySpawner == null)
        {
            Debug.LogError("Nie przypisano obiektu EnemySpawner. Przypisz go w Unity Inspector.");
        }
    }

    private void Update()
    {
        if (enemySpawner != null && textMeshPro != null)
        {
            int currentWaveValue = enemySpawner.GetCurrentWave();  // Add parentheses here
            textMeshPro.text = "Wave: " + currentWaveValue.ToString();
        }
    }


}
