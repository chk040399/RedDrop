#nullable disable

using BD;

namespace BD.Central.Core.DTOs
{

    public partial class WilayaDTO
    {
        #region Constructors

        public WilayaDTO() {
        }

        public WilayaDTO(int id, string name, List<BloodTansfusionCenterDTO> bloodTansfusionCenters, List<CommuneDTO> communes) {

      Id = id;
      Name = name;
      BloodTansfusionCenters = bloodTansfusionCenters;
      Communes = communes;
        }

        #endregion

        #region Properties

        public int Id { get; set; }

        public string Name { get; set; }

        #endregion

        #region Navigation Properties

        public List<BloodTansfusionCenterDTO> BloodTansfusionCenters { get; set; }

        public List<CommuneDTO> Communes { get; set; }

        #endregion
    }

}
