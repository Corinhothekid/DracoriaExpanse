using UnityEngine;

namespace WildsOfDracoria.Gathering
{
    public class GatheringNodeSceneBootstrap : MonoBehaviour
    {
        [SerializeField] private bool createIfMissing = true;

        private void Start()
        {
            if (!createIfMissing || FindObjectOfType<GatheringNode>() != null)
            {
                return;
            }

            CreateIronhavenNodes();
        }

        private void CreateIronhavenNodes()
        {
            var copperMaterial = MakeMaterial("Gathering_Copper", new Color(0.72f, 0.38f, 0.18f));
            var oakMaterial = MakeMaterial("Gathering_Oak", new Color(0.16f, 0.42f, 0.16f));
            var wheatMaterial = MakeMaterial("Gathering_Wheat", new Color(0.86f, 0.72f, 0.28f));

            CreateNode(GatheringNodeRegistry.CopperVein, "Copper Vein", PrimitiveType.Sphere, new Vector3(-14f, 0.55f, -8f), new Vector3(1.25f, 0.75f, 1.25f), copperMaterial);
            CreateNode(GatheringNodeRegistry.CopperVein, "Copper Vein", PrimitiveType.Sphere, new Vector3(-16f, 0.55f, -5.5f), new Vector3(1.1f, 0.65f, 1.1f), copperMaterial);
            CreateNode(GatheringNodeRegistry.CopperVein, "Copper Vein", PrimitiveType.Sphere, new Vector3(-12.5f, 0.55f, -4.2f), new Vector3(1.0f, 0.6f, 1.0f), copperMaterial);

            CreateNode(GatheringNodeRegistry.OakTree, "Gatherable Oak Tree", PrimitiveType.Cylinder, new Vector3(13f, 1.1f, -11f), new Vector3(0.7f, 1.7f, 0.7f), oakMaterial);
            CreateNode(GatheringNodeRegistry.OakTree, "Gatherable Oak Tree", PrimitiveType.Cylinder, new Vector3(16f, 1.1f, -9f), new Vector3(0.7f, 1.7f, 0.7f), oakMaterial);
            CreateNode(GatheringNodeRegistry.OakTree, "Gatherable Oak Tree", PrimitiveType.Cylinder, new Vector3(14.5f, 1.1f, -5.8f), new Vector3(0.7f, 1.7f, 0.7f), oakMaterial);

            CreateNode(GatheringNodeRegistry.WheatPatch, "Wheat Patch", PrimitiveType.Cube, new Vector3(8.5f, 0.35f, -2.5f), new Vector3(2.2f, 0.22f, 1.5f), wheatMaterial);
            CreateNode(GatheringNodeRegistry.WheatPatch, "Wheat Patch", PrimitiveType.Cube, new Vector3(11.2f, 0.35f, -2.5f), new Vector3(2.2f, 0.22f, 1.5f), wheatMaterial);
        }

        private static void CreateNode(string nodeId, string name, PrimitiveType primitiveType, Vector3 position, Vector3 scale, Material material)
        {
            var nodeObject = GameObject.CreatePrimitive(primitiveType);
            nodeObject.name = name;
            nodeObject.transform.position = position;
            nodeObject.transform.localScale = scale;
            var renderer = nodeObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = material;
            }

            var collider = nodeObject.GetComponent<Collider>();
            if (collider != null)
            {
                collider.isTrigger = true;
            }

            var node = nodeObject.AddComponent<GatheringNode>();
            node.Configure(nodeId);
        }

        private static Material MakeMaterial(string name, Color color)
        {
            var material = new Material(Shader.Find("Standard"));
            material.name = name;
            material.color = color;
            return material;
        }
    }
}