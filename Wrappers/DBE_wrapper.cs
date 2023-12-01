using System.Globalization;
using LokoTrain_DBE_comparator_forms.Structures;

namespace LokoTrain_DBE_comparator_forms.Wrappers
{
    public class DBE_wrapper : IDBE_wrapper // reading attachment for DBE invoice
    {
        /// <summary>
        /// Column index is defaultly 0 in this DBE_wrapper, because there is only one valid column
        /// Skipping first and then reading all rows until empty row
        /// Storing dbe_entry from each row - specifically date, loco_id and row_index.
        /// </summary>
        private string input_filename;
        private char delimeter = ';';
        private HashSet<LocoId> locos = new HashSet<LocoId>();
        private Dictionary<int, LocoId> locoId_for_given_column = new(); // dictionary which is not used in this class

        public Dictionary<int, LocoId> LocoIdForGivenColumn { get { return locoId_for_given_column; } }
        public List<DbeEntry> Entries { get; set; } = new();

        public HashSet<LocoId> LocosIncluded { get { return locos; } set { locos = value; } }


        public DBE_wrapper(string input_filename)
        {
            this.input_filename = input_filename;
        }

        public void GetAllEntriesFromDBE()
        {
            List<DbeEntry> output_list = new();
            using (StreamReader reader = new StreamReader(input_filename))
            {
                reader.ReadLine(); // skip first row
                string? line = "";
                int row = 1;

                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();

                    if (line == null || line == "")
                        break;

                    string[] parts = line.Split(delimeter);

                    // when reading empty row, break
                    if (parts[1].Length == 0)
                        break;

                    var locoId = Comparator.GetLocoId(parts[1]);
                    if (!locos.Contains(locoId))
                        locos.Add(locoId);
                    output_list.Add(new DbeEntry(DateTime.ParseExact(parts[4].Substring(0, 19), "dd.MM.yyyy HH:mm:ss", new CultureInfo("de-DE")), locoId, new SheetIndex(row, 0)));
                    row++;
                }
            }
            Entries = output_list;
        }
    }

    class DBE_abstimmung_wrapper : IDBE_wrapper // reading CSV export from BahnStromPortal
    {
        /// <summary>
        ///  Reading csv file, which includes all locomotives.
        ///  Picks only those dates, where is non-zero energy consumption or recuperation
        ///  Correct column index is required
        /// </summary>

        private string input_filename;
        private char delimeter = ';';

        private HashSet<string> zero_examples = new HashSet<string> { "0,000", "0" };

        private HashSet<LocoId> locos_included = new();

        private Dictionary<int, LocoId> locoId_for_given_column = new(); // dictionary indexed by first energy_column (second is +2), value is loco_id, full loco_id

        public Dictionary<int, LocoId> LocoIdForGivenColumn { get { return locoId_for_given_column; } }

        public List<DbeEntry> Entries { get; set; } = new();

        public HashSet<LocoId> LocosIncluded { get { return locos_included; } set { locos_included = value; } }

        public DBE_abstimmung_wrapper(string input_filename)
        {
            this.input_filename = input_filename;
            locoId_for_given_column = new Dictionary<int, LocoId>();
        }

        public void GetAllEntriesFromDBE()
        {
            List<DbeEntry> output_list = new(); // Date_of_start, loco_id and row index


            using (StreamReader reader = new StreamReader(input_filename))
            {
                ///<Sheet_summary>
                /// 0-base indexing
                /// DateFrom - Column 0
                /// DateTo  - Column 1
                /// Consumption 2, 6, 10... each column for one locomotive
                /// Recuperation 4, 8, 12... each column for one locomotive
                /// 
                /// 8th row containing loco_id for each column
                /// Next 4 rows are skipped
                /// Content followed then until empty line
                ///</Sheet_summary>


                // skip first 7 rows
                int row = 0;
                string? line = "";
                for (int i = 0; i < 7; i++) { line = reader.ReadLine(); row++; }

                // Get columns for all locomotives
                line = reader.ReadLine(); row++;
                if (line != null)
                {
                    string[] headers = line.Split(delimeter);
                    for (int i = 2; i < headers.Length - 1; i += 4) // jump by 4, each loco occupies 4 columns, starting at 2,  // -1, because CSV is ending by ";", so one empty string is generated
                    {
                        var locoId = Comparator.GetLocoId(headers[i]);
                        locoId_for_given_column.Add(i, locoId);
                    }
                }

                // skip next 4 rows
                for (int i = 0; i < 4; i++) { reader.ReadLine(); row++; }

                // read the content
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();

                    if (line == null || line == "")
                        break;

                    string[] entry_line = line.Split(delimeter);
                    for (int column = 2; column < entry_line.Length - 1; column += 4) // -1, because CSV is ending by ";", one empty string is generated at the end
                    {
                        if (!zero_examples.Contains(entry_line[column]) || !zero_examples.Contains(entry_line[column + 2])) // when consumption or recuperation not zero
                        {
                            DateTime row_date = DateTime.ParseExact(entry_line[0].Substring(0, 16), "yyyy.MM.dd HH:mm", new CultureInfo("de-DE"));
                            var columnLocoId = locoId_for_given_column[column];
                            if (!locos_included.Contains(columnLocoId))
                            {
                                locos_included.Add(columnLocoId);
                            }
                            output_list.Add(new DbeEntry(row_date, columnLocoId, new SheetIndex(row, column)));
                        }
                    }
                    row++;
                }
            }

            Entries = output_list;
        }
    }
}