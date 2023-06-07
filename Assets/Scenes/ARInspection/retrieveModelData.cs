using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Siccity.GLTFUtility;

public class retrieveFbxData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject gb = Importer.LoadFromFile("Assets/Scenes/ARInspection/shp_data_pipe.gltf");
        Debug.Log(gb);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
