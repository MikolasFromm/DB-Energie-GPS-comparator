namespace LokoTrain_DBE_comparator_forms.Structures
{
    public class EvaluationResults
    {
        public Dictionary<LocoId, List<SheetIndex>> correct_values;
        public List<SheetIndex> error_values;

        public EvaluationResults()
        {
            correct_values = new();
            error_values = new();
        }
    }

    public class ExportResults
    {
        public double ConsumptionLeft;
        public double RecuperationLeft;

    }
}
