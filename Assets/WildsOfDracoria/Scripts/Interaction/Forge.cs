using WildsOfDracoria.Crafting;

namespace WildsOfDracoria.Interaction
{
    public class Forge : CraftingStation
    {
        private void Reset()
        {
            Configure(CraftingStationType.Forge);
        }

        private void Awake()
        {
            Configure(CraftingStationType.Forge);
        }
    }
}