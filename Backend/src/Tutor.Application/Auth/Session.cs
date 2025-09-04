using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using Tutor.Domain.Entities.Common;
using Interfaces_ISession = Tutor.Domain.Auth.Interfaces.ISession;
using ISession = Tutor.Domain.Auth.Interfaces.ISession;

namespace Tutor.Application.Auth;

public class Session : Interfaces_ISession
{
    public UserId UserId { get; private init; }

    public DateTime Now => DateTime.Now;

    public Session(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User;

        var nameIdentifier = user?.FindFirst(ClaimTypes.NameIdentifier);

        if(nameIdentifier != null)
        {
            UserId = new Guid(nameIdentifier.Value);
        }
    }

}