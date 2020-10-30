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
        private string textCC;

        public TableInfo()
        {
            InitializeComponent();
        }

        public TableInfo(CanonicalCollection cc)
        {
            InitializeComponent();
            collection = cc;
            loadObjects();
        }

        private void loadObjects()
        {
            //Coleccion Canonica
            System.Threading.ThreadStart
                FStart1 = showCollection;
            System.Threading.Thread ThreadCC =
                new System.Threading.Thread(FStart1);
            ThreadCC.Start();
            
            loadTable();
        }

        private void showCollection()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            textCC = "";

            foreach (var item in collection.States)
            {
                if (item != null)
                {
                    textCC += $"Estado {item.ID}: \n";

                    foreach (var lritem in item.items)
                    {
                        textCC += $"\t ->";
                        textCC += $" \t {lritem.Production.LeftSide} =>";

                        int i = 0;

                        foreach (var token in lritem.Production.RightSide)
                        {
                            textCC += i == lritem.Position ? "•" : "";
                            textCC += $" {token} ";
                            i++;
                        }

                        textCC += lritem.Position > lritem.Production.RightSide.Count ? "•" : "";

                        textCC += $" \t\t\tLookahead: ";

                        foreach (var la in lritem.lookahead)
                        {
                            textCC += $"{la} ";
                            i++;
                        }

                        textCC += $" \tAction: {lritem.action}";

                        if (lritem.shiftTo >= 0)
                        {
                            textCC += $" \tGO_TO: {lritem.shiftTo} \n";
                        }
                        else
                        {
                            textCC += "\n";
                        }
                    }
                }
            }
            richTextBox1.Text = textCC;
        }

        private void loadTable()
        {
            Table tabla = new Table(collection);
            string conflictosMensaje = "Conflicto en Estado: ";
            int conflictos = 0;
            List<int> stateConflict = new List<int>();

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
                            //Anunciar conflicto
                            if (dataGridView1.Rows[stateNumber].Cells[i + 1].Value != null)
                            {
                                if (!stateConflict.Contains(stateNumber))
                                {
                                    conflictosMensaje += $"{stateNumber},";
                                    conflictos++;
                                    stateConflict.Add(stateNumber);
                                }
                            }

                            //Agregar valor
                            if (action.accion != ActionType.Accept && action.accion != ActionType.Error)
                            {
                                dataGridView1.Rows[stateNumber].Cells[i + 1].Value += $" {action.accion} {action.estado}";
                            }
                            else
                            {
                                dataGridView1.Rows[stateNumber].Cells[i + 1].Value += $" {action.accion} ";
                            }
                            break;
                        }
                    }
                }

                stateNumber++;
            }

            if (conflictos > 0)
            {
                MessageBox.Show(conflictosMensaje);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}