using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using LokoTrain_DBE_comparator_forms.Structures; 

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

                    using (var gps_file = new XLWorkbook(locoItem.Value.path))
                    {
                        var worksheet = gps_file.Worksheet(1);
                        int numOfRows = worksheet.RowCount();

                        bool rows_found = false;
                        for (int i = 1; i <= numOfRows; i++)
                        {
                            var cell = worksheet.Cell(i, 1);
                            if (rows_found && cell.IsEmpty())
                                break;

                            if (cell.IsEmpty())
                                continue;

                            if (cell.GetText() == country_of_interest)
                            {
                                rows_found = true;
                                DateTime from = worksheet.Cell(i, 2).GetDateTime();
                                DateTime to = worksheet.Cell(i, 3).GetDateTime();
                                output_dict[locoItem.Key].Add(new DateSpan(from, to));
                            }
                        }
                    }
                }
            }
            return output_dict;
        }
    }
}
