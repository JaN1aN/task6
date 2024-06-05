using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task<int> GetRandomNumberAsync(int max)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"https://www.randomnumberapi.com/api/v1.0/random?min=0&max={max - 1}&count=1");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return int.Parse(responseBody.Trim('[', ']', ' '));
            }
        }
        catch
        {
            Random random = new Random();
            return random.Next(0, max);
        }
    }

    static string ReverseString(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    static string FindLongestVowelSubstring(string input)
    {
        string vowels = "aeiouy";
        var vowelIndexes = input
            .Select((ch, index) => new { ch, index })
            .Where(x => vowels.Contains(x.ch))
            .Select(x => x.index)
            .ToList();

        if (vowelIndexes.Count < 2)
        {
            return string.Empty;
        }

        int start = vowelIndexes.First();
        int end = vowelIndexes.Last();
        return input.Substring(start, end - start + 1);
    }

    static char[] QuickSort(char[] array)
    {
        if (array.Length <= 1)
            return array;

        char pivot = array[array.Length / 2];
        char[] less = array.Where(x => x < pivot).ToArray();
        char[] equal = array.Where(x => x == pivot).ToArray();
        char[] greater = array.Where(x => x > pivot).ToArray();

        return QuickSort(less).Concat(equal).Concat(QuickSort(greater)).ToArray();
    }

    static char[] TreeSort(char[] array)
    {
        SortedSet<char> sortedSet = new SortedSet<char>(array);
        return sortedSet.ToArray();
    }

    static async Task Main()
    {
        Console.WriteLine("Вход");
        string lines = Console.ReadLine();
        int length = lines.Length;
        int midIndex = length / 2;
        Regex newReg = new Regex(@"[^a-z]", RegexOptions.None);
        MatchCollection matches = newReg.Matches(lines);
        string firstHalf = lines.Substring(0, midIndex);
        string secondHalf = lines.Substring(midIndex);
        string result = ReverseString(firstHalf) + ReverseString(secondHalf);
        string revers = ReverseString(lines) + lines;
        var array = lines.ToCharArray();
        var returns = array.Distinct();

        if (matches.Count > 0)
        {
            foreach (Match mat in matches)
            {
                Console.WriteLine($"Ошибка, недопустимый символ: {mat.Value}");
            }
        }
        else
        {
            string output = length % 2 == 0 ? result : revers;
            Console.WriteLine($"Выход: {output}");
            foreach (var x in returns)
            {
                Console.WriteLine($"{x} повторяется {output.Count(c => c == x)} раз(а)");
            }

            string longestVowelSubstring = FindLongestVowelSubstring(output);
            if (string.IsNullOrEmpty(longestVowelSubstring))
            {
                Console.WriteLine("Ошибка: нет подходящей подстроки.");
            }
            else
            {
                Console.WriteLine($"Самая длинная подстрока, начинающаяся и заканчивающаяся на гласную: {longestVowelSubstring}");
            }

            Console.WriteLine("Выберите алгоритм сортировки: 1 - Быстрая сортировка, 2 - Сортировка деревом");
            int choice = int.Parse(Console.ReadLine());
            char[] sortedArray = choice == 1 ? QuickSort(output.ToCharArray()) : TreeSort(output.ToCharArray());

            string sortedString = new string(sortedArray);
            Console.WriteLine("Отсортированная строка: " + sortedString);

            int randomIndex = await GetRandomNumberAsync(sortedString.Length);
            string trimmedString = sortedString.Remove(randomIndex, 1);
            Console.WriteLine("Урезанная строка: " + trimmedString);
        }
        Console.ReadKey();
    }
}
