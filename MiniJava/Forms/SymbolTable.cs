using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
