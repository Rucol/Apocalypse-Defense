using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject towerObj;
    public Turret turret;
    public TurretSlowmo turretslowmo;
    private Color startColor;

    private void Start()
    {
        if (sr != null)
        {
            startColor = sr.color;
        }
        else
        {
            Debug.LogError("SpriteRenderer (sr) is not assigned to the Plot script.");
        }
    }
    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        if (UIManager.main.IsHoveringUI()) return;

        if (towerObj != null)
        {
            
            if (turret != null)
            {
                turret.OpenUpgradeUI();
            }
            else if(turretslowmo != null)
            {
                turretslowmo.OpenUpgradeUI();
            }
            else
            {
                Debug.LogError("Turret is not assigned to the Plot script.");
            }
            return;
        }

        Tower towerToBuild = BuildManager.main.GetSelectedTower();

        if (towerToBuild.cost > LevelManager.main.currency)
        {
            Debug.Log("You can't afford this tower");
            return;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost);
        towerObj = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
        turret = towerObj.GetComponent<Turret>();
        turretslowmo = towerObj.GetComponent<TurretSlowmo>();
    }

}
