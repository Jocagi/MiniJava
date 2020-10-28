using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MiniJava.Parser.Ascendente.TableGenerator;

namespace MiniJava.Forms
{
    public partial class TableInfo : Form
    {
        private CanonicalCollection collection;

        public TableInfo()
        {
            InitializeComponent();
        }

        public TableInfo(CanonicalCollection cc)
        {
            InitializeComponent();
            this.collection = cc;
            showCollection();
        }

        private void showCollection()
        {
            foreach (var item in collection.States)
            {
                this.richTextBox1.Text += $"Estado {item.ID}: \n";

                foreach (var lritem in item.items)
                {
                    this.richTextBox1.Text += $"\t ->";
                    this.richTextBox1.Text += $" \t {lritem.Production.LeftSide} =>";

                    int i = 0;

                    foreach (var token in lritem.Production.RightSide)
                    {
                        this.richTextBox1.Text += i == lritem.Position ? "•" : "";
                        this.richTextBox1.Text += $"{token}";
                        i++;
                    }
                    this.richTextBox1.Text += lritem.Position > lritem.Production.RightSide.Count ? "•" : "";

                    this.richTextBox1.Text += $" \t\t\tLookahead: ";

                    foreach (var la in lritem.lookahead)
                    {
                        this.richTextBox1.Text += $"{la} ";
                        i++;
                    }

                    this.richTextBox1.Text += $" \tAction: {lritem.action} \n";

                    string text = this.richTextBox1.Text.Replace("Operator_mas", "+").Replace("NT_Start", "S")
                        .Replace("NT_ExampleE", "E").Replace("NT_ExampleT", "T").Replace("entifier", "");

                    this.richTextBox1.Text = text;
                }
            }
        }
    }
}
