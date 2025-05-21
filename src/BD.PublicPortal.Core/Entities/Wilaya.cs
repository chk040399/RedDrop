#nullable disable


using BD;

namespace BD.PublicPortal.Core.Entities
{
    public partial class Wilaya : EntityBase<int>, IAggregateRoot {

        public Wilaya()
        {
      BloodTansfusionCenters = new List<BloodTansfusionCenter>();
      Communes = new List<Commune>();
            OnCreated();
        }


        public string Nom { get; set; }

        public virtual IList<BloodTansfusionCenter> BloodTansfusionCenters { get; set; }

        public virtual IList<Commune> Communes { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
