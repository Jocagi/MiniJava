using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MiniJava.General;
using MiniJava.Lexer;
using MiniJava.Parser.Ascendente.TableGenerator.Grammar;
using MiniJava.Parser.Ascendente.TableGenerator.LR1;
using MiniJava.Parser.Descendente;
using ScintillaNET;

namespace MiniJava.Forms
{
    public partial class Form1 : Form
    {
        string output = "";
        private ParserReport parserRep;

        public Form1()
        {
            InitializeComponent();
            InitializeTextBox();
            this.AllowDrop = true;
            Debug.WriteLine("Program Started");
        }

        private void InitializeTextBox()
        {
            sourceCodeBox.StyleClearAll();

            sourceCodeBox.Styles[Style.Default].BackColor = Color.Black;
            sourceCodeBox.Margins[0].Width = 25;
            sourceCodeBox.Margins[1].Width = 16;

            sourceCodeBox.Styles[Style.Cpp.Default].BackColor = Color.Black;
            sourceCodeBox.Styles[Style.Cpp.Character].BackColor = Color.Black;
            sourceCodeBox.Styles[Style.Cpp.Identifier].BackColor = Color.Black;
            sourceCodeBox.Styles[Style.Cpp.Comment].BackColor = Color.Black;
            sourceCodeBox.Styles[Style.Cpp.CommentLine].BackColor = Color.Black;
            sourceCodeBox.Styles[Style.Cpp.EscapeSequence].BackColor = Color.Black;
            sourceCodeBox.Styles[Style.Cpp.Number].BackColor = Color.Black;
            sourceCodeBox.Styles[Style.Cpp.Operator].BackColor = Color.Black;
            sourceCodeBox.Styles[Style.Cpp.String].BackColor = Color.Black;

            sourceCodeBox.Styles[Style.Cpp.Default].ForeColor = Color.AliceBlue;
            sourceCodeBox.Styles[Style.Cpp.Character].ForeColor = Color.Coral;
            sourceCodeBox.Styles[Style.Cpp.Identifier].ForeColor = Color.White;
            sourceCodeBox.Styles[Style.Cpp.Comment].ForeColor = Color.Green;
            sourceCodeBox.Styles[Style.Cpp.CommentLine].ForeColor = Color.Green;
            sourceCodeBox.Styles[Style.Cpp.EscapeSequence].ForeColor = Color.Brown;
            sourceCodeBox.Styles[Style.Cpp.Number].ForeColor = Color.BurlyWood; 
            sourceCodeBox.Styles[Style.Cpp.Operator].ForeColor = Color.Brown;
            sourceCodeBox.Styles[Style.Cpp.String].ForeColor = Color.BurlyWood; 
            
            sourceCodeBox.Lexer = ScintillaNET.Lexer.Cpp;
            sourceCodeBox.CaretLineBackColor = Color.DarkGray;
            sourceCodeBox.CaretLineVisible = true;

            sourceCodeBox.SetSelectionForeColor(true, Color.AliceBlue);
            sourceCodeBox.SetSelectionBackColor(true, Color.Black);
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
            Queue<Token> tokensQueue = lex.ListToQueue(tokens);
            Parser.Descendente.Parser pars1 = new Parser.Descendente.Parser(tokensQueue);
            ParserReport parserReport = pars1.getReport();

            this.parserRep = parserReport;

            //ANALIZADOR SINTACTICO
            
            output += parserReport.isCorrect ? "Todo bien :)\n" : "Oh! No! Hay un error\n";

            if (!parserReport.isCorrect)
            {
                TokenLocation location = new TokenLocation(0,0,0);
                foreach (var item in parserReport.Errors)
                {
                    if (location != item.location)
                    {
                        output += $"*** Error sintáctico en linea {item.location.row}: Token inesperado {item.value} ***\n";
                    }
                    location = item.location;
                }
            }

            //Show output
            this.outputBox.Text = output.Replace("\0", "<null>"); //Prevenir que el null corte el texto
            this.outputBox.Visible = true;
            this.symbolButtom.Visible = true;

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

        private void symbolButtom_Click(object sender, EventArgs e)
        {
            SymbolTable t = new SymbolTable(parserRep);
            t.Show();
        }
    }
}
