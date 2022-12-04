using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShadeOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    // constants
    private const float ALPHA_BASE = 0.5f;
    private const float ALPHA_HOVER = 1.0f;

    // Unity variables
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, ALPHA_BASE); // set to initial base alpha value
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = new Color(image.color.r, image.color.b, image.color.g, ALPHA_HOVER); // set ALPHA value (transparent highlight)
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = new Color(image.color.r, image.color.b, image.color.g, ALPHA_BASE); // return to ALPHA_BASE state
    }
}
