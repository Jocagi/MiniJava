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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Archivo seleccionado exitosamente");



                string text = File.ReadAllText(openFileDialog1.FileName);
                string output = "";

                Lexer.Lexer lex = new Lexer.Lexer();

                var tokens = lex.getTokens(text);

                foreach (var item in tokens)
                {
                    output += $"{item.value} >> {item.tokenType} | \r\n";
                }

                MessageBox.Show(output);

            }
            else 
            {
                MessageBox.Show("No se ha seleccionado ningún archivo");
            }
        }
    }
}
