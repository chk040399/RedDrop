namespace BD.PublicPortal.Core.Entities.Enums
{
    public enum BloodDonationPladgeEvolutionStatus : int
    {
        Initiated = 0,
        Honored = 1,
        CanceledByInitiaor = 2,
        CanceledByServiceNotNeeded = 3,
        CanceledByServiceCantBeDone = 4,
        CanceledTimeout = 5
    }
}
