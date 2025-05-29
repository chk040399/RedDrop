using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities.Enums;
using BD.PublicPortal.Core.Entities.Specifications;

namespace BD.PublicPortal.Application.Pledges;

public record ListPledgesQuery(  
    Guid LoggedUserID,
    BloodDonationPledgeSpecificationFilter? Filter = null
) : IQuery<Result<IEnumerable<BloodDonationPledgeDTO>>>;
