using BD.Central.Core.DTOs;

namespace BD.Central.Application.Dashboard;

public record GetDashboardStatsQuery(
  Guid? LoggedUserId = null
) : IQuery<Result<DashboardStatsDTO>>;
