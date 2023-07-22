using Excel = Microsoft.Office.Interop.Excel;
using LokoTrain_DBE_comparator_forms.Structures;
using Microsoft.Office.Interop.Excel;

///<sumary>
/// Wrapper reading all sheets from lokousage excel file. 
/// Determines for each locomotive all their datespans for all customers
///</sumary>
namespace LokoTrain_DBE_comparator_forms.Wrappers
{
    public class LokoUsage_wrapper : ILokoUsage_wrapper
    {

        private string filename;

        private HashSet<string> customers = new();

        public IList<string> CustomerNames { get { return customers.ToList(); } }

        public LokoUsage_wrapper(string filename)
        {
            this.filename = filename;
        }

        public Dictionary<LocoId, List<CustomerDateSpan>> GetAllCustomers(IEnumerable<LocoId> locomotives)
        {
            Excel.Application oXL = new Excel.Application();
            oXL.Visible = false;
            Excel.Workbook oWB = oXL.Workbooks.Open(filename, 3, false, 5);

            Dictionary<LocoId, List<CustomerDateSpan>> output_dictionary = new();

            foreach (LocoId loco in locomotives)
            {
                output_dictionary.Add(loco, new List<CustomerDateSpan>());
                Excel.Worksheet oSheet = oWB.Worksheets[loco.shortId];
                int row = 7;
                DateTime last_date = DateTime.Parse("01.01.1970"); //fallback to anyhow initialize this value
                bool date_loaded = false;
                while (true)
                {
                    if (oSheet.Cells[row, 2].Value == null)
                        break;

                    string date = oSheet.Cells[row, 2].Value.ToString();
                    if (!date_loaded)
                    {
                        last_date = DateTime.Parse(date);
                        date_loaded = true;
                    }
                    // at least one column must be unempty
                    int hours_total = 0;
                    if (oSheet.Cells[row, 3].Value != null)
                        hours_total = (int)oSheet.Cells[row, 3].Value;
                    else if (oSheet.Cells[row, 4].Value != null)
                        hours_total = (int)oSheet.Cells[row, 4].Value;
                    else if (oSheet.Cells[row, 5].Value != null)
                        hours_total = (int)oSheet.Cells[row, 5].Value;
                    else if (oSheet.Cells[row, 6].Value != null)
                        hours_total = (int)oSheet.Cells[row, 6].Value;
                    else if (oSheet.Cells[row, 7].Value != null)
                        hours_total = (int)oSheet.Cells[row, 7].Value;
                    else if (oSheet.Cells[row, 8].Value != null)
                        hours_total = (int)oSheet.Cells[row, 8].Value;

                    string[] customer_parts = oSheet.Cells[row, 11].Value.ToString().Split('/');
                    string customer;
                    if (customer_parts.Length > 1) // when "ČDC / N30 + oprava" -> "ČDC " -> "ČDC"
                        customer = customer_parts[0].Substring(0, customer_parts[0].Length - 1);
                    else
                        customer = customer_parts[0];

                    customers.Add(customer);

                    DateTime new_date = last_date.AddHours(hours_total);

                    output_dictionary[loco].Add(new CustomerDateSpan(last_date, new_date, customer));
                    last_date = new_date;
                    row++;
                }
            }

            // close the file properly

            oWB.Close(false);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oWB);

            oXL.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oXL);

            return output_dictionary;
        }
    }
}