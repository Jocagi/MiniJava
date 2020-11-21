using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MiniJava.Parser.Ascendente.Parser;
using MiniJava.Parser.Descendente;

namespace MiniJava.Forms
{
    public partial class SymbolTable : Form
    {
        private ParserReport parserReport;
        private int page = 0;

        public SymbolTable()
        {
            InitializeComponent();
            printTable();
        }
        public SymbolTable(ParserReport report)
        {
            this.parserReport = report;
            InitializeComponent();
            printTable();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {

        }

        private void previousButtom_Click(object sender, EventArgs e)
        {

        }

        private void saveButtom_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FillOutput();
                File.WriteAllText(saveFileDialog1.FileName, output);
                MessageBox.Show(@"Archivo guardado exitosamente");
            }
            else
            {
                MessageBox.Show(@"No se ha especificado ninguna ruta");
            }
        }
        string output;
        void FillOutput()
        {
            foreach (var item in parserReport.TablaSimbolos)
            {
                foreach (var item2 in item)
                {
                    output += item2.ID.ToString();
                    output += "          --        ";
                    output += item2.dataType.ToString();
                    output += "         --        ";
                    output += item2.type.ToString();
                    output += "         --      ";
                    output += item2.scope.ToString();
                    output += "          --          ";
                    output += item2.value.ToString();
                    output += "\r\n";
                }
                output += "\r\n";
                output += "\r\n";
                output += "\r\n";
            }
        }


        private void printTable()
        {
            //Columnas
            dataGridView1.Columns.Add("SIMBOLO", "SIMBOLO");
            dataGridView1.Columns.Add("TIPO_DATO", "TIPO_DATO");
            dataGridView1.Columns.Add("TIPO_SIMBOLO", "TIPO_SIMBOLO");
            dataGridView1.Columns.Add("AMBITO", "AMBITO");
            dataGridView1.Columns.Add("VALOR", "VALOR");

            //Values
            int i = 0;
            foreach (var value in parserReport.TablaSimbolos[page])
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = value.ID;
                dataGridView1.Rows[i].Cells[1].Value = value.dataType.ToString();
                dataGridView1.Rows[i].Cells[2].Value = value.type.ToString();
                dataGridView1.Rows[i].Cells[3].Value = value.scope;
                dataGridView1.Rows[i].Cells[4].Value = value.value;
                i++;
            }
        }
    }
}
