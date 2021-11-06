using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace task3
{
    class Program
    {
        static int Menu(string[] move, string hash)
        {
            int choice;
            do
            {
                Console.Clear();
                Console.WriteLine("HMAC: " + hash + "\nAvailable move:");
                for (int i = 0; i < move.Length; i++)
                {
                    Console.WriteLine("{0} - {1}", i + 1, move[i]);
                }
                Console.Write("0 - Exit" + "\nEnter your move: ");
                if (!int.TryParse(Console.ReadLine(), out choice) || choice > 5)
                {
                    continue;
                }
                else if (0 == choice)
                    Environment.Exit(0);
                else
                {
                    Console.WriteLine("\nYour move: {0}", move[choice - 1]);
                    break;
                }
            } while (true);
            return choice - 1;
        }
        static void Main(string[] args)
        {
            if (ArgsValid(args))
            {
                int pcMove = RandomNumber(args.Length);
                byte[] key = RandomKey();
                string hash = GetHash(pcMove, key);
                int userMove = Menu(args, hash);
                Console.WriteLine("Computer move: {0}", args[pcMove]);
                GameMoves(args.Length, userMove, pcMove);
                string tempKey = String.Join("", BitConverter.ToString(key).Split('-'));
                Console.WriteLine("HMAC key: {0}\n", tempKey);
            }
            else 
                Console.WriteLine("Error!");
        }
        static void GameMoves(int countMove, int userMove, int pcMove)
        {
            if (userMove == pcMove)
            {
                Console.WriteLine("\nDraw\n");
                return;
            }
            int aver = (int)(countMove / 2) + 1;
            if ((pcMove > userMove && pcMove < userMove + aver) ||
                (pcMove + countMove > userMove && pcMove + countMove < userMove + aver))
            {
                Console.WriteLine("\nLose\n");
                return;
            }
            Console.WriteLine("\nWin!\n");
        }
        static int RandomNumber(int countMove)
        {
            int number;
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] byteRandom = new byte[1];
                rng.GetBytes(byteRandom);
                number = (int)(byteRandom[0] / 255d * countMove);
            }
            return number;
        }
        static byte[] RandomKey()
        {
            byte[] key = new byte[16];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(key);
            }
            return key;
        }
        static string GetHash(int number, byte[] key)
        {
            using (HMACSHA256 hmac = new HMACSHA256(key))
            {
                byte[] data = hmac.ComputeHash(BitConverter.GetBytes(number));
                var strBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    strBuilder.Append(data[i].ToString("x2"));
                }
                return strBuilder.ToString();
            }
        }
        static bool ArgsValid(string[] args)
        {
            if (args.Length < 3 || args.Length % 2 == 0)
                return false;
            var set = new HashSet<string>();
            foreach (var item in args)
                if (!set.Add(item))
                    return false;
            return true;
        }
    }
}