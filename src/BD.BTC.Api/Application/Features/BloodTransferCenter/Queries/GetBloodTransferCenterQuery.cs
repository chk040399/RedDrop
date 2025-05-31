using MediatR;
using Application.DTOs;
using Shared.Exceptions;

namespace Application.Features.BloodTransferCenterManagement.Queries
{
    public record GetBloodTransferCenterQuery() : IRequest<(BloodTransferCenterDTO? center, BaseException? err)>;
}