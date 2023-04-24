using System.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using TextAnalysisAppControl;

using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Linq;

namespace TextAnalisisApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TextAnalysisControl _textAnalysis;
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Событие при нажатии кнопки "Открыть файлы" открывается модальное диалоговое окно для ввода данных с диска
        /// </summary>
        private void openFileMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Title = "Открыть текстовые файлы";
            openFileDialog.Filter = "TXT|*.txt";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Reset();

                string[] arrFiles = openFileDialog.FileNames;
                var files = new Dictionary<string, int>();
                foreach (string file in arrFiles)
                {
                    Random rnd = new Random();
                    int method = rnd.Next(1, 4);
                    var textOfOneFile = File.ReadAllText(file, Encoding.Default);
                    files.Add(textOfOneFile, method);
                }

                _textAnalysis = new TextAnalysisControl(files);

                TextBoxesInitiation();
                ChartInitiation();
            }
        }

        /// <summary>
        /// Событие при нажатии кнопки "Ввести текст" открывается модальное диалоговое окно для ввода данных с клавиатуры
        /// </summary>
        private void writeInputTextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            var input = new InputBox("text").ShowDialog();
            Dictionary<string, int> inputText = new Dictionary<string, int>();
            int method;
            if(int.TryParse(input.Item2, out method))
            {
                inputText.Add(input.Item1, method);
            }
            
            _textAnalysis = new TextAnalysisControl(inputText);

            TextBoxesInitiation();
            ChartInitiation();
        }


        /// <summary>
        /// Очистка текстовых полей непосредственно перед загрузкой новых данных в текстовые поля
        /// </summary>
        private void Reset()
        {
            richTextBoxAlphabets.DataContext = String.Empty;
            richTextBoxReverses.DataContext = String.Empty;
            //histogram.RowDefinitions.Clear();
            //histogram.ColumnDefinitions.Clear();
        }

        /// <summary>
        /// Инициализация текстовых полей при загрузке новых данных
        /// </summary>
        private void TextBoxesInitiation()
        {
                new TextRange(richTextBoxReverses.Document.ContentStart, richTextBoxReverses.Document.ContentEnd).Text = _textAnalysis.GetReversingTexts();
                new TextRange(richTextBoxAlphabets.Document.ContentStart, richTextBoxAlphabets.Document.ContentEnd).Text = _textAnalysis.GetAlphabetTexts();
        }

        /// <summary>
        /// Построение гистограммы по обработанным данным функцией нахождения повторяющихся слов
        /// </summary>
        public void ChartInitiation()
        {
            if (_textAnalysis.GetWordOccurTextsForChart().Values.Count > 0)
            {
                for (int i = 0; i < _textAnalysis.GetWordOccurTextsForChart().Values.Max(); i++)
                {
                    RowDefinition rows = new RowDefinition();
                    histogram.RowDefinitions.Add(rows);
                }

                for (int i = 0; i < _textAnalysis.GetWordOccurTextsForChart().Count; i++)
                {
                    ColumnDefinition columns = new ColumnDefinition();
                    histogram.ColumnDefinitions.Add(columns);

                    Color color = i % 2 == 0 ? Colors.Coral : Colors.Aquamarine;

                    placeSingleColorColumn(histogram, color, _textAnalysis.GetWordOccurTextsForChart().Values.ElementAt(i), i, _textAnalysis.GetWordOccurTextsForChart().Values.Max());
                }
                string[] labels = _textAnalysis.GetWordOccurTextsForChart().Keys.ToArray();
                createLabels(labelGrid, labels);
            }
        }

        /// <summary>
        /// Построение столбцов гистограммы по количеству повторений соответствующих слов
        /// </summary>
        private void placeSingleColorColumn(Grid grid, Color color, int height, int colNum, int maxHeight)
        {
            Brush brush = new SolidColorBrush(color);
            Rectangle rect = new Rectangle();
            rect.Fill = brush;
            Grid.SetColumn(rect, colNum);
            Grid.SetRow(rect, maxHeight - height);
            Grid.SetRowSpan(rect, height);
            grid.Children.Add(rect); 
        }

        /// <summary>
        /// Создание подсказок к гистограмме
        /// </summary>
        private void createLabels(Grid labelGrid, string[] labels)
        {
            for (int i = 0; i < labels.Length; i++)
            {
                ColumnDefinition columnLabels = new ColumnDefinition();
                labelGrid.ColumnDefinitions.Add(columnLabels);

                System.Windows.Controls.Button button = new System.Windows.Controls.Button();
                button.Content = _textAnalysis.GetWordOccurTextsForChart().Values.ElementAt(i);
                
                button.Click += (sender, args) => 
                {
                    System.Windows.MessageBox.Show(_textAnalysis.GetWordOccurTextsForChart().FirstOrDefault(
                        x => x.Value == int.Parse(button.Content.ToString())).Key 
                        + ", количество повторений: " 
                        + int.Parse(button.Content.ToString()), 
                        "Слово в этом столбце", MessageBoxButton.OK);
                };

                Grid.SetColumn(button, i);    
                labelGrid.Children.Add(button);
            }
        }

        /// <summary>
        /// Событие выхода из программы
        /// </summary>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
