using UnityEngine;
using Siccity.GLTFUtility;

public class RetrieveGltfData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // FIXME: 在手机上运行时找不到文件
        GameObject gb = Importer.LoadFromFile("Assets/Scenes/ARInspection/model.gltf");
        gb.transform.position = new Vector3(0, 0, -6);
    }

    // Update is called once per frame
    void Update()
    {

    }
}