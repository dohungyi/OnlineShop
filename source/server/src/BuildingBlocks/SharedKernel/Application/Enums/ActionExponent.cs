﻿namespace SharedKernel.Application;

public enum ActionExponent : int
{
    AllowAnonymous = -1,
    Staff = 128,
    Admin = 64,
    View = 0,
    Add = 1,
    Edit = 2,
    Delete = 3,
    Export = 4,
    Import = 5,
    Upload = 6,
    Download = 7
}