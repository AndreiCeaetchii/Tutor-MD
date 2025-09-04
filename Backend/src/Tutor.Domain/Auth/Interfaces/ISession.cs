using System;
using UserId = Tutor.Domain.Entities.Common.UserId;

namespace Tutor.Domain.Auth.Interfaces;

public interface ISession
{
    public UserId UserId { get; }

    public DateTime Now { get; }
}