using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class RuntimeUI : MonoBehaviour
{
    private TextField idTextField;
    private Button setValueBtn;
    private UIDocument document;
    void Start()
    {
        document = GetComponent<UIDocument>();
        SetId("abcd");
        setValueBtn = document.rootVisualElement.Q("SetValueBtn") as Button;
        setValueBtn.RegisterCallback<ClickEvent>(OnBtnClick);
    }

    public void SetId(string str)
    {
        idTextField = document.rootVisualElement.Q("id") as TextField;
        idTextField.value = $"{str}";
    }

    void OnBtnClick (ClickEvent evt)
    {
        SetId("10086");
    }
}
