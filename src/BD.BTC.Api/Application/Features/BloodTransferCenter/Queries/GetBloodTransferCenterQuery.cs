using MediatR;
using Application.DTOs;
using Shared.Exceptions;

namespace Application.Features.BloodTransferCenterManagement.Queries
{
    public class GetBloodTransferCenterQuery : IRequest<(BloodTransferCenterDTO? center, BaseException? err)>
    {
        // No parameters needed since we're fetching the single center
    }
}