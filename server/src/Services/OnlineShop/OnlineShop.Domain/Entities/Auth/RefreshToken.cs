namespace OnlineShop.Domain.Entities;

public class RefreshToken : PersonalizedEntity
{
    public string RefreshTokenValue { get; set; }

    public string CurrentAccessToken { get; set; }

    public DateTime ExpirationDate { get; set; }
    
    #region [REFRENCE PROPERTIES]
    
    public virtual ApplicationUser User { get; set; }
    
    #endregion
}