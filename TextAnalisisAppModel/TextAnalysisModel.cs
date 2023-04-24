using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAnalysisAppModel
{
    public class TextAnalysisModel
    {
        private static readonly string[] Sep = new string[] { " ", ".", ",", ":", "&", "?", "#", "*", "=", "//", "-", "{", "}", ";", "+", "-", "\n" };
        private Dictionary<string, int> _dataText;
        private List<Dictionary<string, int>> _occurTexts;
        private List<string> _alphabetTexts;
        private List<string> _reverseTexts;

        public TextAnalysisModel(Dictionary<string, int> dataText)
        {
            _dataText = new Dictionary<string, int>(dataText);
            _occurTexts = GetWordOccurTexts(_dataText);
            _alphabetTexts = GetAlphabetTexts(_dataText);
            _reverseTexts = GetReversingTexts(_dataText);
        }

        public List<Dictionary<string, int>> GetWordOccurTexts()
        {
            return _occurTexts;
        }

        public List<string> GetAlphabetTexts()
        {
            return _alphabetTexts;
        }

        public List<string> GetReversingTexts()
        {
            return _reverseTexts;
        }

        public static List<Dictionary<string, int>> GetWordOccurTexts(Dictionary<string, int> files)
        {
            Dictionary<string, int> wordOccurInOneText = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            List<Dictionary<string, int>> occurTexts = new List<Dictionary<string, int>>();
            foreach (KeyValuePair<string, int> kvp in files)
            {
                if (kvp.Value == 1)
                {
                    string[] lines = kvp.Key.Split(Sep, StringSplitOptions.RemoveEmptyEntries);

                    wordOccurInOneText = lines
                        .GroupBy(word => word)
                        .ToDictionary(k => k.Key, k => k.Count())
                        .OrderByDescending(kv => kv.Value)
                        .TakeWhile(TopCount => TopCount.Value > 1)
                        .ToDictionary(kv => kv.Key, kv => kv.Value);

                    occurTexts.Add(wordOccurInOneText);
                }
            }
            return occurTexts;
        }

        public static List<string> GetAlphabetTexts(Dictionary<string, int> files)
        {
            List<string> alphabetTexts = new List<string>();
            SortedSet<string> alphabetInOneText = new SortedSet<string>();
            foreach (KeyValuePair<string, int> kvp in files)
            {
                if (kvp.Value == 2)
                {
                    var chars = new HashSet<char>(kvp.Key);
                    foreach (char ch in chars.ToArray())
                    {
                        if (!char.IsLetter(ch) && !char.IsDigit(ch)) chars.Remove(ch);
                    }
                    alphabetTexts.Add(String.Join(", ", chars));
                }
            }
            return alphabetTexts;
        }

        public static List<string> GetReversingTexts(Dictionary<string, int> files)
        {
            List<string> reverseTexts = new List<string>();

            HashSet<char> Separator = new HashSet<char> { ' ', '.', ',', ':', ';', '!', '?', '\n' };
            StringBuilder stringBuilder = new StringBuilder();
            var reverseOneText = String.Empty;

            foreach (KeyValuePair<string, int> kvp in files)
            {
                if (kvp.Value == 3)
                {
                    List<char> current = new List<char>();
                    for (int i = kvp.Key.Length - 1; i >= 0; i--)
                    {
                        char c = kvp.Key[i];

                        if (Separator.Contains(c))
                        {
                            if (current.Count > 0)
                            {
                                current.Reverse();
                                stringBuilder.Append(String.Join(String.Empty, current));
                                current.Clear();
                            }
                            stringBuilder.Append(c.ToString());
                        }
                        else current.Add(c);
                    }

                    if (current.Count > 0)
                    {
                        current.Reverse();
                        stringBuilder.Append(String.Join(String.Empty, current));
                    }

                    reverseOneText = stringBuilder.ToString();
                    reverseTexts.Add(reverseOneText);
                }
            }
            return reverseTexts;
        }
    }
}
