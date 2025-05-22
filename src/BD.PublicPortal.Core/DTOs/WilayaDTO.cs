#nullable disable

using BD;

namespace BD.PublicPortal.Core.DTOs
{

    public partial class WilayaDTO
    {
        #region Constructors

        public WilayaDTO() {
        }

        public WilayaDTO(int id, string name, List<System.Guid> bloodTansfusionCenters, List<int> communes) {

          this.Id = id;
          this.Name = name;
          this.BloodTansfusionCenters = bloodTansfusionCenters;
          this.Communes = communes;
        }

        #endregion

        #region Properties

        public int Id { get; set; }

        public string Name { get; set; }

        #endregion

        #region Navigation Properties

        public List<System.Guid> BloodTansfusionCenters { get; set; }

        public List<int> Communes { get; set; }

        #endregion
    }

}
