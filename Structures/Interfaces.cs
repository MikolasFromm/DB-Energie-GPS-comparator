namespace LokoTrain_DBE_comparator_forms.Structures
{
    public interface IGPS_wrapper
    {
        Dictionary<LocoId, List<DateSpan>> GetAllTimesInGermany(Dictionary<LocoId, GpsLocoFilePath> gpsMapping); // indexed by each loco, containing sorted dates "from - to" in germany

        HashSet<LocoId> LocomotivesWithOutGPS { get; }
    }

    public interface ILokoUsage_wrapper
    {
        Dictionary<LocoId, List<CustomerDateSpan>> GetAllCustomers(IEnumerable<LocoId> locomotives); // indexed by locomotive, containing "from - to" and customer name for each time span.

        IList<string> CustomerNames { get; }
    }

    public interface IDBE_wrapper
    {
        void GetAllEntriesFromDBE(); // containing the entry date, loco id, consumption, recuparation, sheet_row, sheet_column index 

        List<DbeEntry> Entries { get; }

        HashSet<LocoId> LocosIncluded { get; }

        public Dictionary<int, LocoId> LocoIdForGivenColumn { get; }

    }

    public interface IExporter
    {
        void ExportAndFillTemplate(EvaluationResults evaluationResults, Dictionary<LocoId, List<CustomerDateSpan>> customerDateTimes, IEnumerable<string> customerNames, double price);

        void ExportAndFillTemplate(EvaluationResults evaluationResults);

        void ExportAndFillTemplate(EvaluationResults evaluation, Dictionary<int, LocoId> LocoIdForGivenColumn);

        void AddOutputDir(string outputDir);
    }
}
