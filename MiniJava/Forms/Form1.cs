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
        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;

            this.labelOUTPUT.Visible = false;
            this.outputBox.Visible = false;
            this.Size = new Size(625, 615);
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
            string output = "";

            sourceCodeBox.Text = text;

            Lexer.Lexer lex = new Lexer.Lexer();

            var tokens = lex.getTokens(text);

            foreach (var item in tokens)
            {
                if (item.tokenType != TokenType.WhiteSpace && item.tokenType != TokenType.Enter
                    && item.tokenType != TokenType.Block_Comments && item.tokenType != TokenType.Comments)
                {
                    if (item.tokenType == TokenType.Error)
                    {
                        output += $"*** Error line {item.location.row}.*** Unrecognized element: {item.value}\r\n";
                    }
                    else if (item.tokenType == TokenType.Error_Comment)
                    {
                        output += $"*** Error unfinished comment at line {item.location.row}.***\r\n";
                    }
                    else
                    {
                        output +=
                        $"{item.value} \t\t>> {item.tokenType} Line: {item.location.row}  Col: [{item.location.firstCol}:{item.location.lastCol}]\r\n";
                    }
                }
            }

            //Show output

            this.outputBox.Text = output;

            this.labelOUTPUT.Visible = true;
            this.outputBox.Visible = true;
            this.Size = new Size(1200, 615);
        }
    }
}
