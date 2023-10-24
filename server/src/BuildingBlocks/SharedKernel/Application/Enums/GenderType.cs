using System.ComponentModel;

namespace SharedKernel.Application;

public enum GenderType
{
    [Description("Other")]
    Other = 0,

    [Description("Female")]
    Female = 1,

    [Description("Male")]
    Male = 2,
}