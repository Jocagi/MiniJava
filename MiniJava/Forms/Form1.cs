using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MiniJava.Lexer;
using MiniJava.Parser.Ascendente.TableGenerator;
using MiniJava.Parser.RecursiveDescent;

namespace MiniJava.Forms
{
    public partial class Form1 : Form
    {
        string output = "";

        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;
            Debug.WriteLine("Program Started");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show(@"Archivo seleccionado exitosamente");

                ReadFile(openFileDialog1.FileName);
            }
            else 
            {
                MessageBox.Show(@"No se ha seleccionado ningún archivo");
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? 
                DragDropEffects.All : DragDropEffects.None;
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
                string line = "";

                switch (item.tokenType)
                {
                    case TokenType.Enter:
                    case TokenType.WhiteSpace:
                    case TokenType.Block_Comments:
                    case TokenType.Comments:
                        line = "";
                        break;
                    case TokenType.Error:
                        line = $"*** Error line {item.location.row}.*** Unrecognized element: {item.value}\r\n";
                        break;
                    case TokenType.Error_Comment:
                        line = $"*** Error line {item.location.row}.*** Unfinished comment.\r\n";
                        break;
                    case TokenType.Error_Length:
                        line = $"*** Warning: Max id length exceeded at line {item.location.row}.***\r\n";
                        break;
                    case TokenType.Error_EOFComment:
                        line = $"*** Error line {item.location.row}.*** EOF in comment.\r\n";
                        break;
                    case TokenType.Error_nullString:
                        line = $"*** Error line {item.location.row}.*** Null character in string.\r\n";
                        break;
                    case TokenType.Error_null:
                        line = $"*** Error line {item.location.row}.*** Null character.\r\n";
                        break;
                    case TokenType.Error_EOFstring:
                        line = $"*** Error line {item.location.row}.*** EOF in string.\r\n";
                        break;
                    case TokenType.Error_String:
                        line = $"*** Error line {item.location.row}.*** Unfinished string.\r\n";
                        break;
                    case TokenType.Error_UnpairedComment:
                        line = $"*** Error line {item.location.row}.*** Unpaired comment.\r\n";
                        break;
                    default:
                        //line =
                    //$"{item.value} \t\t\t>> {item.tokenType} Line: {item.location.row}  Col: [{item.location.firstCol}:{item.location.lastCol}]\r\n";
                        break;
                }

                output += line;
            }

            //Analizador sintactico
            Queue<Token> tokensQueue = lex.ListToQueue(tokens);
            Parser.RecursiveDescent.Parser pars = new Parser.RecursiveDescent.Parser(tokensQueue);
            ParserReport parserReport = pars.getReport();

            output += parserReport.isCorrect ? "Todo bien :)\n" : "Oh! No! Hay un error\n";

            if (!parserReport.isCorrect)
            {
                foreach (var item in parserReport.Errors)
                {
                    output += $"*** Error sintáctico en linea {item.location.row}: Se encontró {item.value}, se esperaba {item.expected} ***\n";
                }
            }

            //Show output
            this.outputBox.Text = output.Replace("\0", "<null>"); //Prevenir que el null corte el texto
            this.outputBox.Visible = true;
            
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
                MessageBox.Show(@"Archivo guardado exitosamente");
            }
            else
            {
                MessageBox.Show(@"No se ha especificado ninguna ruta");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CanonicalCollection cc = new CanonicalCollection(new Grammar());
           
            TableInfo t = new TableInfo(cc);
            t.Show();
        }
    }
}
