using RuntimeGizmos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformGizmoMgr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var uiToolkit = GameObject.Find("UIDocument");
        if (uiToolkit.TryGetComponent<RuntimeUI>(out var runtimeUI))
        {

            if (this.gameObject.TryGetComponent<TransformGizmo>(out var transformGizmo))
            {
                if (runtimeUI.IsEdit())
                {
                    transformGizmo.enabled = true;
                }
                else
                {
                    transformGizmo.enabled = false;
                }
            }
        }
    }

}
