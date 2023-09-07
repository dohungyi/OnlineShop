using System.ComponentModel;

namespace SharedKernel.Application;

public static class Enum
{
    public enum OtpType
    {
        [Description("None")]
        None = 0,

        [Description("For password")]
        Password = 1,

        [Description("For verify")]
        Verify = 2,

        [Description("Multi-factor Authentication")]
        MFA = 3,
    }

    public enum MFAType
    {
        [Description("None")]
        None = 0,

        [Description("Use email")]
        Email = 1,

        [Description("Use phonenumber")]
        Phone = 2,
    }

    public enum AccountState
    {
        [Description("Actived")]
        Actived = 1,

        [Description("NotActived")]
        NotActived = 2,

        [Description("Blocked")]
        Blocked = 3,
    }

    public enum GenderType
    {
        [Description("Other")]
        Other = 0,

        [Description("Female")]
        Female = 1,

        [Description("Male")]
        Male = 2,
    }

    public enum HttpStatusCodeExtension
    {
        AuthenticationRequired = 432,
        NotVerified = 801,
    }

    public enum EntityState
    {
        Add = 1,
        Edit = 2,
        Delete = 3
    }

    public enum ChangeType
    {
        Add = 1,
        Edit = 2,
        Delete = 3,
    }

    public enum ValidateCode : int
    {
        Required = 1,
        Duplicate = 2,
        Invalid = 3,
        EntityNull = 4,
    }

    public enum RoleId : long
    {
        SA = 1,
        ADMIN = 2,
        EMPLOYEE = 3
    }

    public enum ActionExponent : int
    {
        AllowAnonymous = -1,
        SA = 128,
        Admin = 64,
        View = 0,
        Add = 1,
        Edit = 2,
        Delete = 3,
        Export = 4,
        Import = 5,
        Upload = 6,
        Download = 7,
        Process = 8,
        Cloud = 9,
        ChatGenerator = 10,
        Notebook = 11,
    }

    public enum FileType
    {
        None = 0,
        Image = 1,
        Video = 2,
        Other = 3,
    }
}