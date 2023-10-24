using System.Numerics;
using Console.Utility;

namespace Console;

public class Program
{
    public static void Main(string[] args)
    {
           
        System.Console.WriteLine(AuthUtility.CalculateToTalPermission(Enumerable.Range(0, 129)));
        System.Console.WriteLine(AuthUtility.ConvertToBinary(AuthUtility.CalculateToTalPermission(Enumerable.Range(0, 129))));
        System.Console.WriteLine(AuthUtility.ComparePermissionAsString(AuthUtility.CalculateToTalPermission(Enumerable.Range(0, 129)), 
            AuthUtility.FromExponentToPermission((int)7)));
    }
}

