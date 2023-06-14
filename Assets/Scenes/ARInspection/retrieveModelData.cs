using UnityEngine;
using Newtonsoft.Json.Linq;
using Siccity.GLTFUtility;

public class RetrieveGltfData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject gb = Importer.LoadFromFile("Assets/Scenes/ARInspection/pipe_brick_monkey.gltf");
        gb.transform.position = new Vector3(1, 1, 1);
        var extra = gb.GetComponent<ExtraData>().extraData;
        foreach (var extraData in extra)
        {
            string key = extraData.Key;
            JToken val = extraData.Value;
            Debug.Log($"{key}: {val}");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}