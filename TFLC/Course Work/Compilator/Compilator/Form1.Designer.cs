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
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.RunButton = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CallHelpButton = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutProgrammButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.NewFile_Button = new System.Windows.Forms.ToolStripButton();
            this.OpenFile_Button = new System.Windows.Forms.ToolStripButton();
            this.SaveFile_Button = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Undo_Button = new System.Windows.Forms.ToolStripButton();
            this.Redo_Button = new System.Windows.Forms.ToolStripButton();
            this.Copy_Button = new System.Windows.Forms.ToolStripButton();
            this.Cut_Button = new System.Windows.Forms.ToolStripButton();
            this.Paste_Button = new System.Windows.Forms.ToolStripButton();
            this.Play_Button = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.исходныйКодПрограммыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.списокЛитературыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CreateFileButton = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenFileButton = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveFileButton = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsFileButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitProgrammButton = new System.Windows.Forms.ToolStripMenuItem();
            this.правкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UndoTextButton = new System.Windows.Forms.ToolStripMenuItem();
            this.RedoTextButton = new System.Windows.Forms.ToolStripMenuItem();
            this.CutTextButton = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyTextButton = new System.Windows.Forms.ToolStripMenuItem();
            this.PasteTextButton = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteTextButton = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectAllTextButton = new System.Windows.Forms.ToolStripMenuItem();
            this.текстToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.постановакаЗадачиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.грамматикаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.классификацияГрамматикиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.методАнализаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.диагностикаИНейтрализацияОшибокToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.тестовыйПримерToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBox1.Location = new System.Drawing.Point(4, 4);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(1015, 197);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // RunButton
            // 
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(55, 24);
            this.RunButton.Text = "Пуск";
            this.RunButton.Click += new System.EventHandler(this.RunParserButton_Click);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CallHelpButton,
            this.AboutProgrammButton});
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.справкаToolStripMenuItem.Text = "Справка";
            // 
            // CallHelpButton
            // 
            this.CallHelpButton.Name = "CallHelpButton";
            this.CallHelpButton.Size = new System.Drawing.Size(197, 26);
            this.CallHelpButton.Text = "Вызов справки";
            this.CallHelpButton.Click += new System.EventHandler(this.CallHelpButton_Click);
            // 
            // AboutProgrammButton
            // 
            this.AboutProgrammButton.Name = "AboutProgrammButton";
            this.AboutProgrammButton.Size = new System.Drawing.Size(197, 26);
            this.AboutProgrammButton.Text = "О программе";
            this.AboutProgrammButton.Click += new System.EventHandler(this.AboutProgrammButton_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewFile_Button,
            this.OpenFile_Button,
            this.SaveFile_Button,
            this.toolStripSeparator1,
            this.Undo_Button,
            this.Redo_Button,
            this.Copy_Button,
            this.Cut_Button,
            this.Paste_Button,
            this.Play_Button});
            this.toolStrip1.Location = new System.Drawing.Point(0, 28);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1043, 39);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // NewFile_Button
            // 
            this.NewFile_Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.NewFile_Button.Image = ((System.Drawing.Image)(resources.GetObject("NewFile_Button.Image")));
            this.NewFile_Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NewFile_Button.Name = "NewFile_Button";
            this.NewFile_Button.Size = new System.Drawing.Size(36, 36);
            this.NewFile_Button.Text = "Создать файл";
            this.NewFile_Button.Click += new System.EventHandler(this.CreateFileButton_Click);
            // 
            // OpenFile_Button
            // 
            this.OpenFile_Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.OpenFile_Button.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.OpenFile_Button.Image = ((System.Drawing.Image)(resources.GetObject("OpenFile_Button.Image")));
            this.OpenFile_Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenFile_Button.Name = "OpenFile_Button";
            this.OpenFile_Button.Size = new System.Drawing.Size(36, 36);
            this.OpenFile_Button.Text = "Открыть файл";
            this.OpenFile_Button.Click += new System.EventHandler(this.OpenFileButton_Click);
            // 
            // SaveFile_Button
            // 
            this.SaveFile_Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveFile_Button.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.SaveFile_Button.Image = ((System.Drawing.Image)(resources.GetObject("SaveFile_Button.Image")));
            this.SaveFile_Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveFile_Button.Name = "SaveFile_Button";
            this.SaveFile_Button.Size = new System.Drawing.Size(36, 36);
            this.SaveFile_Button.Text = "Сохранить файл";
            this.SaveFile_Button.Click += new System.EventHandler(this.SaveFileButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AutoSize = false;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(100, 39);
            // 
            // Undo_Button
            // 
            this.Undo_Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Undo_Button.Image = ((System.Drawing.Image)(resources.GetObject("Undo_Button.Image")));
            this.Undo_Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Undo_Button.Name = "Undo_Button";
            this.Undo_Button.Size = new System.Drawing.Size(36, 36);
            this.Undo_Button.Text = "Отменить";
            this.Undo_Button.Click += new System.EventHandler(this.UndoButton_Click);
            // 
            // Redo_Button
            // 
            this.Redo_Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Redo_Button.Image = ((System.Drawing.Image)(resources.GetObject("Redo_Button.Image")));
            this.Redo_Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Redo_Button.Name = "Redo_Button";
            this.Redo_Button.Size = new System.Drawing.Size(36, 36);
            this.Redo_Button.Text = "Вернуть";
            this.Redo_Button.Click += new System.EventHandler(this.RedoButton_Click);
            // 
            // Copy_Button
            // 
            this.Copy_Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Copy_Button.Image = ((System.Drawing.Image)(resources.GetObject("Copy_Button.Image")));
            this.Copy_Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Copy_Button.Name = "Copy_Button";
            this.Copy_Button.Size = new System.Drawing.Size(36, 36);
            this.Copy_Button.Text = "Копировать";
            this.Copy_Button.Click += new System.EventHandler(this.CopyButton_Click);
            // 
            // Cut_Button
            // 
            this.Cut_Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Cut_Button.Image = ((System.Drawing.Image)(resources.GetObject("Cut_Button.Image")));
            this.Cut_Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Cut_Button.Name = "Cut_Button";
            this.Cut_Button.Size = new System.Drawing.Size(36, 36);
            this.Cut_Button.Text = "Вырезать";
            this.Cut_Button.Click += new System.EventHandler(this.CutButton_Click);
            // 
            // Paste_Button
            // 
            this.Paste_Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Paste_Button.Image = ((System.Drawing.Image)(resources.GetObject("Paste_Button.Image")));
            this.Paste_Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Paste_Button.Name = "Paste_Button";
            this.Paste_Button.Size = new System.Drawing.Size(36, 36);
            this.Paste_Button.Text = "Вставить";
            this.Paste_Button.Click += new System.EventHandler(this.PasteButton_Click);
            // 
            // Play_Button
            // 
            this.Play_Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Play_Button.Image = ((System.Drawing.Image)(resources.GetObject("Play_Button.Image")));
            this.Play_Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Play_Button.Name = "Play_Button";
            this.Play_Button.Size = new System.Drawing.Size(36, 36);
            this.Play_Button.Text = "Пуск";
            this.Play_Button.Click += new System.EventHandler(this.RunParserButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(0, 76);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1035, 238);
            this.tabControl1.TabIndex = 8;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(1027, 209);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "NewFile";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // исходныйКодПрограммыToolStripMenuItem
            // 
            this.исходныйКодПрограммыToolStripMenuItem.Name = "исходныйКодПрограммыToolStripMenuItem";
            this.исходныйКодПрограммыToolStripMenuItem.Size = new System.Drawing.Size(363, 26);
            this.исходныйКодПрограммыToolStripMenuItem.Text = "Исходный код программы";
            this.исходныйКодПрограммыToolStripMenuItem.Click += new System.EventHandler(this.исходныйКодПрограммыToolStripMenuItem_Click);
            // 
            // richTextBox2
            // 
            this.richTextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBox2.Location = new System.Drawing.Point(10, 317);
            this.richTextBox2.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.richTextBox2.Size = new System.Drawing.Size(1015, 194);
            this.richTextBox2.TabIndex = 7;
            this.richTextBox2.Text = "";
            // 
            // списокЛитературыToolStripMenuItem
            // 
            this.списокЛитературыToolStripMenuItem.Name = "списокЛитературыToolStripMenuItem";
            this.списокЛитературыToolStripMenuItem.Size = new System.Drawing.Size(363, 26);
            this.списокЛитературыToolStripMenuItem.Text = "Список литературы";
            this.списокЛитературыToolStripMenuItem.Click += new System.EventHandler(this.списокЛитературыToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.правкаToolStripMenuItem,
            this.текстToolStripMenuItem,
            this.RunButton,
            this.справкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1043, 28);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreateFileButton,
            this.OpenFileButton,
            this.SaveFileButton,
            this.SaveAsFileButton,
            this.ExitProgrammButton});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(59, 24);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // CreateFileButton
            // 
            this.CreateFileButton.Name = "CreateFileButton";
            this.CreateFileButton.Size = new System.Drawing.Size(192, 26);
            this.CreateFileButton.Text = "Создать";
            this.CreateFileButton.Click += new System.EventHandler(this.CreateFileButton_Click);
            // 
            // OpenFileButton
            // 
            this.OpenFileButton.Name = "OpenFileButton";
            this.OpenFileButton.Size = new System.Drawing.Size(192, 26);
            this.OpenFileButton.Text = "Открыть";
            this.OpenFileButton.Click += new System.EventHandler(this.OpenFileButton_Click);
            // 
            // SaveFileButton
            // 
            this.SaveFileButton.Name = "SaveFileButton";
            this.SaveFileButton.Size = new System.Drawing.Size(192, 26);
            this.SaveFileButton.Text = "Сохранить";
            this.SaveFileButton.Click += new System.EventHandler(this.SaveFileButton_Click);
            // 
            // SaveAsFileButton
            // 
            this.SaveAsFileButton.Name = "SaveAsFileButton";
            this.SaveAsFileButton.Size = new System.Drawing.Size(192, 26);
            this.SaveAsFileButton.Text = "Сохранить как";
            this.SaveAsFileButton.Click += new System.EventHandler(this.SaveAsFileButton_Click);
            // 
            // ExitProgrammButton
            // 
            this.ExitProgrammButton.Name = "ExitProgrammButton";
            this.ExitProgrammButton.Size = new System.Drawing.Size(192, 26);
            this.ExitProgrammButton.Text = "Выход";
            this.ExitProgrammButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // правкаToolStripMenuItem
            // 
            this.правкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UndoTextButton,
            this.RedoTextButton,
            this.CutTextButton,
            this.CopyTextButton,
            this.PasteTextButton,
            this.DeleteTextButton,
            this.SelectAllTextButton});
            this.правкаToolStripMenuItem.Name = "правкаToolStripMenuItem";
            this.правкаToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.правкаToolStripMenuItem.Text = "Правка";
            // 
            // UndoTextButton
            // 
            this.UndoTextButton.Name = "UndoTextButton";
            this.UndoTextButton.Size = new System.Drawing.Size(177, 26);
            this.UndoTextButton.Text = "Отменить";
            this.UndoTextButton.Click += new System.EventHandler(this.UndoButton_Click);
            // 
            // RedoTextButton
            // 
            this.RedoTextButton.Name = "RedoTextButton";
            this.RedoTextButton.Size = new System.Drawing.Size(177, 26);
            this.RedoTextButton.Text = "Повторить";
            this.RedoTextButton.Click += new System.EventHandler(this.RedoButton_Click);
            // 
            // CutTextButton
            // 
            this.CutTextButton.Name = "CutTextButton";
            this.CutTextButton.Size = new System.Drawing.Size(177, 26);
            this.CutTextButton.Text = "Вырезать";
            this.CutTextButton.Click += new System.EventHandler(this.CutButton_Click);
            // 
            // CopyTextButton
            // 
            this.CopyTextButton.Name = "CopyTextButton";
            this.CopyTextButton.Size = new System.Drawing.Size(177, 26);
            this.CopyTextButton.Text = "Копировать";
            this.CopyTextButton.Click += new System.EventHandler(this.CopyButton_Click);
            // 
            // PasteTextButton
            // 
            this.PasteTextButton.Name = "PasteTextButton";
            this.PasteTextButton.Size = new System.Drawing.Size(177, 26);
            this.PasteTextButton.Text = "Вставить";
            this.PasteTextButton.Click += new System.EventHandler(this.PasteButton_Click);
            // 
            // DeleteTextButton
            // 
            this.DeleteTextButton.Name = "DeleteTextButton";
            this.DeleteTextButton.Size = new System.Drawing.Size(177, 26);
            this.DeleteTextButton.Text = "Удалить";
            this.DeleteTextButton.Click += new System.EventHandler(this.DeleteTextButton_Click);
            // 
            // SelectAllTextButton
            // 
            this.SelectAllTextButton.Name = "SelectAllTextButton";
            this.SelectAllTextButton.Size = new System.Drawing.Size(177, 26);
            this.SelectAllTextButton.Text = "Выделть все";
            this.SelectAllTextButton.Click += new System.EventHandler(this.SelectAllTextButton_Click);
            // 
            // текстToolStripMenuItem
            // 
            this.текстToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.постановакаЗадачиToolStripMenuItem,
            this.грамматикаToolStripMenuItem,
            this.классификацияГрамматикиToolStripMenuItem,
            this.методАнализаToolStripMenuItem,
            this.диагностикаИНейтрализацияОшибокToolStripMenuItem,
            this.тестовыйПримерToolStripMenuItem,
            this.списокЛитературыToolStripMenuItem,
            this.исходныйКодПрограммыToolStripMenuItem});
            this.текстToolStripMenuItem.Name = "текстToolStripMenuItem";
            this.текстToolStripMenuItem.Size = new System.Drawing.Size(59, 24);
            this.текстToolStripMenuItem.Text = "Текст";
            // 
            // постановакаЗадачиToolStripMenuItem
            // 
            this.постановакаЗадачиToolStripMenuItem.Name = "постановакаЗадачиToolStripMenuItem";
            this.постановакаЗадачиToolStripMenuItem.Size = new System.Drawing.Size(363, 26);
            this.постановакаЗадачиToolStripMenuItem.Text = "Постановака задачи";
            this.постановакаЗадачиToolStripMenuItem.Click += new System.EventHandler(this.постановакаЗадачиToolStripMenuItem_Click);
            // 
            // грамматикаToolStripMenuItem
            // 
            this.грамматикаToolStripMenuItem.Name = "грамматикаToolStripMenuItem";
            this.грамматикаToolStripMenuItem.Size = new System.Drawing.Size(363, 26);
            this.грамматикаToolStripMenuItem.Text = "Грамматика";
            this.грамматикаToolStripMenuItem.Click += new System.EventHandler(this.грамматикаToolStripMenuItem_Click);
            // 
            // классификацияГрамматикиToolStripMenuItem
            // 
            this.классификацияГрамматикиToolStripMenuItem.Name = "классификацияГрамматикиToolStripMenuItem";
            this.классификацияГрамматикиToolStripMenuItem.Size = new System.Drawing.Size(363, 26);
            this.классификацияГрамматикиToolStripMenuItem.Text = "Классификация грамматики";
            this.классификацияГрамматикиToolStripMenuItem.Click += new System.EventHandler(this.классификацияГрамматикиToolStripMenuItem_Click);
            // 
            // методАнализаToolStripMenuItem
            // 
            this.методАнализаToolStripMenuItem.Name = "методАнализаToolStripMenuItem";
            this.методАнализаToolStripMenuItem.Size = new System.Drawing.Size(363, 26);
            this.методАнализаToolStripMenuItem.Text = "Метод анализа";
            this.методАнализаToolStripMenuItem.Click += new System.EventHandler(this.методАнализаToolStripMenuItem_Click);
            // 
            // диагностикаИНейтрализацияОшибокToolStripMenuItem
            // 
            this.диагностикаИНейтрализацияОшибокToolStripMenuItem.Name = "диагностикаИНейтрализацияОшибокToolStripMenuItem";
            this.диагностикаИНейтрализацияОшибокToolStripMenuItem.Size = new System.Drawing.Size(363, 26);
            this.диагностикаИНейтрализацияОшибокToolStripMenuItem.Text = "Диагностика и нейтрализация ошибок";
            this.диагностикаИНейтрализацияОшибокToolStripMenuItem.Click += new System.EventHandler(this.диагностикаИНейтрализацияОшибокToolStripMenuItem_Click);
            // 
            // тестовыйПримерToolStripMenuItem
            // 
            this.тестовыйПримерToolStripMenuItem.Name = "тестовыйПримерToolStripMenuItem";
            this.тестовыйПримерToolStripMenuItem.Size = new System.Drawing.Size(363, 26);
            this.тестовыйПримерToolStripMenuItem.Text = "Тестовый пример";
            this.тестовыйПримерToolStripMenuItem.Click += new System.EventHandler(this.тестовыйПримерToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1043, 531);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Form1";
            this.Text = "Компилятор";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExitCheck);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ToolStripMenuItem RunButton;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CallHelpButton;
        private System.Windows.Forms.ToolStripMenuItem AboutProgrammButton;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton NewFile_Button;
        private System.Windows.Forms.ToolStripButton OpenFile_Button;
        private System.Windows.Forms.ToolStripButton SaveFile_Button;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton Undo_Button;
        private System.Windows.Forms.ToolStripButton Redo_Button;
        private System.Windows.Forms.ToolStripButton Copy_Button;
        private System.Windows.Forms.ToolStripButton Cut_Button;
        private System.Windows.Forms.ToolStripButton Paste_Button;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ToolStripMenuItem исходныйКодПрограммыToolStripMenuItem;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.ToolStripMenuItem списокЛитературыToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CreateFileButton;
        private System.Windows.Forms.ToolStripMenuItem OpenFileButton;
        private System.Windows.Forms.ToolStripMenuItem SaveFileButton;
        private System.Windows.Forms.ToolStripMenuItem SaveAsFileButton;
        private System.Windows.Forms.ToolStripMenuItem ExitProgrammButton;
        private System.Windows.Forms.ToolStripMenuItem правкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem UndoTextButton;
        private System.Windows.Forms.ToolStripMenuItem RedoTextButton;
        private System.Windows.Forms.ToolStripMenuItem CutTextButton;
        private System.Windows.Forms.ToolStripMenuItem CopyTextButton;
        private System.Windows.Forms.ToolStripMenuItem PasteTextButton;
        private System.Windows.Forms.ToolStripMenuItem DeleteTextButton;
        private System.Windows.Forms.ToolStripMenuItem SelectAllTextButton;
        private System.Windows.Forms.ToolStripMenuItem текстToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem постановакаЗадачиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem грамматикаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem классификацияГрамматикиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem методАнализаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem диагностикаИНейтрализацияОшибокToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem тестовыйПримерToolStripMenuItem;
        private ToolStripButton Play_Button;
    }
}

