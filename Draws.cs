using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrackPIN
{
    public class Draws
    {
        public List<int> elements;
        public int k;
        public int n;
        public List<Draw> draws;
        public double[][] probs; // k arrays woth array of size n
        public double[] probs_max_k;
        public double[] probs_max_i_k;
        public bool[] isOn; //
        public bool[] isOnPrev; //
        //public List<bool[]> condts;
        public int[] guess;
        public int[] last_guess;
        public int last_exact_cnt = 0;
        public int last_exist_cnt = 0;
        public List<string> submitted = new List<string>();
        public int cnt_left;

        public Draws(int k, int n, bool repetition = false)
        {
            this.k = k;
            this.n = n;
            elements = new List<int>();
            draws = new List<Draw>();
            probs_max_k = new double[k];
            probs = new double[k][]; // n rows, k cols
            last_guess = new int[k];
            for (int i_k = 0; i_k < k; i_k++)
            {
                probs[i_k] = new double[n];
                last_guess[i_k] = -1;
            }
            ResetGuess();
            for (int i = 0; i < n; i++)
            {
                elements.Add(i);
            }

            int n_perm = (int)Math.Pow(n, k);
            for (int i = 0; i < n_perm; i++)
            {
                draws.Add(new Draw(k, n));
            }

            for (int i_pos = 0; i_pos < k; i_pos++)
            {
                int i_len = (int)Math.Pow(n, i_pos);
                int i_draw = 0;
                while (i_draw < n_perm)
                {
                    foreach (int el in elements)
                    {
                        for (int i = 0; i < i_len; i++)
                        {
                            draws[i_draw].elements[i_pos] = el;
                            i_draw++;
                        }
                    }
                }
            }

            List<Draw> toremove = new List<Draw>();
            foreach (var draw in draws)
            {
                draw.Count();
                if (!repetition && draw.count.Max() > 1)
                {
                    toremove.Add(draw);
                }
            }

            foreach (Draw d in toremove)
            {
                draws.Remove(d);
            }

            isOn = new bool[draws.Count];
            isOnPrev = new bool[draws.Count];
            for (int i = 0; i < isOnPrev.Length; i++)
            {
                isOnPrev[i] = true;
            }

            Console.WriteLine();

        }

        public void CountProbs()
        {
            ResetCountProbs();

            double count_isOn = 0;
            for (int i = 0; i < draws.Count; i++)
            {
                isOn[i] = isOnPrev[i];

                for (int i_k = 0; i_k < k; i_k++)
                {
                    if (guess[i_k] >= 0)
                    {
                        isOn[i] = isOn[i] && (draws[i].elements[i_k] == guess[i_k]);
                    }
                }

                if (isOn[i])
                {
                    count_isOn++;
                    for (int i_k = 0; i_k < k; i_k++)
                    {
                        probs[i_k][draws[i].elements[i_k]]++;
                    }
                }
            }

            for (int i_k = 0; i_k < k; i_k++)
            {
                for (int i_n = 0; i_n < n; i_n++)
                {
                    probs[i_k][i_n] = probs[i_k][i_n] / count_isOn;
                }
            }

            for (int i_k = 0; i_k < k; i_k++)
            {
                if (guess[i_k] < 0)
                {
                    probs_max_k[i_k] = probs[i_k].Max();
                }
                else
                {
                    probs_max_k[i_k] = -1;
                }
            }

            int i_which_max_k = Array.IndexOf(probs_max_k, probs_max_k.Max());
            int i_which_max_n = Array.IndexOf(probs[i_which_max_k], probs[i_which_max_k].Max());

            guess[i_which_max_k] = elements[i_which_max_n];

            //Console.WriteLine("(permutations possible = {0})", count_isOn);
        }

        public void ResetCountProbs()
        {
            probs = new double[k][]; // n rows, k cols
            for (int i_k = 0; i_k < k; i_k++)
            {
                probs[i_k] = new double[n];
            }
        }

        public void PrintProbs()
        {
            Console.WriteLine("____________________________________");
            Console.Write("\t");
            for (int i_k = 0; i_k < k; i_k++)
            {
                Console.Write(i_k);
                Console.Write("\t");
            }
            Console.WriteLine();

            for (int i_n = 0; i_n < n; i_n++)
            {
                Console.Write(i_n);
                Console.Write("\t");
                for (int i_k = 0; i_k < k; i_k++)
                {
                    Console.Write("{0:0.00}", probs[i_k][i_n]);
                    Console.Write("\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine("____________________________________");

            Console.Write("\t");
            for (int i_k = 0; i_k < k; i_k++)
            {
                Console.Write(probs_max_k[i_k].ToString("0.00"));
                Console.Write("\t");
            }
            Console.WriteLine(" - max prob of digit per position");

            Console.Write("\t");
            for (int i_k = 0; i_k < k; i_k++)
            {
                if (guess[i_k] >= 0)
                {
                    Console.Write(guess[i_k]);
                }
                else
                {
                    Console.Write("?");
                }
                Console.Write("\t");
            }
            Console.WriteLine(" - best guess");

        }

        public void ResetGuess()
        {
            guess = new int[k];
            for (int i = 0; i < k; i++)
            {
                guess[i] = -1;
            }
        }

        public bool isGuessComplete()
        {
            int cnt = 0;
            for (int i_k = 0; i_k < k; i_k++)
            {
                if (guess[i_k] >= 0)
                {
                    cnt++;
                }
                //cnt = cnt + guess[i_k] >= 0 ? 1 : 0;
            }
            return (cnt == k);
        }

        public void PrintGuess()
        {
            for (int i = 0; i < k; i++)
            {
                Console.Write(guess[i] == -1 ? "_" : guess[i].ToString());
                Console.Write(" ");
            }
            Console.WriteLine();
        }

        public void UpdateIsOn()
        {
            cnt_left = 0;
            for (int i = 0; i < isOn.Length; i++)
            {
                isOnPrev[i] = isOnPrev[i] && 
                    (CountExist(last_guess, draws[i].elements) == (last_exist_cnt + last_exact_cnt)) && 
                    (CountExact(last_guess, draws[i].elements) == last_exact_cnt);
                if (isOnPrev[i])
                {
                    cnt_left++;
                }
            }
            Console.WriteLine("permutations possible = {0}", cnt_left);
        }

        public int CountExact(int[] guess, int[] ldraw)
        {
            int cnt = 0;
            for (int i = 0; i < k; i++)
            {
                if (guess[i] == ldraw[i])
                {
                    cnt++;
                }
            }
            return cnt;
        }

        public int CountExist(int[] guess, int[] ldraw)
        {
            int cnt = 0;
            for (int i1 = 0; i1 < k; i1++)
            {
                for (int i2 = 0; i2 < k; i2++)
                {
                    if (guess[i1] == ldraw[i2])
                    {
                        cnt++;
                    }
                }
            }
            return cnt;
        }

        public string GuessToString()
        {
            string s = "";
            for (int i = 0; i < k; i++)
            {
                s += guess[i].ToString();
            }
            return (s);
        }
    }
}
