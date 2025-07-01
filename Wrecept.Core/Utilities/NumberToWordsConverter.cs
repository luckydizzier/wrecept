using System.Collections.Generic;

namespace Wrecept.Core.Utilities;

public static class NumberToWordsConverter
{
    private static readonly string[] Ones = { "nul", "egy", "kettő", "három", "négy", "öt", "hat", "hét", "nyolc", "kilenc" };
    private static readonly string[] Tens = { "", "tíz", "húsz", "harminc", "negyven", "ötven", "hatvan", "hetven", "nyolcvan", "kilencven" };

    public static string Convert(long number)
    {
        if (number == 0) return "nulla";
        if (number < 0) return "mínusz " + Convert(-number);
        var parts = new List<string>();
        if (number / 1000000 > 0)
        {
            parts.Add(Convert(number / 1000000) + " millió");
            number %= 1000000;
        }
        if (number / 1000 > 0)
        {
            parts.Add(Convert(number / 1000) + " ezer");
            number %= 1000;
        }
        if (number / 100 > 0)
        {
            if (number / 100 == 1)
                parts.Add("száz");
            else
                parts.Add(Ones[number / 100] + "száz");
            number %= 100;
        }
        if (number >= 10)
        {
            var t = number / 10;
            var o = number % 10;
            if (o == 0)
                parts.Add(Tens[t]);
            else if (t == 1)
                parts.Add("tizen" + Ones[o]);
            else
                parts.Add(Tens[t] + "" + Ones[o]);
        }
        else if (number > 0)
        {
            parts.Add(Ones[number]);
        }
        return string.Join(" ", parts);
    }
}
