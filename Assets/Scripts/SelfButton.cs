using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelfButton : Button
{
    public ButtonClickedEvent onSelfClick = new ButtonClickedEvent();
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (Panel.mouseButton)
        {
            onSelfClick?.Invoke();
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        onSelfClick?.Invoke();
    }
}
