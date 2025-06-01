using BD.Central.Core.DTOs;
using BD.Central.Core.Entities.Specifications;
using BD.Central.Core.Entities.Enums;

namespace BD.Central.Application.BloodDonationRequests;


public record ListBloodDonationRequestsQuery(BloodDonationRequestSpecificationFilter? filter = null, int? Level = null): IQuery<Result<IEnumerable<BloodDonationRequestDTO>>>;
