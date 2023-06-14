using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GltfClass
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Accessor
    {
        public int bufferView { get; set; }
        public int componentType { get; set; }
        public int count { get; set; }
        public List<double> max { get; set; }
        public List<double> min { get; set; }
        public string type { get; set; }
    }

    public class Asset
    {
        public string generator { get; set; }
        public string version { get; set; }
    }

    public class Attributes
    {
        public int POSITION { get; set; }
        public int TEXCOORD_0 { get; set; }
        public int NORMAL { get; set; }
    }

    public class Buffer
    {
        public int byteLength { get; set; }
        public string uri { get; set; }
    }

    public class BufferView
    {
        public int buffer { get; set; }
        public int byteLength { get; set; }
        public int byteOffset { get; set; }
        public int target { get; set; }
    }

    public class CrsX
    {
        public string description { get; set; }
        public double @default { get; set; }
    }

    public class CrsY
    {
        public string description { get; set; }
        public double @default { get; set; }
    }

    public class Extras
    {
        public dynamic _RNA_UI { get; set; }
        public string SRID { get; set; }

        [JsonProperty("crs x")]
        public double crsx { get; set; }

        [JsonProperty("crs y")]
        public double crsy { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public string id { get; set; }
        public string dept { get; set; }
    }

    public class Latitude
    {
        public string description { get; set; }
        public double @default { get; set; }
        public double min { get; set; }
        public double max { get; set; }
    }

    public class Longitude
    {
        public string description { get; set; }
        public double @default { get; set; }
        public double min { get; set; }
        public double max { get; set; }
    }

    public class Mesh
    {
        public string name { get; set; }
        public List<Primitive> primitives { get; set; }
    }

    public class Node
    {
        public JObject extras { get; set; }
        public int mesh { get; set; }
        public string name { get; set; }
        public List<double> scale { get; set; }
        public List<int> rotation { get; set; }
        public List<double> translation { get; set; }
    }

    public class Primitive
    {
        public Attributes attributes { get; set; }
        public int indices { get; set; }
    }

    public class RNAUI
    {
        public SRID SRID { get; set; }

        [JsonProperty("crs x")]
        public CrsX crsx { get; set; }

        [JsonProperty("crs y")]
        public CrsY crsy { get; set; }
        public Longitude longitude { get; set; }
        public Latitude latitude { get; set; }
    }

    public class Root
    {
        public Asset asset { get; set; }
        public int scene { get; set; }
        public List<Scene> scenes { get; set; }
        public List<Node> nodes { get; set; }
        public List<Mesh> meshes { get; set; }
        public List<Accessor> accessors { get; set; }
        public List<BufferView> bufferViews { get; set; }
        public List<Buffer> buffers { get; set; }
    }

    public class Scene
    {
        public JObject extras { get; set; }
        public string name { get; set; }
        public List<int> nodes { get; set; }
    }

    public class SRID
    {
        public string description { get; set; }
        public string @default { get; set; }
    }



}
