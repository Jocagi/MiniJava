using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniJava.Lexer;

namespace MiniJava
{
    public partial class Form1 : Form
    {
        string output = "";

        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;

            this.labelOUTPUT.Visible = false;
            this.outputBox.Visible = false;
            this.Size = new Size(645, 715);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Archivo seleccionado exitosamente");

                ReadFile(openFileDialog1.FileName);
            }
            else 
            {
                MessageBox.Show("No se ha seleccionado ningún archivo");
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            
            ReadFile(files[0]);
        }

        private void ReadFile(string path)
        {
            string text = File.ReadAllText(path);
            output = "";

            //Show text
            labelFileName.Text = Path.GetFileName(path);
            sourceCodeBox.Text = text.Replace("\0", "<null>"); //Prevenir que el null corte el texto

            //Analizar tokens
            Lexer.Lexer lex = new Lexer.Lexer();
            var tokens = lex.getTokens(text);

            foreach (var item in tokens)
            {
                string line;

                if (item.tokenType != TokenType.WhiteSpace && item.tokenType != TokenType.Enter
                    && item.tokenType != TokenType.Block_Comments && item.tokenType != TokenType.Comments)
                {
                    if (item.tokenType == TokenType.Error)
                    {
                        line = $"*** Error line {item.location.row}.*** Unrecognized element: {item.value}\r\n";
                    }
                    else if (item.tokenType == TokenType.Error_Comment)
                    {
                        line = $"*** Error unfinished comment at line {item.location.row}.***\r\n";
                    }
                    else if (item.tokenType == TokenType.Error_Length)
                    {
                        line = $"*** Warning: Max id length exceeded at line {item.location.row}.***\r\n";
                    }
                    else if (item.tokenType == TokenType.Error_EOFComnet)
                    {
                        line = $"*** Error: EOF in comment at line { item.location.row}.***\r\n";
                    }
                    else if (item.tokenType == TokenType.Error_EOFstring)
                    {
                        line = $"*** Error: EOF in string at line { item.location.row}.***\r\n";
                    }
                    else if (item.tokenType == TokenType.Error_String)
                    {
                        line = $"*** Error unfinished string at line {item.location.row}.***\r\n";
                    }
                    else
                    {
                        line =
                        $"{item.value} \t\t\t>> {item.tokenType} Line: {item.location.row}  Col: [{item.location.firstCol}:{item.location.lastCol}]\r\n";
                    }

                    output += line;
                }
            }

            //Show output

            this.outputBox.Text = output.Replace("\0", "<null>"); //Prevenir que el null corte el texto
            this.labelOUTPUT.Visible = true;
            this.outputBox.Visible = true;
            this.Size = new Size(1500, 715);

            this.saveButton.Visible = true;

            //Color errors

            for (int i = 0; i < outputBox.Lines.Length; i++)
            {
                string line = outputBox.Lines[i];

                if (line.Contains("***"))
                {
                    outputBox.Select(outputBox.GetFirstCharIndexFromLine(i), line.Length);
                    outputBox.SelectionColor = Color.Red;
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, output);
                MessageBox.Show("Archivo guardado exitosamente");
            }
            else
            {
                MessageBox.Show("No se ha especificado ninguna ruta");
            }
        }
    }
}
