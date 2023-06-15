using UnityEngine;
using Siccity.GLTFUtility;

public class RetrieveGltfData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject gb = Importer.LoadFromFile("Assets/Scenes/ARInspection/pipe_brick_monkey.gltf");
        gb.transform.position = new Vector3(0, 0, -6);
    }

    // Update is called once per frame
    void Update()
    {

    }
}