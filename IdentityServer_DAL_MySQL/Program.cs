using IdentityServer_DAL_MySQL.MenegmentData;
using System;

namespace IdentityServer_DAL_MySQL
{
    public class Program
    {
        public static void Main(string[] args)
        {
                Console.ForegroundColor= ConsoleColor.Green;
                Console.WriteLine($"�����i�� ���i��� ������:");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"1: ���������� ���� ������ �������");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"2: ��������� � ����, ������");
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

