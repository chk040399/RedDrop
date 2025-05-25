namespace BD.PublicPortal.Core.Entities.Enums
{
    public enum DonorAvailability : int
    {
        [Display(Name="Matin")]
        Morning = 1,
        [Display(Name = "Après Midi")]
        Afternoon = 2,
        [Display(Name = "La journee")]
        Day = 3,
        [Display(Name = "Le Soir")]
        Night = 4,
        [Display(Name = "A Tout Moment")]
        AllTime = 5
    }
}
