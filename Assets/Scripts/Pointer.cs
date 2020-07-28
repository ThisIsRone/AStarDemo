using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{

    static Pointer start;

    static Pointer end;

    private Map map;

    private int x,y;

    private Vector2 fator = new Vector2(80,80);

    private Image img = null;

    public GameObject obj_infoRoot;

    public Text txt_number;

    public Text txt_pos;

    private void Awake()
    {
        img = transform.GetComponent<Image>();
        SelfButton btn = transform.GetComponent<SelfButton>();
        btn.onSelfClick.AddListener(Switch);
    }

    public void SetPointer(Map map,int x,int y,Toggle tgl_pos,Toggle tgl_cost)
    {
        this.map = map;
        this.x = x;
        this.y = y;
        transform.name = ToString();
        if (map.IsStart(x, y))
        {
            start = this;
            SetStart();
        }
        else if (map.IsEnd(x, y))
        {
            end = this;
            SetEnd();
        }
        else
        {
            if (map.GetValue(x, y) > 0)
            {
                SetObstacle();
            }
            else
            {
                SetNormal();
            }
        }
        OnClickTglPos(tgl_pos.isOn);
        OnClickTglCost(tgl_cost.isOn);
        tgl_pos.onValueChanged.AddListener(OnClickTglPos);
        tgl_cost.onValueChanged.AddListener(OnClickTglCost);
    }

    private void OnClickTglPos(bool isOn)
    {
        txt_pos.gameObject.SetActive(isOn);
    }

    private void OnClickTglCost(bool isOn)
    {
        txt_number.gameObject.SetActive(isOn);
    }

    public void Switch()
    {
        if (map.IsStart(x,y))
        {
            Panel.isClickStart = true;
            Panel.isClickEnd = false;
        }
        else if (map.IsEnd(x, y))
        {
            Panel.isClickEnd = true;
            Panel.isClickStart = false;
        }

        if (Panel.isClickStart)
        {
            start.SetNormal();
            SetStart();
            start = this;
        }
        else if (Panel.isClickEnd)
        {
            end.SetNormal();
            SetEnd();
            end = this;
        }
        else
        {
            if (map.GetValue(x, y) == 0)
            {
                SetObstacle();
            }
            else
            {
                SetNormal();
            }
        }
    }

    private void SetStart()
    {
        map.SetValue(x, y, 0);
        map.start = new Vector2(x, y);
        img.color = Color.cyan;
        obj_infoRoot.SetActive(true);
        txt_pos.text = ToString();
    }

    private void SetEnd()
    {
        map.SetValue(x, y, 0);
        map.end = new Vector2(x, y);
        img.color = Color.blue;
        obj_infoRoot.SetActive(true);
        txt_pos.text = ToString();
    }


    private void SetNormal()
    {
        map.SetValue(x, y, 0);
        img.color = Color.white;
        obj_infoRoot.SetActive(true);
        txt_pos.text = ToString();
    }    
   
    public void SetSearch(Point point)
    {
        float a = img.color.a == 1?0.8f: img.color.a;
        a -= 0.2f;
        a = Mathf.Clamp(a, 0.3f, 0.8f);
        Color color = new Color(img.color.r, img.color.g, img.color.b, a);
        img.color = color;
    }

    public void ShowGHF(Point point)
    {
        txt_number.text = string.Format("F:{2} \n G:{0} H:{1}", point.G, point.H, point.F);
    }

    private void SetObstacle()
    {
        map.SetValue(x, y, 1);
        img.color = Color.red;
        obj_infoRoot.SetActive(false);
    }

    public void SetSelect()
    {
        if (start == this || end == this)
        {
            return;
        }
        img.color = Color.green;
    }

    public override string ToString()
    {
        return string.Format("(x:{0},y:{1})", x, y);
    }
}