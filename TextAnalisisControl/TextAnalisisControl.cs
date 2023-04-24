using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextAnalysisAppModel;

namespace TextAnalysisAppControl
{
    public class TextAnalysisControl
    {
        private TextAnalysisModel textAnalysisModel;

        public TextAnalysisControl(Dictionary<string, int> files)
        {
            textAnalysisModel = new TextAnalysisModel(files);
        }

        public List<Dictionary<string, int>> GetWordOccurTexts()
        {
            return textAnalysisModel.GetWordOccurTexts();
        }

        public Dictionary<string, int> GetWordOccurTextsForChart()
        {
            List<Dictionary<string, int>> files = new List<Dictionary<string, int>>();
            files = textAnalysisModel.GetWordOccurTexts();
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach (Dictionary<string, int> file in files)
            {
                foreach (KeyValuePair<string, int> kvp in file)
                {
                    if (result.ContainsKey(kvp.Key)) result[kvp.Key] += kvp.Value;
                    else result.Add(kvp.Key, kvp.Value);
                }
            }
            return result;
        }

        public string GetAlphabetTexts()
        {
            List<string> listAlphabet = textAnalysisModel.GetAlphabetTexts();
            StringBuilder alphabetFilesSB = new StringBuilder();

            for (int i = 0; i < listAlphabet.Count; i++)
            {
                alphabetFilesSB.Append(String.Format("Текст(nameFile){0}\n{1}\n", i, listAlphabet.ElementAt(i)));
            }
            return alphabetFilesSB.ToString();
        }

        public string GetReversingTexts()
        {
            List<string> listReverse = textAnalysisModel.GetReversingTexts();
            StringBuilder reverseFilesSB = new StringBuilder();

            for (int i = 0; i < listReverse.Count; i++)
            {
                reverseFilesSB.Append(String.Format("Текст(nameFile){0}\n{1}\n", i, listReverse.ElementAt(i)));
            }
            return reverseFilesSB.ToString();
        }
    }
}
