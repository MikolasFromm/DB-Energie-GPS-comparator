using Excel = Microsoft.Office.Interop.Excel;
using LokoTrain_DBE_comparator_forms.Structures;
using Microsoft.Office.Interop.Excel;

namespace LokoTrain_DBE_comparator_forms.Wrappers
{
    public class GPS_wrapper : IGPS_wrapper
    {
        private string country_of_interest = "Německo";
        private HashSet<LocoId> locosWithoutGps = new();

        public HashSet<LocoId> LocomotivesWithOutGPS { get { return locosWithoutGps; } }

        public Dictionary<LocoId, List<DateSpan>> GetAllTimesInGermany(Dictionary<LocoId, GpsLocoFilePath> gpsMapping)
        {
            Dictionary<LocoId, List<DateSpan>> output_dict = new();

            foreach (var locoItem in gpsMapping)
            {
                if (locoItem.Value.path == string.Empty)
                {
                    // add the default entry
                    output_dict.Add(locoItem.Key, new List<DateSpan>());

                    // add the enough long date span
                    output_dict[locoItem.Key].Add(new DateSpan(DateTime.Parse("01.01.1970"), DateTime.Parse("31.12.2100"))); // ultra wide range - all will fall into correct value

                    // add to the list of locos without GPS
                    locosWithoutGps.Add(locoItem.Key);
                }
                else
                {
                    // add the default entry
                    output_dict.Add(locoItem.Key, new List<DateSpan>());

                    // Creating new excel app window
                    Excel.Application app = new Excel.Application();
                    app.Visible = false;

                    Excel.Workbooks workbooks = app.Workbooks;
                    Excel.Workbook workbook = workbooks.Open(locoItem.Value.path, 3, false, 5);
                    Excel.Sheets worksheets = workbook.Worksheets;
                    Excel.Worksheet worksheet = worksheets[1];
                    Excel.Range usedRange = worksheet.UsedRange;
                    Excel.Range rows = usedRange.Rows;
                    Excel.Range row = null;
                    Excel.Range cell = null;
                    int numOfRows = rows.Count;

                    bool rows_found = false;
                    for (int i = 0; i < numOfRows; i++)
                    {
                        row = rows[i + 1];
                        cell = row.Cells[1, 1];
                        if (rows_found && cell.Value == null)
                            break;

                        if (cell.Value == country_of_interest)
                        {
                            rows_found = true;
                            cell = row.Cells[1, 2];
                            DateTime from = DateTime.Parse(cell.Value.ToString());

                            cell = row.Cells[1, 3];
                            DateTime to = DateTime.Parse(cell.Value.ToString());

                            output_dict[locoItem.Key].Add(new DateSpan(from, to));
                        }
                    }

                    // close the app properly

                    workbook.Close(false, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                    workbooks.Close();

                    if (cell is not null)
                    {
                        while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(cell) != 0) { };
                        cell = null;
                    }

                    if (row is not null)
                    {
                        while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(row) != 0) { };
                        row = null;
                    }

                    if (rows is not null)
                    {
                        while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(rows) != 0) { };
                        rows = null;
                    }

                    if (usedRange is not null)
                    {
                        while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(usedRange) != 0) { };
                        usedRange = null;
                    }

                    if (worksheet is not null)
                    {
                        while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(worksheet) != 0) { };
                        worksheet = null;
                    }

                    if (worksheets is not null)
                    {
                        while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(worksheets) != 0) { };
                        worksheets = null;
                    }

                    if (workbook is not null)
                    {
                        while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(workbook) != 0) { };
                        workbook = null;
                    }

                    if (workbooks is not null)
                    {
                        while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(workbooks) != 0) { };
                        workbooks = null;
                    }

                    if (app is not null)
                    {
                        while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(app) != 0) { };
                        app = null;
                    }
                }
            }
            return output_dict;
        }
    }
}
