#nullable disable



using System.ComponentModel.DataAnnotations.Schema;
using BD;
using BD.PublicPortal.Core.Entities.Events;


namespace BD.PublicPortal.Core.Entities
{
    public partial class ApplicationUser : IdentityUser<Guid>, IAggregateRoot, IHasDomainEvents {

        public ApplicationUser()
        {
            this.DonorBloodTransferCenterSubscriptions = new List<DonorBloodTransferCenterSubscriptions>();
            this.BloodDonationPledges = new List<BloodDonationPledge>();

            RegisterDomainEvent(new ApplicationUserEvent(this));
            OnCreated();
        }



        public Guid? DonorCorrelationId { get; set; }

        public bool? DonorWantToStayAnonymous { get; set; }

        public bool? DonorExcludeFromPublicPortal { get; set; }

        public DonorAvailability? DonorAvailability { get; set; }

        public DonorContactMethod? DonorContactMethod { get; set; }

        public string DonorName { get; set; }

        public DateTime DonorBirthDate { get; set; }

        public BloodGroup DonorBloodGroup { get; set; }

        public string DonorNIN { get; set; }

        public string DonorTel { get; set; }

        public string DonorNotesForBTC { get; set; }

        public DateTime? DonorLastDonationDate { get; set; }

        public int? CommuneId { get; set; }

        public virtual IList<DonorBloodTransferCenterSubscriptions> DonorBloodTransferCenterSubscriptions { get; set; }

        public virtual IList<BloodDonationPledge> BloodDonationPledges { get; set; }

        public virtual Commune Commune { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

    #endregion

        private readonly List<DomainEventBase> _domainEvents = new();
        [NotMapped]
        public IReadOnlyCollection<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

        protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);
        internal void ClearDomainEvents() => _domainEvents.Clear();

    void IHasDomainEvents.RegisterDomainEvent(DomainEventBase domainEvent)
    {
      RegisterDomainEvent(domainEvent);
    }

    void IHasDomainEvents.ClearDomainEvents()
    {
      ClearDomainEvents();
    }
  }

}
