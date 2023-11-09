using Excel = Microsoft.Office.Interop.Excel;
using LokoTrain_DBE_comparator_forms.Structures;
using System.Globalization;

namespace LokoTrain_DBE_comparator_forms
{
    class Exporter : IExporter
    {
        const string VENT = "DE00758760326VIRT0000000000000236";
        const string NETZSTATUS = "E";

        const int single_file_consumption_column_index = 6;
        const int single_file_recuperation_column_index = 7;
        const int single_file_loco_id_column_index = 1;
        const int single_file_startdate_column_index = 4;
        const int single_file_enddate_column_index = 5;

        const string operator_output_templateDir = "DBE_comparator_templates";
        const string operator_output_templateFileName = "vypis_dopravce_template.xlsx";
        const string operator_output_fallbackFileName = "fallback";
        const string operator_output_errorFileName = "error";
        const string refund_output_templateFileName = "Anlage9_Aufenthaltsstatus_DBEnergie.xlsx";


        private string operator_output_resultDir = "DBE_comparator_results";
        private string dbe_input_filename;
        private double price;
        private char delimiter = ';';

        public Exporter(string filePath)
        {
            this.dbe_input_filename = filePath;
        }

        public IExporter AddOutputDir(string outputDir)
        {
            if (!string.IsNullOrEmpty(outputDir))
                this.operator_output_resultDir = outputDir;

            return this;
        }

        private interface IExcelWrapper
        {
            public void WriteLine(string[] lines, double[] numbers = null);

            public void Close();
        }

        private class ExcelOutputWrapper : IExcelWrapper
        {
            private Excel.Application app = null;
            private Excel.Workbooks workbooks = null;
            private Excel.Workbook workbook = null;
            private Excel.Sheets sheets = null;
            private Excel.Worksheet sheet = null;
            private Excel.Range range = null;
            private Excel.Range cell_1 = null;
            private Excel.Range cell_2 = null;
            private Excel.Range cell_price = null;

            private int nextLineIndex = 2; // 1-based and first row is occupied

            private const int correctDataArrayLen = 8;
            private const int numOfTextCols = 6;
            private const int finalPriceColumn = 14;
            private const int finalPriceRow = 6;

            private bool openned = false;

            public ExcelOutputWrapper(string filename, double price = 0)
            {
                app = new Excel.Application();
                app.Visible = false;
                workbooks = app.Workbooks;
                workbook = workbooks.Open(filename, Type.Missing, false);
                sheets = workbook.Worksheets;
                sheet = sheets[1];
                cell_price = sheet.Cells[finalPriceRow, finalPriceColumn];
                cell_price.Value = price;
                openned = true;
            }

            public void WriteLine(string[] lines, double[] numbers)
            {
                if (!openned)
                    return;

                if (lines.Length + numbers.Length != correctDataArrayLen)
                    return;

                cell_1 = sheet.Cells[nextLineIndex, 1];
                cell_2 = sheet.Cells[nextLineIndex, numOfTextCols];
                range = sheet.Range[cell_1, cell_2];

                range.Value = lines;

                cell_1 = sheet.Cells[nextLineIndex, numOfTextCols + 1];
                cell_2 = sheet.Cells[nextLineIndex, numOfTextCols + 2];
                range = sheet.Range[cell_1, cell_2];
                range.Value = numbers;
                nextLineIndex++;
            }

            public void Close()
            {
                workbook.Save();
                workbook.Close(false, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                workbooks.Close();

                if (cell_price is not null)
                {
                    while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(cell_price) != 0) { };
                    cell_price = null;
                }

                if (cell_1 is not null)
                {
                    while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(cell_1) != 0) { };
                    cell_1 = null;
                }

                if (cell_2 is not null)
                {
                    while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(cell_2) != 0) { };
                    cell_2 = null;
                }

                if (range is not null)
                {
                    while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(range) != 0) { };
                    range = null;
                }

                if (sheet is not null)
                {
                    while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(sheet) != 0) { };
                    sheet = null;
                }

                if (sheets is not null)
                {
                    while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(sheets) != 0) { };
                    sheets = null;
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

                app.Quit();

                if (app is not null)
                {
                    while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(app) != 0) { };
                    app = null;
                }

                openned = false;
            }
        }

        private class ExcelRefundWrapper : IExcelWrapper
        {
            private Excel.Application app = null;
            private Excel.Workbooks workbooks = null;
            private Excel.Workbook workbook = null;
            private Excel.Sheets sheets = null;
            private Excel.Worksheet sheet = null;
            private Excel.Range range = null;
            private Excel.Range cell_1 = null;
            private Excel.Range cell_2 = null;

            private int nextLineIndex = 11;
            private const int correctDataArrayLen = 5;

            private bool openned = false;

            public ExcelRefundWrapper(string filename)
            {
                app = new Excel.Application();
                app.Visible = false;
                workbooks = app.Workbooks;
                workbook = workbooks.Open(filename, Type.Missing, false);
                sheets = workbook.Worksheets;
                sheet = sheets[1];
                openned = true;
            }

            public void WriteLine(string[] lines, double[] numbers = null)
            {
                if (!openned)
                    return;

                if (lines.Length != correctDataArrayLen)
                    return;

                cell_1 = sheet.Cells[nextLineIndex, 1];
                cell_2 = sheet.Cells[nextLineIndex, correctDataArrayLen];
                range = sheet.Range[cell_1, cell_2];
                range.Value = lines;
                nextLineIndex++;
            }

            public void Close()
            {
                workbook.Save();
                workbook.Close(false, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                workbooks.Close();

                if (cell_1 is not null)
                {
                    while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(cell_1) != 0) { };
                    cell_1 = null;
                }

                if (cell_2 is not null)
                {
                    while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(cell_2) != 0) { };
                    cell_2 = null;
                }

                if (range is not null)
                {
                    while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(range) != 0) { };
                    range = null;
                }

                if (sheet is not null)
                {
                    while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(sheet) != 0) { };
                    sheet = null;
                }

                if (sheets is not null)
                {
                    while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(sheets) != 0) { };
                    sheets = null;
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

                app.Quit();

                if (app is not null)
                {
                    while (System.Runtime.InteropServices.Marshal.FinalReleaseComObject(app) != 0) { };
                    app = null;
                }

                openned = false;
            }
        }

        public void ExportAndFillTemplate(EvaluationResults evaluationResults, Dictionary<LocoId, List<CustomerDateSpan>> customerDateTimes, IEnumerable<string> customerNames, double price)
        {
            bool useExportPaths = false;

            string projectDirectory = null;
            string exportProjectDirectory = null;
            try
            {
                projectDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
                exportProjectDirectory = Environment.CurrentDirectory;
            }
            catch 
            {
                throw new Exception("Could not determine current project directory");    
            }
            
            string templatePath = Path.Combine(projectDirectory, operator_output_templateDir, operator_output_templateFileName);
            string exportTemplatePath = Path.Combine(exportProjectDirectory, operator_output_templateDir, operator_output_templateFileName);

            string templateRefundPath = Path.Combine(projectDirectory, operator_output_templateDir, refund_output_templateFileName);
            string exportTemplateRefundPath = Path.Combine(exportProjectDirectory, operator_output_templateDir, refund_output_templateFileName);

            string outputDirPath = Path.Combine(projectDirectory, operator_output_resultDir);
            string exportOutputDirPath = Path.Combine(exportProjectDirectory, operator_output_resultDir);

            if (!Directory.Exists(outputDirPath) && !Directory.Exists(exportOutputDirPath))
                Directory.CreateDirectory(exportOutputDirPath);

            if (!File.Exists(templatePath))
            {
                if (!File.Exists(exportTemplatePath))
                {
                    throw new Exception("Template file not found");
                }
                else
                {
                    useExportPaths = true;
                }
            }
                

            if (!File.Exists(templateRefundPath))
            {
                if (!File.Exists(exportTemplateRefundPath))
                {
                    throw new Exception("Template refund file not found");
                }
                else
                {
                    useExportPaths = true;
                }
            }

            if (useExportPaths)
            {
                templatePath = exportTemplatePath;
                templateRefundPath = exportTemplateRefundPath;
                outputDirPath = exportOutputDirPath;
            }

            Dictionary<string, IExcelWrapper> customerSheets = new();
            HashSet<string> untouchedCustomers = new();


            // create new file for each operator from template and open the workbook
            foreach (var customer in customerNames)
            {
                string outputFilePath = Path.Combine(outputDirPath, $"DBE-výpis_{customer}.xlsx");
                if (File.Exists(outputFilePath))
                    File.Delete(outputFilePath);

                File.Copy(templatePath, outputFilePath);
                IExcelWrapper excelWrapper = new ExcelOutputWrapper(outputFilePath, price);
                customerSheets.TryAdd(customer, excelWrapper);
                untouchedCustomers.Add(customer);
            }

            // create fallback file
            string outputFallbackFilePath = Path.Combine(outputDirPath, $"DBE-výpis_{operator_output_fallbackFileName}.xlsx");
            if (File.Exists(outputFallbackFilePath))
                File.Delete(outputFallbackFilePath);
            File.Copy(templatePath, outputFallbackFilePath);
            customerSheets.TryAdd(operator_output_fallbackFileName, new ExcelOutputWrapper(outputFallbackFilePath));
            untouchedCustomers.Add(operator_output_fallbackFileName);

            // create error file
            string outputErrorFilePath = Path.Combine(outputDirPath, $"DBE-{operator_output_errorFileName}.xlsx");
            if (File.Exists(outputErrorFilePath))
                File.Delete(outputErrorFilePath);
            File.Copy(templateRefundPath, outputErrorFilePath);
            customerSheets.TryAdd(operator_output_errorFileName, new ExcelRefundWrapper(outputErrorFilePath));

            // go through all correct values
            foreach (var loco in evaluationResults.correct_values)
            {
                StreamReader reader = new StreamReader(dbe_input_filename);
                reader.ReadLine(); // skip the first row
                int row = 1;
                string? line = null;

                int currentLocoDateSpanIndex = 0; // for sequential reading of the locoDateSpans

                foreach (var entry in loco.Value)
                {
                    while (entry.row >= row) { line = reader.ReadLine(); row++; }
                    if (line is null) { break; }

                    string[] lineResultRaw = line.Split(delimiter);

                    string[] lineResultString = lineResultRaw.Take(6).ToArray();
                    double[] lineResultDouble = new double[] { Double.Parse(lineResultRaw[single_file_consumption_column_index], CultureInfo.CurrentCulture), Double.Parse(lineResultRaw[single_file_recuperation_column_index], CultureInfo.CurrentCulture) };

                    DateTime startDate = DateTime.Parse(lineResultRaw[single_file_startdate_column_index]);
                    DateTime endDate = DateTime.Parse(lineResultRaw[single_file_enddate_column_index]);

                    while (!(customerDateTimes[loco.Key][currentLocoDateSpanIndex].dateSpan.startDate <= startDate
                        && customerDateTimes[loco.Key][currentLocoDateSpanIndex].dateSpan.endDate >= endDate)) // while the entry not in the next operator dateSpan (probably error)
                    {
                        if (customerDateTimes[loco.Key][currentLocoDateSpanIndex].dateSpan.endDate <= startDate) // when already behind the currently read datetime
                            currentLocoDateSpanIndex++;

                        if (customerDateTimes[loco.Key][currentLocoDateSpanIndex].dateSpan.startDate >= endDate)
                            customerSheets[operator_output_fallbackFileName].WriteLine(lineResultString, lineResultDouble); // when no operator assigned to the correct value
                    }

                    // already in the dateSpan
                    customerSheets[customerDateTimes[loco.Key][currentLocoDateSpanIndex].customerName].WriteLine(lineResultString, lineResultDouble);
                    untouchedCustomers.Remove(customerDateTimes[loco.Key][currentLocoDateSpanIndex].customerName);
                }

                reader.Close();
            }

            // go through all error values
            {
                StreamReader reader = new StreamReader(dbe_input_filename);
                reader.ReadLine(); // skip the first row
                int row = 1;
                string? line = null;

                Dictionary<LocoId, DateSpan?> error_occupations = new(); // stores first and (currently) last date of an error entry for each loco

                foreach (var errorValue in evaluationResults.error_values)
                {
                    while (errorValue.row >= row) { line = reader.ReadLine(); row++; }
                    if (line is null) { break; }

                    string[] lineResultRaw = line.Split(delimiter);

                    string[] lineResultString = lineResultRaw.Take(6).ToArray();
                    double[] lineResultDouble = new double[] { Double.Parse(lineResultRaw[single_file_consumption_column_index], CultureInfo.CurrentCulture), Double.Parse(lineResultRaw[single_file_recuperation_column_index], CultureInfo.CurrentCulture) };

                    LocoId locoId = Comparator.GetLocoId(lineResultRaw[single_file_loco_id_column_index]);
                    DateTime startDate = DateTime.Parse(lineResultRaw[single_file_startdate_column_index]);
                    DateTime endDate = DateTime.Parse(lineResultRaw[single_file_enddate_column_index]);

                    if (error_occupations.ContainsKey(locoId))
                    {
                        if (Comparator.DatesInGivenTimeSpan(error_occupations[locoId].Value.endDate, startDate, 15))
                        {
                            error_occupations[locoId] = new DateSpan(error_occupations[locoId].Value.startDate, endDate); // extend the timeSpan
                        }
                        else
                        {
                            customerSheets[operator_output_errorFileName].WriteLine(new string[] {
                                locoId.longId,
                                VENT,
                                $"{error_occupations[locoId].Value.startDate.ToString("dd.MM.yyyy HH:mm")}",
                                $"{error_occupations[locoId].Value.endDate.ToString("dd.MM.yyyy HH:mm")}",
                                NETZSTATUS
                            });

                            error_occupations[locoId] = new DateSpan(startDate, endDate); // refresh the temp timespan
                        }
                    }
                    else
                    {
                        error_occupations.Add(locoId, new DateSpan(startDate, endDate));
                    }

                    customerSheets[operator_output_errorFileName].WriteLine(lineResultString, lineResultDouble);
                }

                reader.Close();

                // Write the error occupations left
                foreach (var occupation in error_occupations)
                {
                    if (occupation.Value != null)
                    {
                        customerSheets[operator_output_errorFileName].WriteLine(new string[] {
                                occupation.Key.longId,
                                VENT,
                                $"{error_occupations[occupation.Key].Value.startDate.ToString("dd.MM.yyyy HH:mm")}",
                                $"{error_occupations[occupation.Key].Value.endDate.ToString("dd.MM.yyyy HH:mm")}",
                                NETZSTATUS
                            });
                    }
                }

                foreach (var excel in customerSheets)
                {
                    excel.Value.Close();
                }


                foreach (var untouchedCustomer in untouchedCustomers)
                {
                    string outputFilePath = Path.Combine(outputDirPath, $"DBE-výpis_{untouchedCustomer}.xlsx");
                    if (File.Exists(outputFilePath))
                        File.Delete(outputFilePath);
                }
            }
        }

        public void ExportAndFillTemplate(EvaluationResults evaluationResults)
        {
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            string outputDirPath = Path.Combine(projectDirectory, operator_output_resultDir);
            string templateRefundPath = Path.Combine(projectDirectory, operator_output_templateDir, refund_output_templateFileName);

            // create error file
            string outputErrorFilePath = Path.Combine(outputDirPath, $"DBE-{operator_output_errorFileName}.xlsx");
            if (File.Exists(outputErrorFilePath))
                File.Delete(outputErrorFilePath);
            File.Copy(templateRefundPath, outputErrorFilePath);
            IExcelWrapper excelWrapper = new ExcelRefundWrapper(outputErrorFilePath);

            // go through all error values
            {
                StreamReader reader = new StreamReader(dbe_input_filename);
                reader.ReadLine(); // skip the first row
                int row = 1;
                string? line = null;

                Dictionary<LocoId, DateSpan?> error_occupations = new(); // stores first and (currently) last date of an error entry for each loco

                foreach (var errorValue in evaluationResults.error_values)
                {
                    while (errorValue.row >= row) { line = reader.ReadLine(); row++; }
                    if (line is null) { break; }

                    string[] lineResultRaw = line.Split(delimiter);

                    string[] lineResultString = lineResultRaw.Take(6).ToArray();
                    double[] lineResultDouble = new double[] { Double.Parse(lineResultRaw[single_file_consumption_column_index], CultureInfo.CurrentCulture), Double.Parse(lineResultRaw[single_file_recuperation_column_index], CultureInfo.CurrentCulture) };

                    LocoId locoId = Comparator.GetLocoId(lineResultRaw[single_file_loco_id_column_index]);
                    DateTime startDate = DateTime.Parse(lineResultRaw[single_file_startdate_column_index]);
                    DateTime endDate = DateTime.Parse(lineResultRaw[single_file_enddate_column_index]);

                    if (error_occupations.ContainsKey(locoId))
                    {
                        if (Comparator.DatesInGivenTimeSpan(error_occupations[locoId].Value.endDate, startDate, 15))
                        {
                            error_occupations[locoId] = new DateSpan(error_occupations[locoId].Value.startDate, endDate); // extend the timeSpan
                        }
                        else
                        {
                            excelWrapper.WriteLine(new string[] {
                                locoId.longId,
                                VENT,
                                $"{error_occupations[locoId].Value.startDate.ToString("dd.MM.yyyy HH:mm")}",
                                $"{error_occupations[locoId].Value.endDate.ToString("dd.MM.yyyy HH:mm")}",
                                NETZSTATUS
                            });

                            error_occupations[locoId] = new DateSpan(startDate, endDate); // refresh the temp timespan
                        }
                    }
                    else
                    {
                        error_occupations.Add(locoId, new DateSpan(startDate, endDate));
                    }

                    excelWrapper.WriteLine(lineResultString, lineResultDouble);
                }

                reader.Close();

                // Write the error occupations left
                foreach (var occupation in error_occupations)
                {
                    if (occupation.Value != null)
                    {
                        excelWrapper.WriteLine(new string[] {
                                occupation.Key.longId,
                                VENT,
                                $"{error_occupations[occupation.Key].Value.startDate.ToString("dd.MM.yyyy HH:mm")}",
                                $"{error_occupations[occupation.Key].Value.endDate.ToString("dd.MM.yyyy HH:mm")}",
                                NETZSTATUS
                            });
                    }
                }
            }
            excelWrapper.Close();
        }

        public void ExportAndFillTemplate(EvaluationResults evaluated_results, Dictionary<int, LocoId> locoIdForGivenColumn)
        {
            ///<summary>
            /// evaluated_entires are sorted by row, therefore can use ReadLine(), becuase there is never a need to jump back.
            /// Goes through whole evaluated entires (with non-zero values) and export them to given file or output.
            ///</summary>

            const int firstDataRowIndex = 12;

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            string outputDirPath = Path.Combine(projectDirectory, operator_output_resultDir);
            string templateRefundPath = Path.Combine(projectDirectory, operator_output_templateDir, refund_output_templateFileName);

            // create error file
            string outputErrorFilePath = Path.Combine(outputDirPath, $"DBE-{operator_output_errorFileName}.xlsx");
            if (File.Exists(outputErrorFilePath))
                File.Delete(outputErrorFilePath);
            File.Copy(templateRefundPath, outputErrorFilePath);
            IExcelWrapper excelWrapper = new ExcelRefundWrapper(outputErrorFilePath);

            // false entries handle
            {
                double error_consumption = 0;
                double error_recuperation = 0;

                Dictionary<string, DateSpan?> error_occupations = new(); // stores first and (currently) last date of an error entry for each loco

                StreamReader reader = new StreamReader(this.dbe_input_filename);

                int row = 0;
                for (int i = 0; i < firstDataRowIndex; i++) { reader.ReadLine(); row++; }
                string? line = null;

                foreach (var entry in evaluated_results.error_values)
                {
                    int entry_row = entry.row;
                    int entry_column = entry.column;
                    string loco_id = locoIdForGivenColumn[entry_column].longId;

                    while (entry_row >= row) { line = reader.ReadLine(); row++; } // multiple entries can be in one line. Newline read is only when entry_row >= row (must be = for loading the current line)
                    if (line is null)
                        break;

                    string[] results = line.Split(delimiter);
                    error_consumption += Double.Parse(results[entry_column], CultureInfo.CurrentCulture);
                    error_recuperation += Double.Parse(results[entry_column + 2], CultureInfo.CurrentCulture);

                    DateTime startDate = DateTime.Parse(results[0]);
                    DateTime endDate = DateTime.Parse(results[1]);  

                    if (error_occupations.ContainsKey(loco_id))
                    {
                        if (Comparator.DatesInGivenTimeSpan(error_occupations[loco_id].Value.endDate, startDate, 15)) // endDate and startDates are identical, when in sequence
                        {
                            error_occupations[loco_id] = new DateSpan(error_occupations[loco_id].Value.startDate, endDate);
                        }
                        else
                        {
                            excelWrapper.WriteLine(new string[] {
                                locoIdForGivenColumn[entry_column].longId,
                                VENT,
                                $"{error_occupations[locoIdForGivenColumn[entry_column].longId].Value.startDate.ToString("dd.MM.yyyy HH:mm")}",
                                $"{error_occupations[locoIdForGivenColumn[entry_column].longId].Value.endDate.ToString("dd.MM.yyyy HH:mm")}",
                                NETZSTATUS
                            });
                            error_occupations[locoIdForGivenColumn[entry_column].longId] = new DateSpan(startDate, endDate);
                        }
                    }
                    else
                    {
                        error_occupations.Add(locoIdForGivenColumn[entry_column].longId, new DateSpan(startDate, endDate));
                    }
                }

                foreach (var occuupation in error_occupations)
                {
                    excelWrapper.WriteLine(new string[] {
                                occuupation.Key,
                                VENT,
                                $"{occuupation.Value.Value.startDate.ToString("dd.MM.yyyy HH:mm")}",
                                $"{occuupation.Value.Value.endDate.ToString("dd.MM.yyyy HH:mm")}",
                                NETZSTATUS
                            });
                }

                reader.Close();
            }
            excelWrapper.Close();
        }
    }
}