using IdentityServer_DAL_MySQL.MenegmentData;
using System;

namespace IdentityServer_DAL_MySQL
{
    public class Program
    {
        public static void Main(string[] args)
        {
                Console.ForegroundColor= ConsoleColor.Green;
                Console.WriteLine($"Виберiть варiант роботи:");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"1: Заповнення бази данних данними");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"2: Видалення з бази, данних");
                Console.ResetColor();

                int input = Convert.ToInt32( Console.ReadLine() );

                switch (input)
                {
                    case 1:
                    SeedData.EnsureSeedData(Constants.ConnectionMySQL);
                    break; 
                    case 2:
                    DeleteData.DeleteAllUsers(Constants.ConnectionMySQL);
                    break;
                    default:
                    break;
                }
        }
    }
}

