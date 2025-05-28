using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities.Specifications;

namespace BD.PublicPortal.Application.Identity.List;

public record ListApplicationUsersQuery(ApplicationUserSpecificationFilter? filter = null, Guid? LoggedUserID = null, int? Level = null)
  : IQuery<Result<IEnumerable<ApplicationUserDTO>>>;
