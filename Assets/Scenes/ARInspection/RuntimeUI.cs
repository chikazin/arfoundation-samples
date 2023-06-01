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

    void Start()
    {
        document = GetComponent<UIDocument>();
        groupBox = document.rootVisualElement.Q("GroupBox") as GroupBox;
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
}
