using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObjectInfo : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject lastClickObj;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
            {
                Transform target = hitInfo.transform;
                if (target != null)
                {
                    // 隐藏上一次点击的GameObject（如果有）的Outline
                    HideOutline();
                    lastClickObj = target.gameObject;
                    // 选中物体后显示物体的信息到UI框中
                    ShowObjInfo(target.gameObject);
                    // 显示边框
                    ShowOutline(target.gameObject);
                }
            }
            else
            {
                // 隐藏物体信息框
                HideObjInfo();
                // 隐藏上一次点击的GameObject（如果有）的Outline
                HideOutline();


            }
        }
    }

    void ShowOutline(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out Outline outline))
        {
            outline.OutlineColor = Color.red;
            outline.enabled = true;
        }
        else
        {
            gameObject.AddComponent<Outline>();
            gameObject.GetComponent<Outline>().OutlineColor = Color.red;
            gameObject.GetComponent<Outline>().enabled = true;
        }
    }

    void HideOutline()
    {
        if (lastClickObj)
        {
            if (lastClickObj.TryGetComponent<Outline>(out Outline outline))
            {
                outline.enabled = false;
            }
        }
    }

    void ShowObjInfo(GameObject gameObject)
    {
        var uiToolkit = GameObject.Find("UIDocument");
        if (uiToolkit.TryGetComponent<RuntimeUI>(out var runtimeUI))
        {
            runtimeUI.SetId($"{gameObject.name}1008611");
        }
    }

    void HideObjInfo()
    {
        var uiToolkit = GameObject.Find("UIDocument");
        if (uiToolkit.TryGetComponent<RuntimeUI>(out var runtimeUI))
        {
            runtimeUI.HideInfobox();
        }
    }
}
