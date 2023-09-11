using System.ComponentModel;

namespace SharedKernel.Application;

public enum AccountState
{
    [Description("Activated")]
    Activated = 1,

    [Description("NotActivated")]
    NotActivated = 2,

    [Description("Blocked")]
    Blocked = 3,
}