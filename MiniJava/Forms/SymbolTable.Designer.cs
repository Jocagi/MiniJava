namespace MiniJava.Forms
{
    partial class SymbolTable
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SymbolTable));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.labelActualTable = new System.Windows.Forms.Label();
            this.labelTotalTables = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nextButton = new System.Windows.Forms.Button();
            this.previousButtom = new System.Windows.Forms.Button();
            this.saveButtom = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.Size = new System.Drawing.Size(1099, 630);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.Text = "dataGridView1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1142, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tabla:";
            // 
            // labelActualTable
            // 
            this.labelActualTable.AutoSize = true;
            this.labelActualTable.Location = new System.Drawing.Point(1224, 12);
            this.labelActualTable.Name = "labelActualTable";
            this.labelActualTable.Size = new System.Drawing.Size(22, 25);
            this.labelActualTable.TabIndex = 2;
            this.labelActualTable.Text = "0";
            // 
            // labelTotalTables
            // 
            this.labelTotalTables.AutoSize = true;
            this.labelTotalTables.Location = new System.Drawing.Point(1285, 12);
            this.labelTotalTables.Name = "labelTotalTables";
            this.labelTotalTables.Size = new System.Drawing.Size(22, 25);
            this.labelTotalTables.TabIndex = 2;
            this.labelTotalTables.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1255, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "/";
            // 
            // nextButton
            // 
            this.nextButton.Location = new System.Drawing.Point(1142, 65);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(165, 34);
            this.nextButton.TabIndex = 3;
            this.nextButton.Text = "Siguiente";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // previousButtom
            // 
            this.previousButtom.Location = new System.Drawing.Point(1142, 105);
            this.previousButtom.Name = "previousButtom";
            this.previousButtom.Size = new System.Drawing.Size(165, 34);
            this.previousButtom.TabIndex = 3;
            this.previousButtom.Text = "Anterior";
            this.previousButtom.UseVisualStyleBackColor = true;
            this.previousButtom.Click += new System.EventHandler(this.previousButtom_Click);
            // 
            // saveButtom
            // 
            this.saveButtom.Location = new System.Drawing.Point(1142, 590);
            this.saveButtom.Name = "saveButtom";
            this.saveButtom.Size = new System.Drawing.Size(165, 34);
            this.saveButtom.TabIndex = 3;
            this.saveButtom.Text = "Guardar";
            this.saveButtom.UseVisualStyleBackColor = true;
            this.saveButtom.Click += new System.EventHandler(this.saveButtom_Click);
            // 
            // SymbolTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1389, 654);
            this.Controls.Add(this.saveButtom);
            this.Controls.Add(this.previousButtom);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelTotalTables);
            this.Controls.Add(this.labelActualTable);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SymbolTable";
            this.Text = "SymbolTable";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelActualTable;
        private System.Windows.Forms.Label labelTotalTables;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Button previousButtom;
        private System.Windows.Forms.Button saveButtom;
    }
}