namespace Claims.Application.Validators
{
    public static class ClaimErrorMessages
    {
        public const string DamageCostTooHigh = "Damage cost cannot exceed more than 1000";
        public const string InvalidCreationDate = "Claim is created outside of insurance period";
    }
}
