using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class RuntimeUI : MonoBehaviour
{
    private TextField idTextField;
    private UIDocument document;
    private GroupBox groupBox;
    // 是否允许Gizmo调整
    private Toggle isEditToggle;

    void Start()
    {
        document = GetComponent<UIDocument>();
        groupBox = document.rootVisualElement.Q("GroupBox") as GroupBox;
        isEditToggle = document.rootVisualElement.Q("isEdit") as Toggle;
        HideInfobox();
    }

    public void SetId(string str)
    {
        idTextField = document.rootVisualElement.Q("id") as TextField;
        idTextField.value = $"{str}";
        groupBox.style.display = DisplayStyle.Flex;
    }

    public void HideInfobox()
    {
        groupBox.style.display = DisplayStyle.None;
    }

    public bool IsEdit()
    {
        return isEditToggle.value;
    }
}
