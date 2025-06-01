using BD.Central.Core.DTOs;
using BD.Central.Core.Entities.Specifications;

namespace BD.Central.Application.Identity.List;

public record ListApplicationUsersQuery(ApplicationUserSpecificationFilter? filter = null, int? Level = null)
  : IQuery<Result<IEnumerable<ApplicationUserDTO>>>;
