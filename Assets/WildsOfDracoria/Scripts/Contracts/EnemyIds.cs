namespace WildsOfDracoria.Contracts
{
    public static class EnemyIds
    {
        public const string ForestWolf = "forest_wolf";

        public static string Normalize(string enemyIdOrName)
        {
            if (string.IsNullOrWhiteSpace(enemyIdOrName))
            {
                return string.Empty;
            }

            switch (enemyIdOrName.Trim().ToLowerInvariant())
            {
                case "forest wolf":
                case ForestWolf:
                    return ForestWolf;
                default:
                    return enemyIdOrName.Trim().ToLowerInvariant().Replace(" ", "_");
            }
        }
    }
}
