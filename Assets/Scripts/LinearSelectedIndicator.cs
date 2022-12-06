using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinearSelectedIndicator : MonoBehaviour
{
    // constants
    private const float SELECTED_ALPHA = 1f;
    private const float UNSELECTED_ALPHA = 0.75f;

    // Inspector variables
    [SerializeField] private bool isLinear;

    // Unity variables
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.getIsLinear() == isLinear)
            image.color = new Color(image.color.r, image.color.g, image.color.b, SELECTED_ALPHA);
        else
            image.color = new Color(image.color.r, image.color.g, image.color.b, UNSELECTED_ALPHA);
    }
}
