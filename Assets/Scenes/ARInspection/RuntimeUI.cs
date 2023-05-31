using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RuntimeUI : MonoBehaviour
{
    private TextField idTextField;
    void Start()
    {
        SetId("abcd");
    }

    public void SetId(string str)
    {
        var document = GetComponent<UIDocument>();
        idTextField = document.rootVisualElement.Q("id") as TextField;

        idTextField.value = $"{str}";
    }
}
