using System;
using System.Collections.Generic;

namespace ICPC
{
    internal class Program
    {
        static int MinimalPowerToCleanShelf(int n, string s)
        {
            List<char> charList = new List<char>(s.ToCharArray());
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            int counter = 0;
            int did = 0;
            int i = 0;
            for (int j = 0; j < charList.Count; j++)
            {
                if (charList[j] == '0')
                {
                    dictionary.Add(j, 1);
                }
            }

            while (i < n - 1 && i < charList.Count)
            {
            
                if (dictionary.ContainsKey(i))
                {
                    i++;
                    continue;
                }
                if (charList[i] == '0')
                {
                    dictionary.Add(i, 1);
                    i++;
                    continue;
                }
                else if (charList[i] == '1')
                {
                    try
                    {
                        if (charList[i + 1] == '0')
                        {
                            charList[i] = '0';
                            charList[i + 1] = '1';
                            dictionary.Add(i, 1);
                            did++;
                            i++;
                            continue;
                        }
                    }
                    catch { }

                    try
                    {
                        if (charList[i - 1] == '0')
                        {
                            charList[i] = '0';
                            charList[i - 1] = '1';
                            dictionary.Add(i, 1);
                            did++;
                            i++;
                            continue;
                        }
                    }
                    catch { }

                    i++;
                }
            }

            int k = n - 1;
            while (dictionary.Count != n)
            {
                if (k > 0 && charList[k] == '1' && charList[k - 1] == '1')
                {
                    k--;
                }
                else if (k > 0 && charList[k] == '1' && charList[k - 1] == '0')
                {
                    charList[k] = '0';
                    charList[k - 1] = '1';
                    did++;
                    if (!dictionary.ContainsKey(k))
                    {
                        dictionary.Add(k, 1);
                    }
                    k++;
                }
                else if (k == 0 && charList[k] == '1')
                {
                    charList[k] = '0';
                    did++;
                    if (!dictionary.ContainsKey(k))
                    {
                        dictionary.Add(k, 1);
                    }
                    k++;
                }
            }



            return did;
        }

        static void Main()
        {
            int t = int.Parse(Console.ReadLine());
            for (int i = 0; i < t; i++)
            {
                int n = int.Parse(Console.ReadLine());
                string s = Console.ReadLine();
                int result = MinimalPowerToCleanShelf(n, s);
                Console.WriteLine(result);
            }
        }
    }
}

