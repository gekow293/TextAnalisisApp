using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace TextAnalisisApp
{
    public class InputBox
    {

        Window Box = new Window();
        FontFamily font = new FontFamily("New Roman");
        int FontSize = 14;
        StackPanel sp1 = new StackPanel();
        string title = "Введите текст";
        string boxcontent;
        string actionText = "Номер функции :";
        string defaulttext = "";
        string errormessage = "";
        string errortitle = "Errore";
        string okbuttontext = "OK";
        Brush BoxBackgroundColor = Brushes.White;
        Brush InputBackgroundColor = Brushes.Ivory;
        bool clicked = false;
        RichTextBox input = new RichTextBox();
        TextBox method = new TextBox();
        Label action = new Label();
        Button ok = new Button();
        bool inputreset = false;

        public InputBox(string content)
        {
            try
            {
                boxcontent = content;
            }
            catch { boxcontent = "Error!"; }
            windowdStack();
        }

        public InputBox(string content, string Htitle, string DefaultText)
        {
            try
            {
                boxcontent = content;
            }
            catch { boxcontent = "Error!"; }
            try
            {
                title = Htitle;
            }
            catch
            {
                title = "Error!";
            }
            try
            {
                defaulttext = "";
            }
            catch
            {
                DefaultText = "Error!";
            }
            windowdStack();
        }

        public InputBox(string content, string Htitle, string Font, int Fontsize)
        {
            try
            {
                boxcontent = content;
            }
            catch { boxcontent = "Error!"; }
            try
            {
                font = new FontFamily(Font);
            }
            catch { font = new FontFamily("New Roman"); }
            try
            {
                title = Htitle;
            }
            catch
            {
                title = "Error!";
            }
            if (Fontsize >= 1)
                FontSize = Fontsize;
            windowdStack();
        }

        private void windowdStack()
        {
            Box.Height = 500;
            Box.Width = 400;
            Box.Background = BoxBackgroundColor;
            Box.Title = title;
            Box.Content = sp1;
            Box.Closing += Box_Closing;

            //action.Background = null;
            //action.MinWidth = 150;
            
            //action.HorizontalAlignment = HorizontalAlignment.Left;
            //action.VerticalAlignment = VerticalAlignment.Top;
            //action.DataContext = actionText;
            //action.FontSize = 16;
            //sp1.Children.Add(action);

            method.Background = null;
            method.MinWidth = 30;
            method.HorizontalAlignment = HorizontalAlignment.Left;
            method.DataContext = boxcontent;
            method.FontFamily = font;
            method.FontSize = FontSize;
            sp1.Children.Add(method);

            input.Background = InputBackgroundColor;
            input.FontFamily = font;
            input.FontSize = FontSize;
            input.HorizontalAlignment = HorizontalAlignment.Left;
            input.DataContext = defaulttext;
            input.MinWidth = 300;
            input.MinHeight = 400;
            input.MouseEnter += input_MouseDown;
            sp1.Children.Add(input);

            ok.Width = 70;
            ok.Height = 30;
            ok.Click += ok_Click;
            ok.Content = okbuttontext;
            ok.HorizontalAlignment = HorizontalAlignment.Center;
            sp1.Children.Add(ok);

        }

        void Box_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!clicked)
                e.Cancel = true;
        }

        private void input_MouseDown(object sender, MouseEventArgs e)
        {
            if ((string)(sender as RichTextBox).DataContext == defaulttext && inputreset == false)
            {
                (sender as RichTextBox).DataContext = null;
                inputreset = true;
            }
        }

        void ok_Click(object sender, RoutedEventArgs e)
        {
            clicked = true;
            if ((string)input.DataContext == defaulttext || (string)input.DataContext == "")
                MessageBox.Show(errormessage, errortitle);
            else
            {
                Box.Close();
            }
            clicked = false;
        }

        public Tuple<string,string> ShowDialog()
        {
            Box.ShowDialog();
            Tuple<string, string> taskText = new Tuple<string, string>(
                new TextRange(input.Document.ContentStart,input.Document.ContentEnd).Text,
                method.Text);
            return taskText;
        }
    }
}
