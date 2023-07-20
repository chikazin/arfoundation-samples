using Newtonsoft.Json.Linq;
using Siccity.GLTFUtility;
using UnityEngine;
using UnityEngine.UIElements;

public class RuntimeUI : MonoBehaviour
{
    private TextField idTextField;
    private UIDocument document;
    private GroupBox groupBox;
    // 是否允许Gizmo调整
    private Toggle isEditToggle;
    // 模型节点信息box
    private VisualElement objInfoBox;

    // 鼠标是否在信息框上
    public bool isMouseInInfoBox;

    void MouseEnterEventCallback(MouseEnterEvent evt){
        this.isMouseInInfoBox = true;
    }
    void MouseLeaveEventCallback(MouseLeaveEvent evt){
        this.isMouseInInfoBox = false;
    }


    void Start()
    {
        document = GetComponent<UIDocument>();
        groupBox = document.rootVisualElement.Q("GroupBox") as GroupBox;
        groupBox.RegisterCallback<MouseEnterEvent>(MouseEnterEventCallback);
        groupBox.RegisterCallback<MouseLeaveEvent>(MouseLeaveEventCallback);
        isEditToggle = document.rootVisualElement.Q("isEdit") as Toggle;
        objInfoBox = document.rootVisualElement.Q("obj-info-box") as VisualElement;
        HideInfobox();
    }

    public void SetObjName(string str)
    {
        idTextField = document.rootVisualElement.Q("obj-name") as TextField;
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

    public void GenerateInfo(GameObject gameObject)
    {
        // 删除之前的内容
        objInfoBox.Clear();
        SetObjName($"{gameObject.name}");

        if (!gameObject.TryGetComponent<ExtraData>(out var extraCompo))
            return;

        JObject jobj = extraCompo.extraData;
        if (jobj == null)
            return;

        foreach (var item in jobj)
        {
            var key = item.Key;
            var value = item.Value;
            var textField = new TextField
            {
                label = $"{key}：",
                value = value.ToString()
            };
            textField.AddToClassList("info-item");
            textField.isReadOnly = true;
            textField.focusable = false;
            textField.selectAllOnFocus = false;
            textField.selectAllOnMouseUp = false;
            objInfoBox.Add(textField);
        }
    }
}
