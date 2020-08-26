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
                if (item.tokenType != TokenType.WhiteSpace && item.tokenType != TokenType.Enter)
                {
                    if (item.tokenType == TokenType.Error)
                    {
                        output += $"*** Error line {item.location.row}.*** Unrecognized element: {item.value}\r\n";
                    }
                    else
                    {
                        output +=
                        $"{item.value} \t>> {item.tokenType} Line: {item.location.row}  Col: [{item.location.firstCol}:{item.location.lastCol}]\r\n";
                    }
                }
            }

            MessageBox.Show(output);
        }
    }
}
