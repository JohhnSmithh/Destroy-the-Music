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
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {    
    }


    void OnEnable()
    {
        image = GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, ALPHA_BASE); // set to initial base alpha value
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(button.interactable)
            image.color = new Color(image.color.r, image.color.b, image.color.g, ALPHA_HOVER); // set ALPHA value (transparent highlight)
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(button.interactable)
            image.color = new Color(image.color.r, image.color.b, image.color.g, ALPHA_BASE); // return to ALPHA_BASE state
    }
}
