using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Core.DTOs;

namespace BD.PublicPortal.Application.Donor;

public class UpdateProfileHandler(IRepository<ApplicationUser> _userRepository)
  : ICommandHandler<UpdateProfileCommand, Result<ApplicationUserDTO>>
{
  public async Task<Result<ApplicationUserDTO>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
  {
    var existingUser = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
    
    if (existingUser == null)
    {
      return Result.NotFound();
    }

    request.UpdateUserDTO.UpdateEntity(existingUser);
    await _userRepository.UpdateAsync(existingUser, cancellationToken);
    var userDto = existingUser.ToDto();

    return Result.Success(userDto);
  }
}
