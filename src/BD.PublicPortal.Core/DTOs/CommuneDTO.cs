#nullable disable

using BD;

namespace BD.PublicPortal.Core.DTOs
{

    public partial class CommuneDTO
    {
        #region Constructors

        public CommuneDTO() {
        }

        public CommuneDTO(int id, string name, int wilayaId, List<System.Guid> applicationUsers) {

          this.Id = id;
          this.Name = name;
          this.WilayaId = wilayaId;
          this.ApplicationUsers = applicationUsers;
        }

        #endregion

        #region Properties

        public int Id { get; set; }

        public string Name { get; set; }

        public int WilayaId { get; set; }

        #endregion

        #region Navigation Properties

        public List<System.Guid> ApplicationUsers { get; set; }

        #endregion
    }

}
