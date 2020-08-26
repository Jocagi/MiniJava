namespace MiniJava
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
            this.labelOUTPUT = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(9, 10);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(142, 41);
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
            this.sourceCodeBox.Location = new System.Drawing.Point(9, 53);
            this.sourceCodeBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.sourceCodeBox.Name = "sourceCodeBox";
            this.sourceCodeBox.ReadOnly = true;
            this.sourceCodeBox.ShowSelectionMargin = true;
            this.sourceCodeBox.Size = new System.Drawing.Size(584, 494);
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
            this.outputBox.Location = new System.Drawing.Point(604, 53);
            this.outputBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.outputBox.Name = "outputBox";
            this.outputBox.ReadOnly = true;
            this.outputBox.ShowSelectionMargin = true;
            this.outputBox.Size = new System.Drawing.Size(568, 494);
            this.outputBox.TabIndex = 1;
            this.outputBox.Text = "";
            this.outputBox.WordWrap = false;
            // 
            // labelOUTPUT
            // 
            this.labelOUTPUT.AutoSize = true;
            this.labelOUTPUT.Font = new System.Drawing.Font("Courier New", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelOUTPUT.Location = new System.Drawing.Point(604, 10);
            this.labelOUTPUT.Name = "labelOUTPUT";
            this.labelOUTPUT.Size = new System.Drawing.Size(150, 41);
            this.labelOUTPUT.TabIndex = 2;
            this.labelOUTPUT.Text = "OUTPUT";
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1178, 559);
            this.Controls.Add(this.labelOUTPUT);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.sourceCodeBox);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
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
        private System.Windows.Forms.Label labelOUTPUT;
    }
}

