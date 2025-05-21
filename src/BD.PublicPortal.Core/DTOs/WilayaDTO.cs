#nullable disable

using BD;

namespace BD.PublicPortal.Core.DTOs
{

    public partial class WilayaDTO
    {
        #region Constructors

        public WilayaDTO() {
        }

        public WilayaDTO(int id, string nom, List<BloodTansfusionCenterDTO> bloodTansfusionCenters, List<CommuneDTO> communes) {

      Id = id;
      Nom = nom;
      BloodTansfusionCenters = bloodTansfusionCenters;
      Communes = communes;
        }

        #endregion

        #region Properties

        public int Id { get; set; }

        public string Nom { get; set; }

        #endregion

        #region Navigation Properties

        public List<BloodTansfusionCenterDTO> BloodTansfusionCenters { get; set; }

        public List<CommuneDTO> Communes { get; set; }

        #endregion
    }

}
