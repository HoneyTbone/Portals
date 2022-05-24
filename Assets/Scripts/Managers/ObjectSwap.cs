using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSwap : MonoBehaviour
{
    private DimensionManager _dimensionManager;

    public GameObject dimensionOneObject;
    public GameObject dimensionTwoObject;

    public Collider dimensionOneCollider;
    public Collider dimensionTwoCollider;

    private float checkCountDown = 1f;

    private void Awake()
    {
        _dimensionManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DimensionManager>();
    }
    private void Update()
    {
        if (checkCountDown <= 0f)
        {
            if (_dimensionManager.whatDimension == 1)
            {
                dimensionOneObject.gameObject.SetActive(true);
                dimensionTwoObject.gameObject.SetActive(false);

                dimensionOneCollider.enabled = true;
                dimensionTwoCollider.enabled = false;
            }
            else if (_dimensionManager.whatDimension == 2)
            {
                dimensionOneObject.gameObject.SetActive(false);
                dimensionTwoObject.gameObject.SetActive(true);

                dimensionOneCollider.enabled = false;
                dimensionTwoCollider.enabled = true;
            }
            checkCountDown = 1f;
        }
        else
        {
            checkCountDown -= Time.deltaTime;
        }
        
    }
}
