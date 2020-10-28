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
        }

        private void showCollection()
        {
            foreach (var item in collection.States)
            {
                this.richTextBox1.Text += $"Estado {item.ID}: \n";

                foreach (var lritem in item.items)
                {
                    this.richTextBox1.Text += $"\t ->";
                    this.richTextBox1.Text += $" {lritem.Production.LeftSide}";

                    int i = 0;

                    foreach (var token in lritem.Production.RightSide)
                    {
                        this.richTextBox1.Text += i == lritem.Position ? $"{token}" : $"•{token}";
                        i++;
                    }

                    this.richTextBox1.Text += $" Lookahead: ";

                    foreach (var la in lritem.lookahead)
                    {
                        this.richTextBox1.Text += $"{la} ";
                        i++;
                    }

                    this.richTextBox1.Text += $" Action: {lritem.action} \n";
                }
            }
        }
    }
}
