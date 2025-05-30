using BD.PublicPortal.Core.DTOs;

namespace BD.PublicPortal.Application.Dashboard;

public record GetDashboardStatsQuery(
  Guid? LoggedUserId = null
) : IQuery<Result<DashboardStatsDTO>>;
