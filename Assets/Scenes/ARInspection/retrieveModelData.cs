using System.Collections.Generic;
using UnityEngine;
using Siccity.GLTFUtility;

public class RetrieveGltfData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject gb = Importer.LoadFromFile("Assets/Scenes/ARInspection/pipe_brick.gltf");
        gb.transform.position = new Vector3(1, 1, 1);
        var extra = gb.GetComponentInChildren<ExtraData>().extraData;

        foreach (KeyValuePair<string, string> pair in extra)
        {
            string key = pair.Key;
            string value = pair.Value;
            Debug.Log($"{key}:{value}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
