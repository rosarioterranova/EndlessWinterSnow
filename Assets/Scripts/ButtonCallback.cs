using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonCallback : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public delegate void ClickState();
    public ClickState OnPressed;
    public ClickState OnReleased;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPressed();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnReleased();
    }
}

