using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities.Enums;
using BD.PublicPortal.Core.Entities.Specifications;

namespace BD.PublicPortal.Application.BloodDonationRequests;


public record ListBloodDonationRequestsQuery(BloodDonationRequestSpecificationFilter? filter = null, Guid? LoggedUserID = null, int? Level = null): IQuery<Result<IEnumerable<BloodDonationRequestDTO>>>;
