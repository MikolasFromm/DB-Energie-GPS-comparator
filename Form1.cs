using LokoTrain_DBE_comparator_forms.Structures;
using LokoTrain_DBE_comparator_forms.Wrappers;
using System.ComponentModel;

namespace LokoTrain_DBE_comparator_forms
{
    public partial class Form1 : Form
    {
        // input file paths
        public string inputFilePath { get; set; } = string.Empty;
        public string lokoUsageFilePath { get; set; } = string.Empty;
        public string outputDir { get; set; } = string.Empty;


        // GPS input paths and loaded flags
        public Dictionary<LocoId, GpsLocoFilePath> gpsMapping { get; set; } = new();

        // wrappers
        IDBE_wrapper DbeWrapper { get; set; }
        IGPS_wrapper GpsWrapper { get; set; }
        ILokoUsage_wrapper LokoUsageWrapper { get; set; }
        IExporter Exporter { get; set; }


        // future operation
        CheckMethod checkMethod = CheckMethod.None;
        bool lokoUsageLoaded = false;

        public Form1()
        {
            InitializeComponent();
            GpsWrapper = new GPS_wrapper();
        }

        public void AddGPSLokoInputs(HashSet<LocoId> locos)
        {
            int basex = this.panel_LocoGPS.Location.X;
            int basey = this.panel_LocoGPS.Location.Y;

            var label_ChooseGpsToEachLoco = new Label();
            label_ChooseGpsToEachLoco.Text = "Vyberte GPS záznam pro každou lokomotivu:";
            label_ChooseGpsToEachLoco.Location = new Point(0, 0);
            label_ChooseGpsToEachLoco.AutoSize = true;
            label_ChooseGpsToEachLoco.Font = new Font("Segoe UI", 12);
            panel_LocoGPS.Controls.Add(label_ChooseGpsToEachLoco);

            // sort locos
            List<LocoId> locosSorted = new List<LocoId>(locos);
            locosSorted = locosSorted.OrderBy(x => x.shortId).ToList();

            int i = 0;
            foreach (var loco in locosSorted)
            {
                // create default entry for each loco
                var gpsLocoFilePath = GpsLocoFilePath.Default();
                gpsMapping.Add(loco, gpsLocoFilePath);

                var button = new Button();
                button.Text = $"GPS - {loco.shortId}";
                button.Location = new Point(0, 40 + i * 40);
                button.Size = new Size(300, 40);
                button.ForeColor = Color.Red;
                button.Font = new Font("Segoe UI", 12);
                button.Click += delegate
                {
                    using (OpenFileDialog ofd = new OpenFileDialog())
                    {
                        ofd.Title = $"Vyberte GPS záznam pro lokomotivu {loco.shortId}";
                        ofd.Filter = "XLSX (*.xlsx)|*.xlsx";
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            gpsMapping[loco] = new GpsLocoFilePath(ofd.FileName);
                            button.ForeColor = Color.Green;
                        }
                    }
                };
                panel_LocoGPS.Controls.Add(button);
                i++;
            }
        }

        private void checkBox_NonAbstimmung_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_NonAbstimmung.Checked)
            {
                // unclick the second option
                if (checkBox_Abstimmung.Checked)
                {
                    checkBox_Abstimmung.Checked = false;
                }

                checkBox_Abstimmung_CarrierCalculation.Enabled = false;
                checkBox_Abstimmung_CarrierCalculation.Checked = false;
                checkBox_Abstimmung_CarrierCalculation.ForeColor = Color.DarkGray;
            }
        }

        private void checkBox_Abstimmung_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Abstimmung.Checked)
            {
                // unclick the second option
                if (checkBox_NonAbstimmung.Checked)
                {
                    checkBox_NonAbstimmung.Checked = false;
                }

                checkBox_Abstimmung_CarrierCalculation.Enabled = true;
                checkBox_Abstimmung_CarrierCalculation.ForeColor = Color.Black;
            }
        }

        private void button_FileDialog_InputData_Click(object sender, EventArgs e)
        {
            openFileDialog_InputData.Title = "Vyberte soubor se vstupními daty: [tEns / LastProfile]";
            openFileDialog_InputData.Filter = "CSV (*.csv)|*.csv";

            if (!checkBox_Abstimmung.Checked && !checkBox_NonAbstimmung.Checked)
                return;

            if (openFileDialog_InputData.ShowDialog() == DialogResult.OK)
            {
                inputFilePath = openFileDialog_InputData.FileName;

                if (checkBox_Abstimmung.Checked)
                {
                    DbeWrapper = new DBE_wrapper(inputFilePath);
                    Exporter = new Exporter(inputFilePath).AddOutputDir(outputDir);
                    checkMethod = CheckMethod.InvoiceCheck; // freezing the user-choice
                }
                else
                {
                    DbeWrapper = new DBE_abstimmung_wrapper(inputFilePath);
                    Exporter = new Exporter(inputFilePath).AddOutputDir(outputDir);
                    checkMethod = CheckMethod.PreCheck; // freezing the user-choice
                }

                button_FileDialog_InputData.ForeColor = Color.Green;

                DbeWrapper.GetAllEntriesFromDBE(); // load the file

                AddGPSLokoInputs(DbeWrapper.LocosIncluded);
            }
        }


        private void button_FileDialog_LokoUsage_Click(object sender, EventArgs e)
        {
            openFileDialog_LokoUsage.Title = "Vyberte soubor s LokoUsage pro daný mìsíc:";
            openFileDialog_LokoUsage.Filter = "XLSX (*.xlsx)|*.xlsx";

            if (!checkBox_Abstimmung_CarrierCalculation.Checked)
                return;

            if (openFileDialog_LokoUsage.ShowDialog() == DialogResult.OK)
            {
                lokoUsageFilePath = openFileDialog_LokoUsage.FileName;
                LokoUsageWrapper = new LokoUsage_wrapper(lokoUsageFilePath);
                lokoUsageLoaded = true;

                button_FileDialog_LokoUsage.ForeColor = Color.Green;
            }
        }

        private void button_StartCheck_Click(object sender, EventArgs e)
        {
            if (!CheckInputs())
                return;

            if (!backgroundWorker_Comparer.IsBusy)
            {
                button_StartCheck.Enabled = false;
                backgroundWorker_Comparer.RunWorkerAsync();
            }

        }

        private bool CheckInputs()
        {
            if (string.IsNullOrEmpty(inputFilePath))
            {
                MessageBox.Show("Vstupní soubor nebyl vybrán", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (checkMethod == CheckMethod.InvoiceCheck && checkBox_Abstimmung_CarrierCalculation.Checked && (string.IsNullOrEmpty(textBox_InvoicePrice.Text) || !Double.TryParse(textBox_InvoicePrice.Text, out double res)))
            {
                MessageBox.Show("Špatný formát èísla tarifu faktury", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            return true;
        }

        /// <summary>
        /// Calls the main comparing method which evaluates all results
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker_Comparer_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (checkMethod == CheckMethod.None)
            {
                MessageBox.Show("Vyberte prosím typ porovnání", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (checkMethod == CheckMethod.InvoiceCheck && checkBox_Abstimmung_CarrierCalculation.Checked && lokoUsageLoaded)
            {
                if (!Double.TryParse(textBox_InvoicePrice.Text, out double price))
                {
                    MessageBox.Show("Špatný formát èísla tarifu faktury", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Comparator.MakeCompareWork(gpsMapping, DbeWrapper.Entries, DbeWrapper.LocosIncluded, GpsWrapper, LokoUsageWrapper, Exporter, price, checkBox_Abstimmung_CarrierCalculation.Checked);
            }
            else if (checkMethod == CheckMethod.InvoiceCheck && !checkBox_Abstimmung_CarrierCalculation.Checked)
            {
                Comparator.MakeCompareWork(gpsMapping, DbeWrapper.Entries, DbeWrapper.LocosIncluded, GpsWrapper, Exporter);
            }
            else if (checkMethod == CheckMethod.PreCheck)
            {
                Comparator.MakeCompareWork(gpsMapping, DbeWrapper.Entries, DbeWrapper.LocoIdForGivenColumn, DbeWrapper.LocosIncluded, GpsWrapper, Exporter);
            }
        }

        private void checkBox_Abstimmung_CarrierCalculation_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Abstimmung_CarrierCalculation.Checked)
            {
                label_FileDialog_LokoUsage.Visible = true;
                button_FileDialog_LokoUsage.Visible = true;
            }
            else
            {
                label_FileDialog_LokoUsage.Visible = false;
                button_FileDialog_LokoUsage.Visible = false;
                lokoUsageFilePath = string.Empty; // reset the value
                lokoUsageLoaded = false;
            }

        }

        private void backgroundWorker_Comparer_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            DialogResult result;

            if (e.Error is null)
                result = MessageBox.Show("Porovnání dokonèeno. Po stisknutí OK ukonèíte program.", "Hotovo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            else
               result = MessageBox.Show(e.Error.Message, "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (result == DialogResult.OK)
            {
                this.Close();
                Application.Exit();
            }
        }

        private void button_OpenFileDialog_OutputDir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog_OutputDir.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(folderBrowserDialog_OutputDir.SelectedPath))
            {
                if (Exporter is null)
                {
                    outputDir = folderBrowserDialog_OutputDir.SelectedPath;
                }
                else
                {
                    Exporter.AddOutputDir(folderBrowserDialog_OutputDir.SelectedPath);
                }

                button_OpenFileDialog_OutputDir.ForeColor = Color.Green;
            }

        }
    }
}