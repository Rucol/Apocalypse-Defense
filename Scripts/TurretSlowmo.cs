using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class TurretSlowmo : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private GameObject upgradeUI;


    [Header("Attributes")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float aps = 4f;
    [SerializeField] private float freezeTime = 1f;
    [SerializeField] private int baseUpgradeCost = 150;

    private float apsBase;
    private float targetingRangeBase;
    private float freezeTimeBase;

    private float timeUntilFire;
    private int level = 1;

    private void Start()
    {
        apsBase = aps;
        targetingRangeBase = targetingRange;
        upgradeButton.onClick.AddListener(Upgrade);
    }
    private void Update()
    {
        timeUntilFire += Time.deltaTime;

        if (timeUntilFire >= 1f / aps)
        {
            FreezeEnemies();
            timeUntilFire = 0f;
        }
    }

    private void FreezeEnemies()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask); 

        if (hits.Length > 0) 
        {
            for (int i = 0; i < hits.Length; i++) 
            {
                RaycastHit2D hit = hits[i];

                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();
                em.UpdateSpeed(0.5f);

                StartCoroutine(ResetEnemySpeed(em));
            }
        }
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime); 

        em.ResetSpeed();
    }
    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }
    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }
    public void Upgrade()
    {
        if (CalculateCost() > LevelManager.main.currency) return;
        LevelManager.main.SpendCurrency(CalculateCost());

        level++;
        aps = CalculateAPS();
        targetingRange = CalculateRange();
        freezeTime = CalculateFreezeTime();

        CloseUpgradeUI();
        Debug.Log("new aps" + aps);
        Debug.Log("new targeting range" + targetingRange);
        Debug.Log("new freeze time" + freezeTime);
        Debug.Log("new cost" + CalculateCost());
    }
    private int CalculateCost()
    {
        return baseUpgradeCost * Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 1.1f));
    }
    private float CalculateAPS()
    {
        return apsBase * Mathf.Pow(level, 0.6f);
    }
    private float CalculateRange()
    {
        return targetingRangeBase * Mathf.Pow(level, 0.4f);
    }
    private float CalculateFreezeTime()
    {
        return freezeTimeBase * Mathf.Pow(level, 0.1f);
    }
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
