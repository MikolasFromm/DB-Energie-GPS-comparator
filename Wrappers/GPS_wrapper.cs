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
                    Excel.Application oXL = new Excel.Application();
                    oXL.Visible = false;
                    Excel.Workbook oWB = oXL.Workbooks.Open(locoItem.Value.path, 3, false, 5);
                    Excel.Worksheet oSheet = oWB.Worksheets[1];
                    Excel.Range usedRange = oSheet.UsedRange;

                    bool rows_found = false;
                    foreach (Excel.Range row in usedRange.Rows)
                    {
                        if (rows_found && row.Cells[1, 1].Value == null)
                            break;
                        if (row.Cells[1, 1].Value == country_of_interest)
                        {
                            rows_found = true;
                            output_dict[locoItem.Key].Add(new DateSpan(DateTime.Parse(row.Cells[1, 2].Value.ToString()), DateTime.Parse(row.Cells[1, 3].Value.ToString())));
                        }

                    }

                    // close the app properly

                    oWB.Close(true);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oSheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oWB);

                    oXL.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oXL);
                }
            }
            return output_dict;
        }
    }
}
