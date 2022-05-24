using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Target : MonoBehaviour
{
    public float damageChanges = 1;
    public float health = 50f;
    public int whatDimensionForChanges;

    private DimensionManager _dimensionManager;

    private void Awake()
    {
        _dimensionManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DimensionManager>();
    }

    public void TakeDamage(float amount)
    {
        if(whatDimensionForChanges == _dimensionManager.whatDimension)
        {
            health -= amount * damageChanges;
        }
        else
        {
            health -= amount;
        }
        
        if (health <= 0f)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }

}