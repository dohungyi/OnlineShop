namespace OnlineShop.Domain.Entities;

public class ApplicationUserConfig : UserConfig
{
    #region [REFRENCE PROPERTIES]

    public virtual ApplicationUser User { get; set; }

    #endregion
}