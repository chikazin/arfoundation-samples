using UnityEngine;
using Siccity.GLTFUtility;

public class RetrieveGltfData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextAsset gltfFile = Resources.Load<TextAsset>("model");
        GameObject gb = Importer.ImportGLTFFromText(gltfFile.text);
    }

    // Update is called once per frame
    void Update()
    {

    }
}