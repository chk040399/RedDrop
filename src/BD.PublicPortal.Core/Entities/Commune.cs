#nullable disable


using BD;

namespace BD.PublicPortal.Core.Entities
{
    public partial class Commune : EntityBase<int>, IAggregateRoot {

        public Commune()
        {
      ApplicationUsers = new List<ApplicationUser>();
            OnCreated();
        }


        public string Nom { get; set; }

        public int WilayaId { get; set; }

        public virtual Wilaya Wilaya { get; set; }

        public virtual IList<ApplicationUser> ApplicationUsers { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
