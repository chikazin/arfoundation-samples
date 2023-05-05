using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class clickEvent : MonoBehaviour, IPointerClickHandler
{
    private string info = "jijef";
    public Text objectinfo;
    public void OnPointerClick(PointerEventData eventData)
    {
        objectinfo.text = $"object info :{info}";
        Debug.Log($"{objectinfo.gameObject.name} clicked!");
    }
}
