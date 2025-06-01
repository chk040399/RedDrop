#nullable disable


using BD;
using BD.SharedKernel;

namespace BD.Central.Core.Entities
{
    public partial class Wilaya : EntityBase<int>, IAggregateRoot {

        public Wilaya()
        {
      BloodTansfusionCenters = new List<BloodTansfusionCenter>();
      Communes = new List<Commune>();
            OnCreated();
        }


        public string Name { get; set; }

        public virtual IList<BloodTansfusionCenter> BloodTansfusionCenters { get; set; }

        public virtual IList<Commune> Communes { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
