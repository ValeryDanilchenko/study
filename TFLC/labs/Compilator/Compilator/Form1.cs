using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace Compilator
{
    public partial class Form1 : Form
    {
        
        #region Интерфейс для формы
        string CurrentFile;//текущий файл 
        bool FileOpened = false;//Открыт ли хотя бы один файл
        //RichTextBox currentTextBox;// текущий текстбокс
        int createCount = 1;//количество созданных файлов
        List<bool> Saved = new List<bool>(); //Сохранен ли файл в памяти
        List<bool> Changed = new List<bool>(); //Сохранены ли изменения
        List<string> filesPath = new List<string>();
        List<string> openedFileNames = new List<string>();
        ToolStripMenuItem CloseTab = new ToolStripMenuItem("Закрыть текущую вкладку");//элемент контекстного меню для вкладки
        ContextMenuStrip TabMenu = new ContextMenuStrip();
        private RichTextBox currentTextBox;
        private Dictionary<RichTextBox, Stack<string>> undoStacks = new Dictionary<RichTextBox, Stack<string>>();
        private Dictionary<RichTextBox, Stack<string>> redoStacks = new Dictionary<RichTextBox, Stack<string>>();
        private const int MaxUndoRedoDepth = 500; // Установите максимальную глубину для стеков undo и redo
        

        public void Print_Text(string str)
        {
            richTextBox2.Text += str;
        }
        public Form1()
        {
            InitializeComponent();
            currentTextBox = richTextBox1;
            Saved.Add(false);
            Changed.Add(true);
            CloseTab.Click += new EventHandler(this.CloseTab_Click);
            richTextBox1.TextChanged += new System.EventHandler(this.textChanged);
            TabMenu.Items.Add(CloseTab);
            tabControl1.ContextMenuStrip = TabMenu;

            undoStacks[currentTextBox] = new Stack<string>();
            redoStacks[currentTextBox] = new Stack<string>();
            undoStacks[currentTextBox].Push(currentTextBox.Text);
            richTextBox1.TextChanged += new System.EventHandler(this.textChanged);
            tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);           
        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.Count > 0)
            {
                currentTextBox = (RichTextBox)tabControl1.SelectedTab.Controls[0];
            }
        }

        //зактрыть вкладку
        private void CloseTab_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount == 1)
            {
                DialogResult result = MessageBox.Show("Закрытие последнеей вкладки приведет к завершению работы. Продолжить?", "", MessageBoxButtons.YesNo,
                     MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                    ExitButton_Click(sender, e);
            }
            else
            {
                if (!Saved[tabControl1.SelectedIndex] || !Changed[tabControl1.SelectedIndex])
                    SaveFileButton_Click(sender, e);

                if (Saved[tabControl1.SelectedIndex])
                {
                    Saved.RemoveAt(tabControl1.SelectedIndex);
                    Changed.RemoveAt(tabControl1.SelectedIndex);
                    filesPath.RemoveAt(tabControl1.SelectedIndex);


                    openedFileNames.Remove(tabControl1.SelectedTab.Text);
                    tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                }
            }
        }

        //смена вкладки
        private void tabIndexChanged(object sender, EventArgs e)
        {
            CurrentFile = filesPath[tabControl1.SelectedIndex];
            currentTextBox = (RichTextBox)tabControl1.SelectedTab.Controls[0];
        }

        //изменение текста
        private void textChanged(object sender, EventArgs e)
        {
            Changed[tabControl1.SelectedIndex] = false;
            //RichTextBox currentTextBox = (RichTextBox)sender;

            if (Undo_Button.Enabled == false)
                Undo_Button.Enabled = undoStacks[currentTextBox].Count > 1;
            if (Redo_Button.Enabled == false)
                Redo_Button.Enabled = redoStacks[currentTextBox].Count > 0;


            if (!undoStacks[currentTextBox].Contains(currentTextBox.Text))
            {
                undoStacks[currentTextBox].Push(currentTextBox.Text);
            }
        }

        //создание файла
        private void CreateFileButton_Click(object sender, EventArgs e)
        {
            //проверка на наличие несохраненного документа
            if (richTextBox1.Text.Length > 0 && FileOpened == false)
            {
                DialogResult result = MessageBox.Show("Сохранить текущие изменения?", "", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.Yes)
                {
                    SaveAsFileButton_Click(sender, e);
                }
            }

            filesPath.Add("");

            if (FileOpened == false)
            {
                tabPage1.Text = "NewFile" + createCount;
                richTextBox1.Clear();
                FileOpened = true;
                Saved[tabControl1.SelectedIndex] = false;
                Changed[tabControl1.SelectedIndex] = false;
            }
            else
            {
                RichTextBox richTextBox2 = new RichTextBox();
                richTextBox2.Location = richTextBox1.Location;
                richTextBox2.Size = richTextBox1.Size;
                richTextBox2.TextChanged += new System.EventHandler(this.textChanged);
                tabControl1.TabPages.Add("NewFile" + createCount);
                tabControl1.TabPages[tabControl1.TabPages.Count - 1].Controls.Add(richTextBox2);
                Saved.Add(false);
                Changed.Add(false);
                undoStacks[richTextBox2] = new Stack<string>();
                redoStacks[richTextBox2] = new Stack<string>();
                undoStacks[richTextBox2].Push(richTextBox2.Text);
                richTextBox2.TextChanged += new System.EventHandler(this.textChanged);
            }

            createCount++;
        }
        //откртыть файл
        private void OpenFileButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return;
            filesPath.Add(openFileDialog1.FileName); //путь нового файла
            CurrentFile = openFileDialog1.FileName;
            string name = openFileDialog1.SafeFileName;

            // Проверка на открытый файл с таким именем
            if (openedFileNames.Contains(CurrentFile))
            {
                MessageBox.Show("Файл с таким именем уже открыт.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            openedFileNames.Add(CurrentFile);

            //если не открыт или не создан ни один файл
            if ((FileOpened == false)/* && (openedFileNames.Contains(name))*/)
            {
                tabPage1.Text = name;
                richTextBox1.Text = System.IO.File.ReadAllText(CurrentFile);
                FileOpened = true;
                Saved[tabControl1.SelectedIndex] = true;
                Changed[tabControl1.SelectedIndex] = true;
            }
            else
            {
                RichTextBox richTextBox2 = new RichTextBox();
                richTextBox2.Location = richTextBox1.Location;
                richTextBox2.Size = richTextBox1.Size;
                richTextBox2.Text = System.IO.File.ReadAllText(CurrentFile);
                richTextBox2.TextChanged += new System.EventHandler(this.textChanged);
                tabControl1.TabPages.Add(name);
                tabControl1.TabPages[tabControl1.TabPages.Count - 1].Controls.Add(richTextBox2);
                Saved.Add(true);
                Changed.Add(true);
                undoStacks[richTextBox2] = new Stack<string>();
                redoStacks[richTextBox2] = new Stack<string>();
                undoStacks[richTextBox2].Push(richTextBox2.Text);
                richTextBox2.TextChanged += new System.EventHandler(this.textChanged);
            }

            // Обнуление стеков undo и redo для текущего текстового поля
            undoStacks[currentTextBox].Clear();
            redoStacks[currentTextBox].Clear();
            undoStacks[currentTextBox].Push(currentTextBox.Text);
        }

        //сохранение файла
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            if (CurrentFile == null || !Saved[tabControl1.SelectedIndex])
                SaveAsFileButton_Click(sender, e);
            else
            {
                System.IO.File.WriteAllText(CurrentFile, currentTextBox.Text);
                Changed[tabControl1.SelectedIndex] = true;
                MessageBox.Show("Файл успешно сохранен!");
            }
        }

        //сохранить как файл
       private void SaveAsFileButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) return;
            System.IO.File.WriteAllText(saveFileDialog1.FileName, tabControl1.SelectedTab.Controls[0].Text);

            if (openedFileNames.Contains(saveFileDialog1.FileName))
            {
                MessageBox.Show("Файл с таким именем уже открыт.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (FileOpened == true)
            {
                tabControl1.SelectedTab.Text = Path.GetFileName(saveFileDialog1.FileName);
                //сохраняем путь ранее не сохраненного файла, либо изменившийся путь
                CurrentFile = saveFileDialog1.FileName;
                filesPath[tabControl1.SelectedIndex] = saveFileDialog1.FileName;
            }

            //файл и изменения сохранены
            Saved[tabControl1.SelectedIndex] = true;
            Changed[tabControl1.SelectedIndex] = true;
            //очищаем стеки "отменить" и "повторить"
            undoStacks[currentTextBox].Clear();
            undoStacks[currentTextBox].Push(currentTextBox.Text);
            redoStacks[currentTextBox].Clear();
            MessageBox.Show("Файл сохранен");
        }

        private void ExitCheck(object sender, FormClosingEventArgs e)
        {
            if (Changed.Contains(false))
            {
                DialogResult result = MessageBox.Show("Есть несохраненные файлы. Хотите сохранить их?", "", MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    for (int i = 0; i < tabControl1.TabPages.Count; i++)
                    {
                        tabControl1.SelectedIndex = i;
                        if (Saved[i] == false)
                        {
                            DialogResult SAresult = MessageBox.Show("Сохранить файл " + tabControl1.SelectedTab.Text + " ?", "", MessageBoxButtons.YesNo,
                                   MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            if (SAresult == DialogResult.Yes)
                            {
                                SaveAsFileButton_Click(sender, e);
                            }
                        }
                        else if (Changed[i] == false)
                        {

                            DialogResult SAresult = MessageBox.Show("Сохранить файл " + tabControl1.SelectedTab.Text + " ?", "", MessageBoxButtons.YesNo,
                                       MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            if (SAresult == DialogResult.Yes)
                            {
                                SaveFileButton_Click(sender, e);
                            }
                        }

                    }
                }
            }
        }

        //выход из файла
        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        //отменить
        private void UndoButton_Click(object sender, EventArgs e)
        {
            if (undoStacks[currentTextBox].Count > 1)
            {
                string lastState = undoStacks[currentTextBox].Pop();
                redoStacks[currentTextBox].Push(lastState);
                currentTextBox.TextChanged -= textChanged;
                currentTextBox.Text = undoStacks[currentTextBox].Peek();
                currentTextBox.TextChanged += textChanged;
            }
            Undo_Button.Enabled = undoStacks[currentTextBox].Count > 1;
            Redo_Button.Enabled = redoStacks[currentTextBox].Count > 0;
        }

        //повторить
        private void RedoButton_Click(object sender, EventArgs e)
        {
            if (redoStacks[currentTextBox].Count >= 1)
            {
                string nextState = redoStacks[currentTextBox].Pop();
                undoStacks[currentTextBox].Push(nextState);
                currentTextBox.TextChanged -= textChanged;
                currentTextBox.Text = nextState;
                currentTextBox.TextChanged += textChanged;
            }
            Undo_Button.Enabled = undoStacks[currentTextBox].Count > 1;
            Redo_Button.Enabled = redoStacks[currentTextBox].Count > 0;
        }

        //копировать
        private void CopyButton_Click(object sender, EventArgs e)
        {
            currentTextBox.Copy();
        }

        //вырезать
        private void CutButton_Click(object sender, EventArgs e)
        {
            currentTextBox.Cut();
        }

        //вставить
        private void PasteButton_Click(object sender, EventArgs e)
        {
            currentTextBox.Paste();
        }

        //удалить
        private void DeleteTextButton_Click(object sender, EventArgs e)
        {
            int a = currentTextBox.SelectionLength;
            currentTextBox.Text = currentTextBox.Text.Remove(currentTextBox.SelectionStart, a);
        }

        //выделить всё
        private void SelectAllTextButton_Click(object sender, EventArgs e)
        {
            currentTextBox.SelectAll();
        }

        //вызов справки
        private void CallHelpButton_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "manual1.chm");
        }

        private void AboutProgrammButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"Course Work.docx");
        }

        private void постановакаЗадачиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "manual.chm", HelpNavigator.Topic, "manual.chm::/postanovka_zadachi.htm");
        }

        private void грамматикаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "manual.chm", HelpNavigator.Topic, "manual.chm::/grammar.htm");
        }

        private void классификацияГрамматикиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "manual.chm", HelpNavigator.Topic, "manual.chm::/klassifikatsiya_grammatiki.htm");
        }

        private void методАнализаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "manual.chm", HelpNavigator.Topic, "manual.chm::/analise.htm");
        }

        private void диагностикаИНейтрализацияОшибокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "manual.chm", HelpNavigator.Topic, "manual.chm::/diagnostika_i_nejtiralizatsiya_oshibok.htm");
        }

        private void тестовыйПримерToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "manual.chm", HelpNavigator.Topic, "manual.chm::/testirovanie.htm");
        }

        private void списокЛитературыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "manual.chm", HelpNavigator.Topic, "manual.chm::/spisok_literatury.htm");

        }

        private void исходныйКодПрограммыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "manual.chm", HelpNavigator.Topic, "manual.chm::/iskhodnyj_kod.htm");

        }

        #endregion

        //запуск проверки с помощью регулярного выражения
        private void RunRegExpButton_Click(object sender, EventArgs e)
        {
            string input = currentTextBox.Text;

            //string pattern = @"([A-Za-z0-9](?:(?:[-_](?=[A-Za-z0-9]))|(?:\.(?=[A-Za-z0-9]))|[A-Za-z0-9])*)@([A-Za-z]+\.[A-Za-z]{2,5})";
            string pattern = @"(?<=^|[\s])([A-Za-z0-9](?:(?:[-_](?=[A-Za-z0-9]))|(?:\.(?=[A-Za-z0-9]))|[A-Za-z0-9])*)@([A-Za-z]+\.[A-Za-z]{2,5})";
            string[] domainPattern = { "ru", "com", "org", "web", "su" };

            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(input);
            
            List<string> filteredEmails = new List<string>();
            foreach (Match match in matches)
            {
                string email = match.Value;
                string mainDomain = email.Split('@')[1].Split('.').Last();

                if (domainPattern.Contains(mainDomain))
                {
                    filteredEmails.Add(email);
                }
            }
            
            if (filteredEmails.Count > 0)
            {
                richTextBox2.Text = "Найдено " + filteredEmails.Count + " email адрес(ов) с доменами " + string.Join(", ", domainPattern) + ":\n";
                foreach (string email in filteredEmails)
                {
                    int length = email.Length;
                    int domainCount = email.Split('@')[1].Split('.').Length - 1;
                    int firstCharIndex = input.IndexOf(email);
                    int lastCharIndex = firstCharIndex + length - 1;
                    int lineNumber = input.Substring(0, firstCharIndex).Split('\n').Length;
                    int lineStartIndex = input.LastIndexOf('\n', firstCharIndex) + 1;
                    int firstCharColumn = firstCharIndex - lineStartIndex + 1;
                    int lastCharColumn = lastCharIndex - lineStartIndex + 1;

                    richTextBox2.Text += "- " + email + "\n";
                    richTextBox2.Text += "  Первый символ: " + lineNumber + ", " + firstCharColumn + "\n";
                    richTextBox2.Text += "  Последний символ: " + lineNumber + ", " + lastCharColumn + "\n";
                    richTextBox2.Text += "  Длина: " + length + "\n";
                    richTextBox2.Text += "  Количество поддоменов: " + domainCount + "\n";
                }
            }
            else
            {
                richTextBox2.Text = "Email адреса с доменами " + string.Join(", ", domainPattern) + " не найдены";
            }
        }

        //запуск проверки с помощью конечного автомата
        public void RunStateMachineButton_Click(object sender, EventArgs e)
        {
            richTextBox2.Clear();
            string input = currentTextBox.Text;
            string[] domainPattern = { "ru", "com", "org", "web", "su" };
            List<string> filteredEmails = new List<string>();

            int state = 0;
            int startIndex = 0;
            int domainStartIndex = 0;
            string currentDomain = "";
            char c;
            char prevChar = '\0';

            for (int i = 0; i < input.Length + 1; i++)
            {
                if (i == input.Length)
                {
                    c = '\0';
                }
                else
                    c = input[i];

                switch (state)
                {
                    case 0:
                        if (char.IsLetterOrDigit(c))
                        {
                            startIndex = i;
                            state = 1;
                            richTextBox2.Text += $"Состояние 0 -> Состояние 1: Текущий символ: {c}\n";
                        }
                        else
                        {
                            richTextBox2.Text += $"Состояние 0 -> Состояние 0: Текущий символ: {c}\n";
                        }
                        break;
                    case 1:
                        if ((c == '@') && (char.IsLetterOrDigit(prevChar)))
                        {
                            state = 2;
                            richTextBox2.Text += $"Состояние 1 -> Состояние 2: Прошлый символ: {prevChar} Текущий символ: {c}\n";
                        }
                        else if (( c == '.' || c == '_' || c == '-') && (prevChar == '.' || prevChar == '_' || prevChar == '-'))
                        {
                            state = 0;
                            richTextBox2.Text += $"Состояние 1 -> Состояние 0: Прошлый символ: {prevChar} Текущий символ: {c}\n";
                            richTextBox2.Text += "\nFALSE\n";
                        }
                        else if (!(char.IsLetterOrDigit(c) || c == '.' || c == '_' || c == '-'))
                        {
                            state = 0;
                            richTextBox2.Text += $"Состояние 1 -> Состояние 0: Прошлый символ: {prevChar} Текущий символ: {c}\n";
                            richTextBox2.Text += "\nFALSE\n";
                        }
                        else
                        {
                            richTextBox2.Text += $"Состояние 1 -> Состояние 1: Текущий символ: {c}\n";
                        }
                        break;
                    case 2:
                        if (char.IsLetterOrDigit(c))
                        {
                            state = 3;
                            richTextBox2.Text += $"Состояние 2 -> Состояние 3: Прошлый символ: {prevChar} Текущий символ: {c}\n";
                        }
                        else
                        {
                            state = 0;
                            richTextBox2.Text += $"Состояние 2 -> Состояние 0: Прошлый символ: {prevChar} Текущий символ: {c}\n";
                            richTextBox2.Text += "\nFALSE\n";
                        }
                        break;
                    case 3:
                        if (c == '.')
                        {
                            domainStartIndex = i + 1;
                            state = 4;
                            richTextBox2.Text += $"Состояние 3 -> Состояние 4: Прошлый символ: {prevChar} Текущий символ: {c}\n";
                        }
                        else if (!char.IsLetterOrDigit(c))
                        {
                            state = 0;
                            richTextBox2.Text += $"Состояние 3 -> Состояние 0: Прошлый символ: {prevChar} Текущий символ: {c}\n";
                            richTextBox2.Text += "\nFALSE\n";
                        }
                        else
                        {
                            richTextBox2.Text += $"Состояние 3 -> Состояние 3: Текущий символ: {c}\n";
                        }
                        break;
                    case 4:
                        if ( c == '\0' || c == '\n' || char.IsWhiteSpace(c))
                        {
                            currentDomain = input.Substring(domainStartIndex, i - domainStartIndex);
                            if (domainPattern.Contains(currentDomain))
                            {
                                string email = input.Substring(startIndex, i - startIndex + (i == input.Length - 1 ? 1 : 0));
                                filteredEmails.Add(email);
                                richTextBox2.Text += $"Состояние 4 Прошлый символ: {prevChar}\n";
                                state = 0;
                                richTextBox2.Text += $"Состояние 4 -> Состояние 0: Прошлый символ: {prevChar} Текущий символ: {c}\n";
                                richTextBox2.Text += "\nTRUE\n";
                            }
                            else
                            {
                                state = 0;
                                richTextBox2.Text += $"Состояние 4 -> Состояние 0: Прошлый символ: {prevChar} Текущий символ: {c}\n";
                                richTextBox2.Text += "\nFALSE\n";
                            }
                            
                        }
                        //else if((c == '.') && (prevChar != '.'))
                        //{
                        //    i--;
                        //    state = 3;
                        //}
                        else if (!char.IsLetter(c))
                        {
                            state = 0;
                            richTextBox2.Text += $"Состояние 4 -> Состояние 0: Прошлый символ: {prevChar} Текущий символ: {c}\n";
                            richTextBox2.Text += "\nFALSE\n";
                        }
                        else
                        {
                            richTextBox2.Text += $"Состояние 4 -> Состояние 4: Текущий символ: {c}\n";
                        }
                        break;

                        default:
                        break;
                }
                if (i > 0)
                    prevChar = c;
            }

            
        }




        private void RunParserTokens_Click()/*(object sender, EventArgs e)*/
        {
            string input = currentTextBox.Text;
            var lexer = new Lexer(input);
            var tokens = lexer.Tokenize();
            int state = 0;
            Token prevToken = new Token();
            prevToken.Position = -2;
            prevToken.Value = "0";
            bool successfully = true;
            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[0].Type == TokenType.EOF)
                {
                    richTextBox2.Text += "Пустая строка!\n";
                    tokens[i].ExpectedValue = "Console.Writeline(\"Hello world!\")";
                    break;
                }
                switch (state)
                {
                    case 0:
                        {
                            if (tokens[i].ExpectedValue == null)
                                tokens[i].ExpectedValue = "Console";
                            switch (tokens[i].Type)
                            {
                                case TokenType.Class:
                                    {
                                        state = 1;
                                        break;
                                    }
                                case TokenType.Expression:
                                case TokenType.Method:
                                case TokenType.Options:
                                case TokenType.String:
                                case TokenType.Number:
                                case TokenType.NumberZero:
                                case TokenType.EndOptions:
                                case TokenType.EndOfMethod:
                                case TokenType.ErrorMethod:
                                case TokenType.ErrorString:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалось имя класса: \"Console\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 1;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.ErrorClass:
                                    {
                                        state = 1;
                                        richTextBox2.Text += "Ошибка! Неизвестный идентификатор. Позиция: " + tokens[i].Position + "  Ожидалось имя класса: \"Console\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && (tokens[i + 1].Type == TokenType.ErrorClass || tokens[i + 1].Type == TokenType.Undefined))
                                            i++;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Undefined:
                                    {
                                        state = 0;
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Неизвестный символ: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && tokens[i + 1].Type == TokenType.Undefined)
                                            i++;
                                        successfully = false;
                                        break;
                                    }
                                default: break;
                            }
                            break;
                        }
                    case 1:
                        {
                            //if (tokens[i].ExpectedValue == null)
                                tokens[i].ExpectedValue = ".";
                            switch (tokens[i].Type)
                            {
                                case TokenType.Expression:
                                    {
                                        state = 2;
                                        break;
                                    }
                                case TokenType.Class:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалось выражение: \".\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 0;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Method:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалось выражение: \".\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 2;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Options:
                                case TokenType.String:
                                case TokenType.Number:
                                case TokenType.NumberZero:
                                case TokenType.EndOptions:
                                case TokenType.EndOfMethod:
                                case TokenType.ErrorString:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалось выражение: \".\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 2;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.ErrorMethod:
                                case TokenType.ErrorClass:
                                    {
                                        state = 2;
                                        richTextBox2.Text += "Ошибка! Неизвестный идентификатор. Позиция: " + tokens[i].Position + "  Ожидалось выражение: \".\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && (tokens[i + 1].Type == TokenType.ErrorMethod || tokens[i + 1].Type == TokenType.ErrorClass))
                                            i++;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Undefined:
                                    {
                                        state = 1;
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Неизвестный символ: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && tokens[i + 1].Type == TokenType.Undefined)
                                        {
                                            i++;
                                        }
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.EOF:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалось выражение: \".\"." + " Получено: \"\\0\"\n";
                                        state = 2;
                                        i--;
                                        break;
                                    }
                                default: break;
                            }
                            break;
                        }
                    case 2:
                        {
                            //if (tokens[i].ExpectedValue == null)
                                tokens[i].ExpectedValue = "WriteLine";
                            switch (tokens[i].Type)
                            {
                                case TokenType.Method:
                                    {
                                        state = 3;
                                        break;
                                    }
                                case TokenType.Expression:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалось имя Метода: \"WriteLine\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 1;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Class:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалось имя Метода: \"WriteLine\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 0;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Options:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалось имя Метода: \"WriteLine\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 3;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.String:
                                case TokenType.Number:
                                case TokenType.NumberZero:
                                case TokenType.EndOptions:
                                case TokenType.EndOfMethod:
                                case TokenType.ErrorClass:
                                case TokenType.ErrorString:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалось имя Метода: \"WriteLine\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 3;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.ErrorMethod:
                                    {
                                        state = 3;
                                        richTextBox2.Text += "Ошибка! Неизвестный идентификатор. Позиция: " + tokens[i].Position + "  Ожидалось имя Метода: \"WriteLine\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && tokens[i + 1].Type == TokenType.ErrorMethod)
                                            i++;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Undefined:
                                    {
                                        state = 2;
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Неизвестный символ: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && tokens[i + 1].Type == TokenType.Undefined)
                                        {
                                            i++;
                                        }
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.EOF:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалось имя Метода: \"WriteLine\"." + " Получено: \"\\0\"\n";
                                        state = 3;
                                        i--;
                                        break;
                                    }
                                default: break;
                            }
                            break;
                        }
                    case 3:
                        {
                            //if (tokens[i].ExpectedValue == null)
                                tokens[i].ExpectedValue = "(";
                            switch (tokens[i].Type)
                            {
                                case TokenType.Options:
                                    {
                                        state = 4;
                                        break;
                                    }
                                case TokenType.Expression:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась открывающая скобка: \"(\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 1;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Method:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась открывающая скобка: \"(\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 2;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Class:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась открывающая скобка: \"(\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 0;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.String:
                                case TokenType.Number:
                                case TokenType.NumberZero:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась открывающая скобка: \"(\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 4;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.EndOptions:
                                case TokenType.EndOfMethod:
                                case TokenType.ErrorString:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась открывающая скобка: \"(\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 4;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.ErrorMethod:
                                case TokenType.ErrorClass:
                                    {
                                        state = 4;
                                        richTextBox2.Text += "Ошибка! Неизвестный идентификатор. Позиция: " + tokens[i].Position + "  Ожидалась открывающая скобка: \"(\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && (tokens[i + 1].Type == TokenType.ErrorMethod || tokens[i + 1].Type == TokenType.ErrorClass))
                                            i++;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Undefined:
                                    {
                                        state = 3;
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Неизвестный символ: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && tokens[i + 1].Type == TokenType.Undefined)
                                        {
                                            i++;
                                        }
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.EOF:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась открывающая скобка: \"(\"." + " Получено: \"\\0\"\n";
                                        state = 4;
                                        i--;
                                        break;
                                    }
                                default: break;
                            }
                            break;
                        }
                    case 4:
                        {
                            //if (tokens[i].ExpectedValue == null)
                                tokens[i].ExpectedValue = "\"" + tokens[i].Value +  "\"";
                            switch (tokens[i].Type)
                            {
                                case TokenType.String:
                                case TokenType.Number:
                                case TokenType.NumberZero:
                                    {
                                        tokens[i].ExpectedValue = tokens[i].Value;
                                        state = 5;
                                        break;
                                    }
                                case TokenType.Expression:
                                    {
                                        tokens[i].ExpectedValue = "";
                                        //if (tokens[i + 1].Type != TokenType.EndOptions)
                                        //{
                                        //    tokens[i].ExpectedValue += tokens[i + 1].Value;
                                        //    tokens[i + 1].ExpectedValue = "";
                                        //}
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидался параметр." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 1;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Method:
                                    {
                                        tokens[i].ExpectedValue = "";
                                        //if (tokens[i + 1].Type != TokenType.EndOptions)
                                        //{
                                        //    tokens[i].ExpectedValue += tokens[i + 1].Value;
                                        //    tokens[i + 1].ExpectedValue = "";
                                        //}
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидался параметр." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 2;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Class:
                                    {
                                        tokens[i].ExpectedValue = "";
                                        //if (tokens[i + 1].Type != TokenType.EndOptions)
                                        //{
                                        //    tokens[i].ExpectedValue += tokens[i + 1].Value;
                                        //    tokens[i + 1].ExpectedValue = "";
                                        //}
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидался параметр." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 0;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Options:
                                    {
                                        tokens[i].ExpectedValue = "";
                                        //if (tokens[i + 1].Type != TokenType.EndOptions)
                                        //{
                                        //    tokens[i].ExpectedValue += tokens[i + 1].Value;
                                        //    tokens[i + 1].ExpectedValue = "";
                                        //}
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидался параметр." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 3;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.EndOptions:
                                    {
                                        tokens[i].ExpectedValue = tokens[i].Value;
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидался параметр." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 5;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.EndOfMethod:
                                case TokenType.ErrorMethod:
                                case TokenType.ErrorClass:
                                    {
                                        tokens[i].ExpectedValue = "";
                                        //if (tokens[i + 1].Type != TokenType.EndOptions)
                                        //{
                                        //    tokens[i].ExpectedValue += tokens[i + 1].Value;
                                        //    tokens[i + 1].ExpectedValue = "";
                                        //}
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидался параметр." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 5;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.ErrorString:
                                    {
                                        tokens[i].ExpectedValue = tokens[i].Value + "\"";
                                        state = 5;
                                        richTextBox2.Text += "Ошибка! Неизвестный идентификатор. Позиция: " + tokens[i].Position + " Ожидался параметр." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && tokens[i + 1].Type == TokenType.ErrorString)
                                            i++;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Undefined:
                                    {
                                        state = 4;
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Неизвестный символ: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && tokens[i + 1].Type == TokenType.Undefined)
                                        {
                                            i++;
                                        }
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.EOF:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидался параметр." + " Получено: \"\\0\"\n";
                                        state = 5;
                                        i--;
                                        break;
                                    }
                                default: break;
                            }
                            break;
                        }
                    case 5:
                        {
                            //if (tokens[i].ExpectedValue == null)
                                tokens[i].ExpectedValue = ")";
                            switch (tokens[i].Type)
                            {
                                case TokenType.EndOptions:
                                    {
                                        state = 6;
                                        break;
                                    }
                                case TokenType.Expression:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась закрывающая скобка: \")\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 1;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Method:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась закрывающая скобка: \")\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 2;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Class:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась закрывающая скобка: \")\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 0;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Options:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась закрывающая скобка: \")\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 4;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.String:
                                case TokenType.Number:
                                case TokenType.NumberZero:
                                    {
                                        if ((tokens[i].Type == TokenType.Number || tokens[i].Type == TokenType.NumberZero) && tokens[i - 1].Type == TokenType.NumberZero)
                                        {
                                            tokens[i].ExpectedValue = "";
                                            state = 4;
                                            richTextBox2.Text += "Ошибка! Позиция: " + tokens[i - 1].Position + " Число не может начинаться с нуля!\n";
                                            successfully = false;
                                            break;
                                        }
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась закрывающая скобка: \")\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 4;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.EndOfMethod:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась закрывающая скобка: \")\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 6;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.ErrorString:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась закрывающая скобка: \")\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 6;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.ErrorMethod:
                                case TokenType.ErrorClass:
                                    {
                                        state = 6;
                                        richTextBox2.Text += "Ошибка! Неизвестный идентификатор. Позиция: " + tokens[i].Position + "  Ожидалась закрывающая скобка: \")\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && (tokens[i + 1].Type == TokenType.ErrorMethod || tokens[i + 1].Type == TokenType.ErrorClass))
                                            i++;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Undefined:
                                    {
                                        state = 5;
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Неизвестный символ: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && tokens[i + 1].Type == TokenType.Undefined)
                                        {
                                            i++;
                                        }
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.EOF:
                                    {
                                        state = 6;
                                        i--;
                                        if (prevToken.Type == TokenType.ErrorString)
                                        {
                                            richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидался конец строки: \" ." + " Получено: \"\\0\"\n";
                                            richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась закрывающая скобка: \")\"." + " Получено: \"\\0\"\n";
                                        }
                                        else
                                        {
                                            richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась закрывающая скобка: \")\"." + " Получено: \"\\0\"\n";
                                        }
                                        break;
                                    }
                                default: break;
                            }
                            break;
                        }
                    case 6:
                        {
                            if(tokens[i].ExpectedValue == null)
                                tokens[i].ExpectedValue = ";";
                            switch (tokens[i].Type)
                            {
                                case TokenType.EndOfMethod:
                                    {
                                        state = 7;
                                        break;
                                    }
                                case TokenType.Expression:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась точка с запятой: \";\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 0;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Method:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась точка с запятой: \";\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 0;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Class:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась точка с запятой: \";\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 0;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.String:
                                case TokenType.Number:
                                case TokenType.NumberZero:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась точка с запятой: \";\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 0;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.EndOptions:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась точка с запятой: \";\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 0;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Options:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась точка с запятой: \";\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 0;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.ErrorClass:
                                case TokenType.ErrorMethod:
                                case TokenType.ErrorString:
                                    {
                                        state = 0;
                                        richTextBox2.Text += "Ошибка! Неизвестный идентификатор. Позиция: " + tokens[i].Position + " Ожидалась точка с запятой: \";\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && (tokens[i + 1].Type == TokenType.ErrorClass || tokens[i + 1].Type == TokenType.ErrorMethod || tokens[i + 1].Type == TokenType.ErrorString))
                                            i++;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Undefined:
                                    {
                                        state = 6;
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Неизвестный символ: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && tokens[i + 1].Type == TokenType.Undefined)
                                        {
                                            i++;
                                        }
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.EOF:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась точка с запятой: \";\"." + " Получено: \"\\0\"\n";
                                        break;
                                    }
                                default: break;
                            }
                            break;
                        }
                    case 7:
                        {
                            if (tokens[i].ExpectedValue == null)
                                tokens[i].ExpectedValue = "\0";
                            if (tokens[i].Type == TokenType.EOF && successfully)
                            {
                                richTextBox2.Text += "Ошибок нет! Конец разбора\n";
                            }
                            else
                            {
                                i--;
                                state = 0;
                            }
                            break;
                        }
                    default: break;
                }
                if (i > 0)
                    prevToken = tokens[i];
            }

            if (!successfully)
            {
                foreach (Token token in tokens)
                    richTextBox2.Text += token.ExpectedValue;
            }
            else
                richTextBox2.Text += input;
        }



        ////запуск лексического анализатора 
        private void RunLexerButton_Click(object sender, EventArgs e)
        {
            richTextBox2.Clear();
            string input = currentTextBox.Text;
            var lexer = new Lexer(input);
            var tokens = lexer.Tokenize();

            richTextBox2.Text += "Работа текстового анализатора для функции Console.WriteLine(); из языка C#:\n";

            foreach (Token token in tokens)
            {
                richTextBox2.Text += "Тип лексемы: \'" + token.Type + "\' \tЗначение: \'" + token.Value + "\'\tПозиция: \'" + token.Position + "\'\n";
            }

            RunParserTokens_Click();
        }

        //запуск проверки тексат методом рекурсивного спуска

        #region Парсер для лексического анализатора
        private void RunParserButton_Click(object sender, EventArgs e)
        {
            richTextBox2.Clear();
            var lexer = new Lexer(currentTextBox.Text);
            var tokens = lexer.Tokenize();
            int state = 0;
            Token prevToken = new Token();
            prevToken.Position = -2;
            prevToken.Value = "0";
            bool successfully = true;
            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[0].Type == TokenType.EOF)
                {
                    richTextBox2.Text += "Пустая строка!\n";
                    break;
                }
                switch (state)
                {
                    case 0:
                        {
                            switch (tokens[i].Type)
                            {
                                case TokenType.Class:
                                    {
                                        state = 1;
                                        break;
                                    }
                                case TokenType.Expression:
                                case TokenType.Method:
                                case TokenType.Options:
                                case TokenType.String:
                                case TokenType.Number:
                                case TokenType.NumberZero:
                                case TokenType.EndOptions:
                                case TokenType.EndOfMethod:
                                case TokenType.ErrorMethod:
                                case TokenType.ErrorString:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалось имя класса: \"Console\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 1;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.ErrorClass:
                                    {
                                        state = 1;
                                        richTextBox2.Text += "Ошибка! Неизвестный идентификатор. Позиция: " + tokens[i].Position + "  Ожидалось имя класса: \"Console\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && (tokens[i + 1].Type == TokenType.ErrorClass || tokens[i + 1].Type == TokenType.Undefined))
                                            i++;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Undefined:
                                    {
                                        state = 0;
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Неизвестный символ: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && tokens[i + 1].Type == TokenType.Undefined)
                                            i++;
                                        successfully = false;
                                        break;
                                    }
                                default: break;
                            }
                            break;
                        }
                    case 1:
                        {
                            switch (tokens[i].Type)
                            {
                                case TokenType.Expression:
                                    {
                                        state = 2;
                                        break;
                                    }
                                case TokenType.Class:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалось выражение: \".\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 0;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Method:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалось выражение: \".\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 2;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Options:
                                case TokenType.String:
                                case TokenType.Number:
                                case TokenType.NumberZero:
                                case TokenType.EndOptions:
                                case TokenType.EndOfMethod:
                                case TokenType.ErrorString:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалось выражение: \".\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 2;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.ErrorMethod:
                                case TokenType.ErrorClass:
                                    {
                                        state = 2;
                                        richTextBox2.Text += "Ошибка! Неизвестный идентификатор. Позиция: " + tokens[i].Position + "  Ожидалось выражение: \".\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && (tokens[i + 1].Type == TokenType.ErrorMethod || tokens[i + 1].Type == TokenType.ErrorClass))
                                            i++;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Undefined:
                                    {
                                        state = 1;
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Неизвестный символ: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && tokens[i + 1].Type == TokenType.Undefined)
                                        {
                                            i++;
                                        }
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.EOF:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалось выражение: \".\"." + " Получено: \"\\0\"\n";
                                        state = 2;
                                        i--;
                                        break;
                                    }
                                default: break;
                            }
                            break;
                        }
                    case 2:
                        {
                            switch (tokens[i].Type)
                            {
                                case TokenType.Method:
                                    {
                                        state = 3;
                                        break;
                                    }
                                case TokenType.Expression:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалось имя Метода: \"WriteLine\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 1;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Class:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалось имя Метода: \"WriteLine\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 0;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Options:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалось имя Метода: \"WriteLine\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 3;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.String:
                                case TokenType.Number:
                                case TokenType.NumberZero:
                                case TokenType.EndOptions:
                                case TokenType.EndOfMethod:
                                case TokenType.ErrorClass:
                                case TokenType.ErrorString:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалось имя Метода: \"WriteLine\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 3;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.ErrorMethod:
                                    {
                                        state = 3;
                                        richTextBox2.Text += "Ошибка! Неизвестный идентификатор. Позиция: " + tokens[i].Position + "  Ожидалось имя Метода: \"WriteLine\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && tokens[i + 1].Type == TokenType.ErrorMethod)
                                            i++;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Undefined:
                                    {
                                        state = 2;
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Неизвестный символ: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && tokens[i + 1].Type == TokenType.Undefined)
                                        {
                                            i++;
                                        }
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.EOF:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалось имя Метода: \"WriteLine\"." + " Получено: \"\\0\"\n";
                                        state = 3;
                                        i--;
                                        break;
                                    }
                                default: break;
                            }
                            break;
                        }
                    case 3:
                        {
                            switch (tokens[i].Type)
                            {
                                case TokenType.Options:
                                    {
                                        state = 4;
                                        break;
                                    }
                                case TokenType.Expression:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась открывающая скобка: \"(\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 1;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Method:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась открывающая скобка: \"(\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 2;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Class:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась открывающая скобка: \"(\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 0;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.String:
                                case TokenType.Number:
                                case TokenType.NumberZero:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась открывающая скобка: \"(\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 4;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.EndOptions:
                                case TokenType.EndOfMethod:
                                case TokenType.ErrorString:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась открывающая скобка: \"(\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 4;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.ErrorMethod:
                                case TokenType.ErrorClass:
                                    {
                                        state = 4;
                                        richTextBox2.Text += "Ошибка! Неизвестный идентификатор. Позиция: " + tokens[i].Position + "  Ожидалась открывающая скобка: \"(\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && (tokens[i + 1].Type == TokenType.ErrorMethod || tokens[i + 1].Type == TokenType.ErrorClass))
                                            i++;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Undefined:
                                    {
                                        state = 3;
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Неизвестный символ: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && tokens[i + 1].Type == TokenType.Undefined)
                                        {
                                            i++;
                                        }
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.EOF:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась открывающая скобка: \"(\"." + " Получено: \"\\0\"\n";
                                        state = 4;
                                        i--;
                                        break;
                                    }
                                default: break;
                            }
                            break;
                        }
                    case 4:
                        {
                            switch (tokens[i].Type)
                            {
                                case TokenType.String:
                                case TokenType.Number:
                                case TokenType.NumberZero:
                                    {
                                        state = 5;
                                        break;
                                    }
                                case TokenType.Expression:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидался параметр." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 1;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Method:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидался параметр." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 2;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Class:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидался параметр." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 0;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Options:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидался параметр." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 3;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.EndOptions:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидался параметр." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 5;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.EndOfMethod:
                                case TokenType.ErrorMethod:
                                case TokenType.ErrorClass:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидался параметр." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 5;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.ErrorString:
                                    {
                                        state = 5;
                                        richTextBox2.Text += "Ошибка! Неизвестный идентификатор. Позиция: " + tokens[i].Position + " Ожидался параметр." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && tokens[i + 1].Type == TokenType.ErrorString)
                                            i++;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Undefined:
                                    {
                                        state = 4;
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Неизвестный символ: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && tokens[i + 1].Type == TokenType.Undefined)
                                        {
                                            i++;
                                        }
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.EOF:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидался параметр." + " Получено: \"\\0\"\n";
                                        state = 5;
                                        i--;
                                        break;
                                    }
                                default: break;
                            }
                            break;
                        }
                    case 5:
                        {
                            switch (tokens[i].Type)
                            {
                                case TokenType.EndOptions:
                                    {
                                        state = 6;
                                        break;
                                    }
                                case TokenType.Expression:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась закрывающая скобка: \")\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 1;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Method:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась закрывающая скобка: \")\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 2;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Class:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась закрывающая скобка: \")\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 0;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Options:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась закрывающая скобка: \")\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 4;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.String:
                                case TokenType.Number:
                                case TokenType.NumberZero:
                                    {
                                        if ((tokens[i].Type == TokenType.Number || tokens[i].Type == TokenType.NumberZero) && tokens[i - 1].Type == TokenType.NumberZero)
                                        {
                                            state = 4;
                                            richTextBox2.Text += "Ошибка! Позиция: " + tokens[i - 1].Position + " Число не может начинаться с нуля!\n";
                                            successfully = false;
                                            break;
                                        }
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась открывающая скобка: \"(\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 4;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.EndOfMethod:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась закрывающая скобка: \")\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 6;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.ErrorString:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась закрывающая скобка: \")\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 6;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.ErrorMethod:
                                case TokenType.ErrorClass:
                                    {
                                        state = 6;
                                        richTextBox2.Text += "Ошибка! Неизвестный идентификатор. Позиция: " + tokens[i].Position + "  Ожидалась закрывающая скобка: \")\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && (tokens[i + 1].Type == TokenType.ErrorMethod || tokens[i + 1].Type == TokenType.ErrorClass))
                                            i++;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Undefined:
                                    {
                                        state = 5;
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Неизвестный символ: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && tokens[i + 1].Type == TokenType.Undefined)
                                        {
                                            i++;
                                        }
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.EOF:
                                    {
                                        state = 6;
                                        i--;
                                        if (prevToken.Type == TokenType.ErrorString)
                                        {
                                            richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидался конец строки: \" ." + " Получено: \"\\0\"\n";
                                            richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась закрывающая скобка: \")\"." + " Получено: \"\\0\"\n";
                                        }
                                        else
                                        {
                                            richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась закрывающая скобка: \")\"." + " Получено: \"\\0\"\n";
                                        }
                                        break;
                                    }
                                default: break;
                            }
                            break;
                        }
                    case 6:
                        {
                            switch (tokens[i].Type)
                            {
                                case TokenType.EndOfMethod:
                                    {
                                        state = 7;
                                        break;
                                    }
                                case TokenType.Expression:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась точка с запятой: \";\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 0;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Method:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась точка с запятой: \";\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 0;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Class:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась точка с запятой: \";\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 0;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.String:
                                case TokenType.Number:
                                case TokenType.NumberZero:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась точка с запятой: \";\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 0;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.EndOptions:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась точка с запятой: \";\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 0;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Options:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась точка с запятой: \";\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        state = 0;
                                        i--;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.ErrorClass:
                                case TokenType.ErrorMethod:
                                case TokenType.ErrorString:
                                    {
                                        state = 0;
                                        richTextBox2.Text += "Ошибка! Неизвестный идентификатор. Позиция: " + tokens[i].Position + " Ожидалась точка с запятой: \";\"." + " Получено: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && (tokens[i + 1].Type == TokenType.ErrorClass || tokens[i + 1].Type == TokenType.ErrorMethod || tokens[i + 1].Type == TokenType.ErrorString))
                                            i++;
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.Undefined:
                                    {
                                        state = 6;
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Неизвестный символ: \"" + tokens[i].Value + "\"\n";
                                        while (tokens[i + 1].Type != TokenType.EOF && tokens[i + 1].Type == TokenType.Undefined)
                                        {
                                            i++;
                                        }
                                        successfully = false;
                                        break;
                                    }
                                case TokenType.EOF:
                                    {
                                        richTextBox2.Text += "Ошибка! Позиция: " + tokens[i].Position + " Ожидалась точка с запятой: \";\"." + " Получено: \"\\0\"\n";
                                        break;
                                    }
                                default: break;
                            }
                            break;
                        }
                    case 7:
                        {
                            if (tokens[i].Type == TokenType.EOF && successfully)
                            {
                                richTextBox2.Text += "Ошибок нет! Конец разбора";
                            }
                            else
                            {
                                i--;
                                state = 0;
                            }
                            break;
                        }
                    default: break;
                }
                if (i > 0)
                    prevToken = tokens[i];
            }
        }
        #endregion
    }
}
