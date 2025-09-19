using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Interfaces;

namespace Tutor.Application.Features.Admin.CreateAdmin;

public class CreateAdminHandler : IRequestHandler<CreateAdminCommand, Result>
{
    private readonly IUserService  _userService;

    public CreateAdminHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<Result> Handle(CreateAdminCommand request, CancellationToken cancellationToken)
    {
      return  await _userService.CreateAdminAsync(request.UserId);
    }
}