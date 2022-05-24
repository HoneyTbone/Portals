using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionManager : MonoBehaviour
{
    public int whatDimension = 1;

    private void Start()
    {
        Debug.Log($"Currently in Dimension {whatDimension}");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SwitchDimension();
        }
    }

    public void SwitchDimension()
    {
        if (whatDimension == 1)
        {
            whatDimension = 2;
        }
        else if (whatDimension == 2)
        {
            whatDimension = 1;
        }
        Debug.Log($"Switched to Dimension {whatDimension}");
    }
}
