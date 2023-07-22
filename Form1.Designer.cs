namespace LokoTrain_DBE_comparator_forms
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
            Label_appName = new Label();
            checkBox_Abstimmung = new CheckBox();
            checkBox_NonAbstimmung = new CheckBox();
            backgroundWorker_Comparer = new System.ComponentModel.BackgroundWorker();
            panel_LocoGPS = new Panel();
            button_FileDialog_InputData = new Button();
            label_FileDialog_InputData = new Label();
            label_StartCheck = new Label();
            button_StartCheck = new Button();
            checkBox_Abstimmung_CarrierCalculation = new CheckBox();
            openFileDialog_InputData = new OpenFileDialog();
            label_FileDialog_LokoUsage = new Label();
            button_FileDialog_LokoUsage = new Button();
            openFileDialog_LokoUsage = new OpenFileDialog();
            label_InvoicePrice = new Label();
            textBox_InvoicePrice = new TextBox();
            label_OpenFileDialog_OutputDir = new Label();
            button_OpenFileDialog_OutputDir = new Button();
            folderBrowserDialog_OutputDir = new FolderBrowserDialog();
            SuspendLayout();
            // 
            // Label_appName
            // 
            Label_appName.AutoSize = true;
            Label_appName.Font = new Font("Sylfaen", 26F, FontStyle.Bold, GraphicsUnit.Point);
            Label_appName.Location = new Point(538, 45);
            Label_appName.Margin = new Padding(2, 0, 2, 0);
            Label_appName.Name = "Label_appName";
            Label_appName.Size = new Size(449, 67);
            Label_appName.TabIndex = 0;
            Label_appName.Text = "DBE - Comparator";
            // 
            // checkBox_Abstimmung
            // 
            checkBox_Abstimmung.AutoSize = true;
            checkBox_Abstimmung.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            checkBox_Abstimmung.Location = new Point(125, 315);
            checkBox_Abstimmung.Margin = new Padding(2);
            checkBox_Abstimmung.Name = "checkBox_Abstimmung";
            checkBox_Abstimmung.Size = new Size(291, 36);
            checkBox_Abstimmung.TabIndex = 1;
            checkBox_Abstimmung.Text = "Kontrola přílohy faktury";
            checkBox_Abstimmung.UseVisualStyleBackColor = true;
            checkBox_Abstimmung.CheckedChanged += checkBox_Abstimmung_CheckedChanged;
            // 
            // checkBox_NonAbstimmung
            // 
            checkBox_NonAbstimmung.AutoSize = true;
            checkBox_NonAbstimmung.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            checkBox_NonAbstimmung.Location = new Point(125, 250);
            checkBox_NonAbstimmung.Margin = new Padding(2);
            checkBox_NonAbstimmung.Name = "checkBox_NonAbstimmung";
            checkBox_NonAbstimmung.Size = new Size(247, 36);
            checkBox_NonAbstimmung.TabIndex = 2;
            checkBox_NonAbstimmung.Text = "Předběžná kontrola";
            checkBox_NonAbstimmung.UseVisualStyleBackColor = true;
            checkBox_NonAbstimmung.CheckedChanged += checkBox_NonAbstimmung_CheckedChanged;
            // 
            // backgroundWorker_Comparer
            // 
            backgroundWorker_Comparer.WorkerSupportsCancellation = true;
            backgroundWorker_Comparer.DoWork += backgroundWorker_Comparer_DoWork;
            backgroundWorker_Comparer.RunWorkerCompleted += backgroundWorker_Comparer_RunWorkerCompleted;
            // 
            // panel_LocoGPS
            // 
            panel_LocoGPS.AutoScroll = true;
            panel_LocoGPS.Location = new Point(723, 196);
            panel_LocoGPS.Margin = new Padding(2);
            panel_LocoGPS.Name = "panel_LocoGPS";
            panel_LocoGPS.Size = new Size(758, 568);
            panel_LocoGPS.TabIndex = 3;
            // 
            // button_FileDialog_InputData
            // 
            button_FileDialog_InputData.AutoSize = true;
            button_FileDialog_InputData.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button_FileDialog_InputData.ForeColor = Color.Red;
            button_FileDialog_InputData.Location = new Point(125, 625);
            button_FileDialog_InputData.Margin = new Padding(4);
            button_FileDialog_InputData.Name = "button_FileDialog_InputData";
            button_FileDialog_InputData.Size = new Size(171, 48);
            button_FileDialog_InputData.TabIndex = 4;
            button_FileDialog_InputData.Text = "Otevřít výběr";
            button_FileDialog_InputData.UseVisualStyleBackColor = true;
            button_FileDialog_InputData.Click += button_FileDialog_InputData_Click;
            // 
            // label_FileDialog_InputData
            // 
            label_FileDialog_InputData.AutoSize = true;
            label_FileDialog_InputData.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label_FileDialog_InputData.Location = new Point(125, 583);
            label_FileDialog_InputData.Margin = new Padding(4, 0, 4, 0);
            label_FileDialog_InputData.Name = "label_FileDialog_InputData";
            label_FileDialog_InputData.Size = new Size(392, 32);
            label_FileDialog_InputData.TabIndex = 5;
            label_FileDialog_InputData.Text = "Zvolte vstupní soubor pro kontrolu:";
            // 
            // label_StartCheck
            // 
            label_StartCheck.AutoSize = true;
            label_StartCheck.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label_StartCheck.Location = new Point(125, 799);
            label_StartCheck.Margin = new Padding(4, 0, 4, 0);
            label_StartCheck.Name = "label_StartCheck";
            label_StartCheck.Size = new Size(221, 32);
            label_StartCheck.TabIndex = 7;
            label_StartCheck.Text = "Provedení kontroly:";
            // 
            // button_StartCheck
            // 
            button_StartCheck.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            button_StartCheck.Location = new Point(125, 841);
            button_StartCheck.Margin = new Padding(4);
            button_StartCheck.Name = "button_StartCheck";
            button_StartCheck.Size = new Size(221, 71);
            button_StartCheck.TabIndex = 6;
            button_StartCheck.Text = "Zahájit";
            button_StartCheck.UseVisualStyleBackColor = true;
            button_StartCheck.Click += button_StartCheck_Click;
            // 
            // checkBox_Abstimmung_CarrierCalculation
            // 
            checkBox_Abstimmung_CarrierCalculation.AutoSize = true;
            checkBox_Abstimmung_CarrierCalculation.Enabled = false;
            checkBox_Abstimmung_CarrierCalculation.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            checkBox_Abstimmung_CarrierCalculation.ForeColor = Color.DarkGray;
            checkBox_Abstimmung_CarrierCalculation.Location = new Point(172, 382);
            checkBox_Abstimmung_CarrierCalculation.Margin = new Padding(2);
            checkBox_Abstimmung_CarrierCalculation.Name = "checkBox_Abstimmung_CarrierCalculation";
            checkBox_Abstimmung_CarrierCalculation.Size = new Size(274, 36);
            checkBox_Abstimmung_CarrierCalculation.TabIndex = 8;
            checkBox_Abstimmung_CarrierCalculation.Text = "Rozpad mezi dopravci";
            checkBox_Abstimmung_CarrierCalculation.UseVisualStyleBackColor = true;
            checkBox_Abstimmung_CarrierCalculation.CheckedChanged += checkBox_Abstimmung_CarrierCalculation_CheckedChanged;
            // 
            // openFileDialog_InputData
            // 
            openFileDialog_InputData.FileName = "openFileDialog1";
            // 
            // label_FileDialog_LokoUsage
            // 
            label_FileDialog_LokoUsage.AutoSize = true;
            label_FileDialog_LokoUsage.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label_FileDialog_LokoUsage.Location = new Point(172, 442);
            label_FileDialog_LokoUsage.Margin = new Padding(4, 0, 4, 0);
            label_FileDialog_LokoUsage.Name = "label_FileDialog_LokoUsage";
            label_FileDialog_LokoUsage.Size = new Size(297, 32);
            label_FileDialog_LokoUsage.TabIndex = 10;
            label_FileDialog_LokoUsage.Text = "Zvolte měsíční LokoUsage:";
            label_FileDialog_LokoUsage.Visible = false;
            // 
            // button_FileDialog_LokoUsage
            // 
            button_FileDialog_LokoUsage.AutoSize = true;
            button_FileDialog_LokoUsage.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button_FileDialog_LokoUsage.ForeColor = Color.Red;
            button_FileDialog_LokoUsage.Location = new Point(172, 484);
            button_FileDialog_LokoUsage.Margin = new Padding(4);
            button_FileDialog_LokoUsage.Name = "button_FileDialog_LokoUsage";
            button_FileDialog_LokoUsage.Size = new Size(216, 48);
            button_FileDialog_LokoUsage.TabIndex = 9;
            button_FileDialog_LokoUsage.Text = "Vybrat LokoUsage";
            button_FileDialog_LokoUsage.UseVisualStyleBackColor = true;
            button_FileDialog_LokoUsage.Visible = false;
            button_FileDialog_LokoUsage.Click += button_FileDialog_LokoUsage_Click;
            // 
            // openFileDialog_LokoUsage
            // 
            openFileDialog_LokoUsage.FileName = "openFileDialog2";
            // 
            // label_InvoicePrice
            // 
            label_InvoicePrice.AutoSize = true;
            label_InvoicePrice.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label_InvoicePrice.Location = new Point(723, 796);
            label_InvoicePrice.Margin = new Padding(4, 0, 4, 0);
            label_InvoicePrice.Name = "label_InvoicePrice";
            label_InvoicePrice.Size = new Size(249, 32);
            label_InvoicePrice.TabIndex = 11;
            label_InvoicePrice.Text = "Tarif faktury: [€ / kWh]";
            // 
            // textBox_InvoicePrice
            // 
            textBox_InvoicePrice.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            textBox_InvoicePrice.Location = new Point(998, 796);
            textBox_InvoicePrice.Name = "textBox_InvoicePrice";
            textBox_InvoicePrice.Size = new Size(150, 39);
            textBox_InvoicePrice.TabIndex = 12;
            // 
            // label_OpenFileDialog_OutputDir
            // 
            label_OpenFileDialog_OutputDir.AutoSize = true;
            label_OpenFileDialog_OutputDir.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label_OpenFileDialog_OutputDir.Location = new Point(723, 867);
            label_OpenFileDialog_OutputDir.Margin = new Padding(4, 0, 4, 0);
            label_OpenFileDialog_OutputDir.Name = "label_OpenFileDialog_OutputDir";
            label_OpenFileDialog_OutputDir.Size = new Size(246, 32);
            label_OpenFileDialog_OutputDir.TabIndex = 13;
            label_OpenFileDialog_OutputDir.Text = "Výstupní složka (opt.):";
            // 
            // button_OpenFileDialog_OutputDir
            // 
            button_OpenFileDialog_OutputDir.AutoSize = true;
            button_OpenFileDialog_OutputDir.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button_OpenFileDialog_OutputDir.Location = new Point(977, 859);
            button_OpenFileDialog_OutputDir.Margin = new Padding(4);
            button_OpenFileDialog_OutputDir.Name = "button_OpenFileDialog_OutputDir";
            button_OpenFileDialog_OutputDir.Size = new Size(171, 48);
            button_OpenFileDialog_OutputDir.TabIndex = 14;
            button_OpenFileDialog_OutputDir.Text = "Otevřít výběr";
            button_OpenFileDialog_OutputDir.UseVisualStyleBackColor = true;
            button_OpenFileDialog_OutputDir.Click += button_OpenFileDialog_OutputDir_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1568, 994);
            Controls.Add(button_OpenFileDialog_OutputDir);
            Controls.Add(label_OpenFileDialog_OutputDir);
            Controls.Add(textBox_InvoicePrice);
            Controls.Add(label_InvoicePrice);
            Controls.Add(label_FileDialog_LokoUsage);
            Controls.Add(button_FileDialog_LokoUsage);
            Controls.Add(checkBox_Abstimmung_CarrierCalculation);
            Controls.Add(label_StartCheck);
            Controls.Add(button_StartCheck);
            Controls.Add(label_FileDialog_InputData);
            Controls.Add(button_FileDialog_InputData);
            Controls.Add(panel_LocoGPS);
            Controls.Add(checkBox_NonAbstimmung);
            Controls.Add(checkBox_Abstimmung);
            Controls.Add(Label_appName);
            Margin = new Padding(2);
            Name = "Form1";
            Text = "+";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Label_appName;
        private CheckBox checkBox_Abstimmung;
        private CheckBox checkBox_NonAbstimmung;
        private System.ComponentModel.BackgroundWorker backgroundWorker_Comparer;
        private Panel panel_LocoGPS;
        private Button button_FileDialog_InputData;
        private Label label_FileDialog_InputData;
        private Label label_StartCheck;
        private Button button_StartCheck;
        private CheckBox checkBox_Abstimmung_CarrierCalculation;
        private OpenFileDialog openFileDialog_InputData;
        private Label label_FileDialog_LokoUsage;
        private Button button_FileDialog_LokoUsage;
        private OpenFileDialog openFileDialog_LokoUsage;
        private Label label_InvoicePrice;
        private TextBox textBox_InvoicePrice;
        private TextBox textBox1;
        private Label label_OpenFileDialog_OutputDir;
        private Button button_OpenFileDialog_OutputDir;
        private FolderBrowserDialog folderBrowserDialog_OutputDir;
    }
}