namespace LokoTrain_DBE_comparator_forms.Structures
{
    public record struct SheetIndex
    {
        public int row;
        public int column;

        public SheetIndex(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
    }

    public struct GpsLocoFilePath
    {
        public string path;

        public GpsLocoFilePath(string value)
        {
            path = value;
        }
        

        public static GpsLocoFilePath Default()
        {
            return new GpsLocoFilePath(string.Empty);
        }
    }

    public enum CheckMethod
    {
        None,
        InvoiceCheck,
        PreCheck
    }

    public enum DistributionResult
    {
        NoError,
        IncludingErrors
    }

    public record struct CustomerDateSpan
    {
        public DateSpan dateSpan;
        public string customerName;
        public CustomerDateSpan(DateSpan date_span, string name)
        {
            dateSpan = date_span;
            customerName = name;
        }
        public CustomerDateSpan(DateTime startDate, DateTime endDate, string name)
        {
            dateSpan = new DateSpan(startDate, endDate);
            customerName = name;
        }

        public DateTime startDate { get { return dateSpan.startDate; } }

        public DateTime endDate { get { return dateSpan.endDate; } }
    }

    public record struct DbeEntry // storing date, loco_id and index of each entry from DBE input
    {
        public DateTime date;
        public LocoId id;
        public SheetIndex sheet_index;

        public DbeEntry(DateTime date, LocoId id, SheetIndex sheet_index)
        {
            this.date = date;
            this.id = id;
            this.sheet_index = sheet_index;
        }
    }

    public record struct DateSpan // storing startDate and endDate of given time span
    {
        public DateTime startDate;
        public DateTime endDate;

        public DateSpan(DateTime startDate, DateTime endDate)
        {
            this.startDate = startDate;
            this.endDate = endDate;
        }
    }

    public struct LocoId
    {
        public string shortId;
        public string longId;

        public LocoId(string short_id)
        {
            shortId = short_id;
            longId = "";
        }
        public LocoId(string short_id, string long_id)
        {
            shortId = short_id;
            longId = long_id;
        }
    }
}
