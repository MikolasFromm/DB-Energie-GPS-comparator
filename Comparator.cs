using LokoTrain_DBE_comparator_forms.Structures;
using LokoTrain_DBE_comparator_forms.Wrappers;


namespace LokoTrain_DBE_comparator_forms
{
    public static class Comparator
    {
        private const int data_entry_time_span = 15; // given in minutes

        public static LocoId GetLocoId(string locomotive) // DE0075876032691547383064300000000 -> 383064 915473830643
        {
            return new LocoId(locomotive.Substring(18, 6), locomotive.Substring(13, 12));
        }

        public static bool DatesInGivenTimeSpan(DateTime startDate, DateTime endDate, int time_span)
        {
            TimeSpan diff = endDate - startDate;
            if (diff.TotalMinutes < time_span)
                return true;
            else
                return false;
        }

        public static EvaluationResults EvaluateResults(IEnumerable<LocoId> locomotives_in_germany, IEnumerable<DbeEntry> dbe_entries, Dictionary<LocoId, List<DateSpan>> real_loco_dates_in_germany)
        {
            EvaluationResults evaluated_results = new();

            // generating dictionary in advance - dont need to check if key exists
            foreach (LocoId loco in locomotives_in_germany)
            {
                evaluated_results.correct_values.Add(loco, new List<SheetIndex>());
            }

            // iteration over whole DBE file
            foreach (DbeEntry entry in dbe_entries)
            {
                // expecting that dbe input is time-sorted!!
                DateTime entry_date = entry.date;
                LocoId loco_id = entry.id;
                SheetIndex sht_index = entry.sheet_index;

                DateTime loco_time_in;
                DateTime loco_time_out;

                if (!real_loco_dates_in_germany.ContainsKey(loco_id) ||
                    real_loco_dates_in_germany[loco_id].Count == 0 ||
                    (entry_date < real_loco_dates_in_germany[loco_id][0].startDate && !DatesInGivenTimeSpan(entry_date, real_loco_dates_in_germany[loco_id][0].startDate, data_entry_time_span)))
                    // when no other time in DE left for the locomotive or the entry date is earlier than the loco time in DE
                    // timespan of data_entry is 15mins, therefore checking, if endtime of the entry is earlier than the real time in DE
                { 
                    evaluated_results.error_values.Add(sht_index);
                }
                else
                {
                    loco_time_in = real_loco_dates_in_germany[loco_id][0].startDate;
                    loco_time_out = real_loco_dates_in_germany[loco_id][0].endDate;
                    while (entry_date > loco_time_out) // while the entry_date is later than the exit from DE, skip to next DE occurence
                    {
                        real_loco_dates_in_germany[loco_id].RemoveAt(0); // time sorted, therefore at0 is the earliest date
                        if (real_loco_dates_in_germany[loco_id].Count == 0) // when removed last piece
                            break;
                        // update data
                        loco_time_in = real_loco_dates_in_germany[loco_id][0].startDate;
                        loco_time_out = real_loco_dates_in_germany[loco_id][0].endDate;
                    }
                    if (real_loco_dates_in_germany[loco_id].Count != 0)
                    {
                        if (entry_date >= loco_time_in || 
                            (entry_date < loco_time_in) && DatesInGivenTimeSpan(entry_date, loco_time_in, data_entry_time_span))
                        {
                            evaluated_results.correct_values[loco_id].Add((sht_index));
                        }
                        else
                        {
                            evaluated_results.error_values.Add(sht_index);
                        }
                    }
                    else
                    {
                        evaluated_results.error_values.Add(sht_index);
                    }
                }
            }
            return evaluated_results;
        }

        public static void MakeCompareWork(Dictionary<LocoId, GpsLocoFilePath> gpsMapping, IEnumerable<DbeEntry> dbeEntries, HashSet<LocoId> locosInGermany, IGPS_wrapper gpsWrapper, ILokoUsage_wrapper lokoUsageWrapper, IExporter exporter, double price = 0, bool splitCustomers = false)
        {
            // GPS
            Dictionary<LocoId, List<DateSpan>> real_loco_dates_in_germany = gpsWrapper.GetAllTimesInGermany(gpsMapping);

            // LokoUsage (optimal)
            Dictionary<LocoId, List<CustomerDateSpan>>? customersForGivenDates = null;

            if (splitCustomers)
                customersForGivenDates = lokoUsageWrapper.GetAllCustomers(locosInGermany);

            // Evaluate
            EvaluationResults output_evaluation = EvaluateResults(locosInGermany, dbeEntries, real_loco_dates_in_germany);

            // Export 
            exporter.ExportAndFillTemplate(output_evaluation, customersForGivenDates, lokoUsageWrapper.CustomerNames, price);

            // Send Information
            return;
        }

        public static void MakeCompareWork(Dictionary<LocoId, GpsLocoFilePath> gpsMapping, IEnumerable<DbeEntry> dbeEntries, HashSet<LocoId> locosInGermany, IGPS_wrapper gpsWrapper, IExporter exporter)
        {
            // GPS
            Dictionary<LocoId, List<DateSpan>> real_loco_dates_in_germany = gpsWrapper.GetAllTimesInGermany(gpsMapping);

            // Evaluate
            EvaluationResults output_evaluation = EvaluateResults(locosInGermany, dbeEntries, real_loco_dates_in_germany);

            // Export 
            exporter.ExportAndFillTemplate(output_evaluation);

            // Send Information
            return;
        }

        public static void MakeCompareWork(Dictionary<LocoId, GpsLocoFilePath> gpsMapping, IEnumerable<DbeEntry> dbeEntries, Dictionary<int, LocoId> locoIdForGivenColumn, HashSet<LocoId> locosInGermany, IGPS_wrapper gpsWrapper, IExporter exporter)
        {
            // GPS
            Dictionary<LocoId, List<DateSpan>> real_loco_dates_in_germany = gpsWrapper.GetAllTimesInGermany(gpsMapping);

            // Evaluate
            EvaluationResults output_evaluation = EvaluateResults(locosInGermany, dbeEntries, real_loco_dates_in_germany);

            // Export 
            exporter.ExportAndFillTemplate(output_evaluation, locoIdForGivenColumn);

            // Send Information
            return;
        }
    }
}
