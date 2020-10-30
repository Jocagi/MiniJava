﻿namespace MiniJava.Forms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.sourceCodeBox = new System.Windows.Forms.RichTextBox();
            this.outputBox = new System.Windows.Forms.RichTextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.labelFileName = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.ErrorBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button1.Location = new System.Drawing.Point(8, 8);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 34);
            this.button1.TabIndex = 0;
            this.button1.Text = "Archivo";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "All files (*.*)|*.*|Archivos de texto (*.frag)|*.frag";
            // 
            // sourceCodeBox
            // 
            this.sourceCodeBox.AcceptsTab = true;
            this.sourceCodeBox.BackColor = System.Drawing.SystemColors.MenuText;
            this.sourceCodeBox.EnableAutoDragDrop = true;
            this.sourceCodeBox.ForeColor = System.Drawing.SystemColors.Info;
            this.sourceCodeBox.Location = new System.Drawing.Point(8, 44);
            this.sourceCodeBox.Margin = new System.Windows.Forms.Padding(2);
            this.sourceCodeBox.Name = "sourceCodeBox";
            this.sourceCodeBox.ReadOnly = true;
            this.sourceCodeBox.ShowSelectionMargin = true;
            this.sourceCodeBox.Size = new System.Drawing.Size(852, 427);
            this.sourceCodeBox.TabIndex = 1;
            this.sourceCodeBox.Text = "";
            this.sourceCodeBox.WordWrap = false;
            // 
            // outputBox
            // 
            this.outputBox.AcceptsTab = true;
            this.outputBox.BackColor = System.Drawing.SystemColors.MenuText;
            this.outputBox.EnableAutoDragDrop = true;
            this.outputBox.ForeColor = System.Drawing.SystemColors.Info;
            this.outputBox.Location = new System.Drawing.Point(9, 474);
            this.outputBox.Margin = new System.Windows.Forms.Padding(2);
            this.outputBox.Name = "outputBox";
            this.outputBox.ReadOnly = true;
            this.outputBox.ShowSelectionMargin = true;
            this.outputBox.Size = new System.Drawing.Size(851, 254);
            this.outputBox.TabIndex = 1;
            this.outputBox.Text = "";
            this.outputBox.WordWrap = false;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.AddExtension = false;
            this.saveFileDialog1.DefaultExt = "out";
            this.saveFileDialog1.FileName = "Output";
            this.saveFileDialog1.Filter = "Out files|*.out";
            this.saveFileDialog1.Title = "miniJava";
            // 
            // labelFileName
            // 
            this.labelFileName.AutoSize = true;
            this.labelFileName.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelFileName.Location = new System.Drawing.Point(132, 20);
            this.labelFileName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(0, 23);
            this.labelFileName.TabIndex = 4;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(589, 8);
            this.button2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(269, 28);
            this.button2.TabIndex = 5;
            this.button2.Text = "Mostrar Tabla";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ErrorBox
            // 
            this.ErrorBox.AcceptsTab = true;
            this.ErrorBox.BackColor = System.Drawing.SystemColors.MenuText;
            this.ErrorBox.EnableAutoDragDrop = true;
            this.ErrorBox.ForeColor = System.Drawing.SystemColors.Info;
            this.ErrorBox.Location = new System.Drawing.Point(8, 732);
            this.ErrorBox.Name = "ErrorBox";
            this.ErrorBox.ReadOnly = true;
            this.ErrorBox.ShowSelectionMargin = true;
            this.ErrorBox.Size = new System.Drawing.Size(852, 201);
            this.ErrorBox.TabIndex = 1;
            this.ErrorBox.Text = "";
            this.ErrorBox.WordWrap = false;
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(868, 955);
            this.Controls.Add(this.ErrorBox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.labelFileName);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.sourceCodeBox);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "MiniJava";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.RichTextBox sourceCodeBox;
        private System.Windows.Forms.RichTextBox outputBox;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RichTextBox ErrorBox;
    }
}

