using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using BD.PublicPortal.Api.Extensions;
using BD.PublicPortal.Application.Identity.Register;
using BD.PublicPortal.Infrastructure.Services.Identity;
using FastEndpoints;
using FluentValidation;
using MediatR;
using ProblemDetails = FastEndpoints.ProblemDetails;


namespace BD.PublicPortal.Api.Features.IdentityManagement.Users.Register;

public class RegisterUserRequest:RegisterUserDto
{

}

public class RegisterUserResponse
{
  public Guid UserId { get; set; } = default!;
}


public class RegisterUserValidator : Validator<RegisterUserRequest>
{
  public RegisterUserValidator()
  {
    RuleFor(x => x.Email).NotEmpty().EmailAddress();
    RuleFor(x => x.UserName).NotEmpty();
    RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    RuleFor(x => x.ConfirmPassword)
      .Equal(x => x.Password).WithMessage("Passwords do not match.");
    RuleFor(x => x.DonorNIN).NotEmpty().MinimumLength(18).MaximumLength(18);
    RuleFor(x => x.DonorBirthDate).NotEmpty();
    RuleFor(x => x.DonorName).NotEmpty();
    RuleFor(x => x.DonorBloodGroup).NotEmpty();
  }
}


public class Register : Endpoint<RegisterUserRequest, RegisterUserResponse>
{
    private readonly IMediator _mediator;

    public Register(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("auth/register2");
        AllowAnonymous();
        Summary(s => s.Summary = "Register a new user.");
    }

    public override async Task HandleAsync(RegisterUserRequest req, CancellationToken ct)
    {
      var result = await _mediator.Send(new RegisterUserCommand(req), ct);

    //await SendResultAsync(result.ToMinimalApiResult());

    if (result.IsSuccess)
    {
      // await SendResultAsync(result.ToMinimalApiResult());
      await SendOkAsync(new RegisterUserResponse { UserId = result.Value! });
    }
    else
    {
      //var pd = result.ToProblemDetails(HttpContext);
      //await SendAsync(pd, pd.Status);
      var pd = result.ToProblemDetails(HttpContext);
      HttpContext.Response.StatusCode = pd.Status;
      await HttpContext.Response.WriteAsJsonAsync(pd);
    }
  }
}
