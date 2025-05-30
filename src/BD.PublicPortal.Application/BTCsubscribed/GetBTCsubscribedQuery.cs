using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities.Specifications;

namespace BD.PublicPortal.Application.BTCsubscribed;
public record GetBTCsubscribedQuery(
  Guid LoggedUserId,
  int? PaginationTake = null,
  int? Level = null
) : IQuery<Result<IEnumerable<DonorBloodTransferCenterSubscriptionsDTO>>>;
