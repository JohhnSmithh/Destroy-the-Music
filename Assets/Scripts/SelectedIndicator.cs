using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedIndicator : MonoBehaviour
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private bool isLinear;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.getIsLinear() == isLinear)
            indicator.SetActive(false);
        else
            indicator.SetActive(true);
    }
}
