using System;
using Tutor.Domain.Entities.Common;
using Tutor.Domain.Entities.Enums;
using HeroId = Tutor.Domain.Entities.Common.HeroId;

namespace Tutor.Domain.Entities;

public class Hero : Entity<HeroId>
{
    public override HeroId Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = null!;

    public string? Nickname { get; set; }
    public string? Individuality { get; set; } = null!;
    public int? Age { get; set; }

    public HeroType HeroType { get; set; }

    public string? Team { get; set; }
}