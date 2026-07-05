using UnityEngine;

namespace WildsOfDracoria.Markets
{
    public class VendorStallSceneBootstrap : MonoBehaviour
    {
        [SerializeField] private bool createIfMissing = true;

        private void Start()
        {
            if (!createIfMissing || Object.FindAnyObjectByType<VendorStall>() != null)
            {
                return;
            }

            CreateIronhavenMarket();
        }

        private void CreateIronhavenMarket()
        {
            var fishMaterial = MakeMaterial("Market_FishStall", new Color(0.16f, 0.42f, 0.62f));
            var forgeMaterial = MakeMaterial("Market_ForgeStall", new Color(0.58f, 0.24f, 0.16f));
            var foodMaterial = MakeMaterial("Market_FoodStall", new Color(0.58f, 0.42f, 0.18f));

            CreateStall(MarketIds.IronhavenFishStall, "Ironhaven Fish Stall", new Vector3(-3.5f, 0.65f, 3.5f), fishMaterial);
            CreateStall(MarketIds.TorensForgeStall, "Toren's Forge Stall", new Vector3(0f, 0.65f, 3.5f), forgeMaterial);
            CreateStall(MarketIds.MarasFoodStall, "Mara's Food Stall", new Vector3(3.5f, 0.65f, 3.5f), foodMaterial);
        }

        private static void CreateStall(string stallId, string name, Vector3 position, Material material)
        {
            var stallObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            stallObject.name = name;
            stallObject.transform.position = position;
            stallObject.transform.localScale = new Vector3(2.2f, 1.1f, 1.2f);

            var renderer = stallObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = material;
            }

            var collider = stallObject.GetComponent<Collider>();
            if (collider != null)
            {
                collider.isTrigger = true;
            }

            var stall = stallObject.AddComponent<VendorStall>();
            stall.Configure(stallId);

            var signObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            signObject.name = name + " Sign";
            signObject.transform.SetParent(stallObject.transform, false);
            signObject.transform.localPosition = new Vector3(0f, 0.85f, -0.55f);
            signObject.transform.localScale = new Vector3(0.9f, 0.25f, 0.08f);
            var signRenderer = signObject.GetComponent<Renderer>();
            if (signRenderer != null)
            {
                signRenderer.sharedMaterial = material;
            }

            var signCollider = signObject.GetComponent<Collider>();
            if (signCollider != null)
            {
                Object.Destroy(signCollider);
            }
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
