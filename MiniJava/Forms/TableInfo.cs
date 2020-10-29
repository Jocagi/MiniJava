using System.Collections.Generic;
using System.Windows.Forms;
using MiniJava.Parser.Ascendente.Parser;
using MiniJava.Parser.Ascendente.TableGenerator.LR1;
// ReSharper disable LocalizableElement

namespace MiniJava.Forms
{
    public partial class TableInfo : Form
    {
        private readonly CanonicalCollection collection;

        public TableInfo()
        {
            InitializeComponent();
        }

        public TableInfo(CanonicalCollection cc)
        {
            InitializeComponent();
            collection = cc;
            showCollection();
            loadTable();
        }

        private void showCollection()
        {
            foreach (var item in collection.States)
            {
                if (item != null)
                {
                    richTextBox1.Text += $"Estado {item.ID}: \n";

                    foreach (var lritem in item.items)
                    {
                        richTextBox1.Text += $"\t ->";
                        richTextBox1.Text += $" \t {lritem.Production.LeftSide} =>";

                        int i = 0;

                        foreach (var token in lritem.Production.RightSide)
                        {
                            richTextBox1.Text += i == lritem.Position ? "•" : "";
                            richTextBox1.Text += $"{token}";
                            i++;
                        }

                        richTextBox1.Text += lritem.Position > lritem.Production.RightSide.Count ? "•" : "";

                        richTextBox1.Text += $" \t\t\tLookahead: ";

                        foreach (var la in lritem.lookahead)
                        {
                            richTextBox1.Text += $"{la} ";
                            i++;
                        }

                        richTextBox1.Text += $" \tAction: {lritem.action}";

                        if (lritem.shiftTo >= 0)
                        {
                            richTextBox1.Text += $" \tGO_TO: {lritem.shiftTo} \n";
                        }
                        else
                        {
                            richTextBox1.Text += "\n";
                        }

                        string text = richTextBox1.Text.Replace("Operator_mas", "+").Replace("NT_Start", "S'")
                            .Replace("NT_ExampleE", "E").Replace("NT_ExampleT", "T").Replace("entifier", "")
                            .Replace("NT_ExampleS", "S").Replace("NT_ExampleV", "V")
                            .Replace("Operator_puntosIgual", ":=")
                            .Replace("Const_Int", "num");

                        richTextBox1.Text = text;
                    }
                }
            }
        }

        private void loadTable()
        {
            Table tabla = new Table(collection);

            //Columnas
            dataGridView1.Columns.Add("ESTADO", "ESTADO");
            foreach (var token in tabla.tokens)
            {
                dataGridView1.Columns.Add($"{token}", $"{token}");
            }

            //Estados
            int stateNumber = 0;
            foreach (var state in tabla.states)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[stateNumber].Cells[0].Value = $"{stateNumber}";

                foreach (var action in state.actions)
                {
                    for (int i = 0; i < tabla.tokens.Count; i++)
                    {
                        if (tabla.tokens[i] == action.symbol)
                        {
                            dataGridView1.Rows[stateNumber].Cells[i+1].Value += $" {action.accion} {action.estado}";
                            break;
                        }
                    }
                }

                stateNumber++;
            }
        }
    }
}