using Tutor.Domain.Entities.Common;

namespace Tutor.Domain.Entities;

public class Password:Entity<int>
{
    public int UserId { get; set; } // 1:1 with users
    public string PasswordHash { get; set; }
    
    //Nav properties
    public User User { get; set; }

}