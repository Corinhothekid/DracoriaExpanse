namespace WildsOfDracoria.Contracts
{
    public static class ContractIds
    {
        public const string FreshFishForIronhaven = "fresh_fish_for_ironhaven";
        public const string TimberForDockRepairs = "timber_for_dock_repairs";
        public const string CopperForTheForge = "copper_for_the_forge";
        public const string WolvesInTheWheat = "wolves_in_the_wheat";
        public const string ProperTrainingBlade = "proper_training_blade";

        public static string Normalize(string contractId)
        {
            return string.IsNullOrWhiteSpace(contractId) ? string.Empty : contractId.Trim().ToLowerInvariant().Replace(" ", "_");
        }
    }
}
