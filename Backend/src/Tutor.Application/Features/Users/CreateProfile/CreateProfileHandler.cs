using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Features.Users.Dtos;
using Tutor.Domain.Entities;
using Tutor.Domain.Interfaces;

namespace Tutor.Application.Features.Users.CreateProfile;

    public class UpdateProfileCommandHandler : IRequestHandler<CreateProfileCommand, Result<CreateProfileDto>>
    {
        private readonly IGenericRepository<User, int> _dbContext;

        public UpdateProfileCommandHandler(IGenericRepository<User, int> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<CreateProfileDto>> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.GetById(request.UserId);

            if (user == null)
                return Result<CreateProfileDto>.NotFound();

            // Update user properties from DTO
            user.Phone = request.ProfileDto.Phone;
            user.FirstName = request.ProfileDto.FirstName;
            user.LastName = request.ProfileDto.LastName;
            user.Bio = request.ProfileDto.Bio;
            user.Birthdate = request.ProfileDto.Birthdate;
            user.Username = request.ProfileDto.Username;

            await _dbContext.Update(user);
            

            return Result.Success(request.ProfileDto);
        }
    }
