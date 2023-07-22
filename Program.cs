namespace LokoTrain_DBE_comparator_forms
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ////// FORMS
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}