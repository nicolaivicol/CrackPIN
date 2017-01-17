using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrackPIN
{
    class Program
    {
        static void Main(string[] args)
        {
            Draws d = new Draws(4, 10);

            Console.WriteLine("Algorithm to find a PIN of 4 digits, non-repeating.");
            Console.WriteLine("Game from: https://meduza.io/games/bud-hakerom-igra-meduzy");
            Console.WriteLine("permutations possible = {0}", d.draws.Count);
            Console.WriteLine();

            while (d.last_exact_cnt < d.k)
            {
                d.ResetGuess();
                while (!d.isGuessComplete())
                {
                    d.CountProbs();
                    //d.PrintProbs();
                }
                Console.WriteLine("best guess: {0}", d.GuessToString());
                Console.Write("submit ? (Hit enter for best guess or type your guess) : ");
                string s = Console.ReadLine();
                if (s == "")
                {
                    Array.Copy(d.guess, d.last_guess, d.k);
                }
                else
                {
                    Console.WriteLine("(!) processing your guess not implemented. Algorithm takes best guess as submitted.");
                    Array.Copy(d.guess, d.last_guess, d.k);
                }
                d.submitted.Add(d.GuessToString());
                Console.Write("give exact pos matches: ");
                d.last_exact_cnt = Convert.ToInt32(Console.ReadLine());
                if (d.last_exact_cnt == d.k)
                {
                    Console.WriteLine();
                    Console.WriteLine("PIN found in {0} attempts.", d.submitted.Count);
                    for (int i = 0; i < d.submitted.Count; i++)
                    {
                        if (i == (d.submitted.Count - 1))
                        {
                            Console.Write("[{0}]", d.submitted[i]);
                        }
                        else
                        {
                            Console.Write("{0} ", d.submitted[i]);
                        }
                    }
                    Console.ReadLine();
                    return;
                }
                Console.Write("give non-exact pos matches: ");
                d.last_exist_cnt = Convert.ToInt32(Console.ReadLine());
                d.UpdateIsOn();
                if (d.cnt_left == 0)
                {
                    Console.WriteLine("(!) Some erroneous feedbacks were given, mutually excluding each other. No possible PINs left. Start again.");
                    Console.ReadLine();
                    return;
                }
                Console.WriteLine();
            }

        }
    }
}
