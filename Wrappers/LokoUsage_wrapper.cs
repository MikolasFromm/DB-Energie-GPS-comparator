using ClosedXML.Excel;
using LokoTrain_DBE_comparator_forms.Structures;

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
            Dictionary<LocoId, List<CustomerDateSpan>> output_dictionary = new();

            using (var lokousage_file = new XLWorkbook(filename))
            {
                foreach (var loco in locomotives)
                {
                    output_dictionary.Add(loco, new List<CustomerDateSpan>());
                    var locosheet = lokousage_file.Worksheet(loco.shortId);
                    int row = 7;
                    DateTime last_date = DateTime.Parse("01.01.1970"); //fallback to anyhow initialize this value
                    bool date_loaded = false;
                    while (true)
                    {
                        if (locosheet.Cell(row, 2).IsEmpty())
                            break;

                        if (date_loaded == false)
                        {
                            last_date = locosheet.Cell(row, 2).GetDateTime();
                            date_loaded = true;
                        }

                        // at least one column must be unempty
                        int hours_total = 0;
                        if (locosheet.Cell(row, 3).IsEmpty() == false)
                            hours_total = locosheet.Cell(row, 3).GetValue<int>();
                        else if (locosheet.Cell(row, 4).IsEmpty() == false)
                            hours_total = locosheet.Cell(row, 4).GetValue<int>();
                        else if (locosheet.Cell(row, 5).IsEmpty() == false)
                            hours_total = locosheet.Cell(row, 5).GetValue<int>();
                        else if (locosheet.Cell(row, 6).IsEmpty() == false)
                            hours_total = locosheet.Cell(row, 6).GetValue<int>();
                        else if (locosheet.Cell(row, 7).IsEmpty() == false)
                            hours_total = locosheet.Cell(row, 7).GetValue<int>();
                        else if (locosheet.Cell(row, 8).IsEmpty() == false)
                            hours_total = locosheet.Cell(row, 8).GetValue<int>();

                        string[] customer_parts = locosheet.Cell(row, 11).GetText().Split('/');
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
            }
            return output_dictionary;
        }
    }
}