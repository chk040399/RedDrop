#nullable disable

using BD;

namespace BD.PublicPortal.Core.DTOs
{

    public partial class CommuneDTO
    {
        #region Constructors

        public CommuneDTO() {
        }

        public CommuneDTO(int id, string name, int wilayaId, WilayaDTO wilaya, List<ApplicationUserDTO> applicationUsers) {

          this.Id = id;
          this.Name = name;
          this.WilayaId = wilayaId;
          this.Wilaya = wilaya;
          this.ApplicationUsers = applicationUsers;
        }

        #endregion

        #region Properties

        public int Id { get; set; }

        public string Name { get; set; }

        public int WilayaId { get; set; }

        #endregion

        #region Navigation Properties

        public WilayaDTO Wilaya { get; set; }

        public List<ApplicationUserDTO> ApplicationUsers { get; set; }

        #endregion
    }

}
