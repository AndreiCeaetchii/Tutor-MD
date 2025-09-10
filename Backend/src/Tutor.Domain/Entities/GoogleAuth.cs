using Tutor.Domain.Entities.Common;

namespace Tutor.Domain.Entities;

public class GoogleAuth :Entity<int>
{
    public int UserId { get; set; } // 1:1 with users
    
    public string OAuthProvider { get; set; } = string.Empty;
    
    public string OAuthProviderId { get; set; } = string.Empty;

    
    
    
    //Nav Properties
    public virtual User User { get; set; }

}