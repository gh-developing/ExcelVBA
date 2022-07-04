
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Excel_VBA.Model;
using Excel = Microsoft.Office.Interop.Excel;

namespace Excel_VBA.Controller
{
	public class GenerateNewCsvService
	{
		public void GenerateCsvFile(string filePath, List<FileCalculationPropertiesDto> calculationResults)
		{
			CalculationsToCsv(calculationResults, filePath);

			var calc = File.ReadAllLines(filePath + "Calc.csv");
			var file1 = File.ReadAllLines(filePath + "File1.csv");
			var file2 = File.ReadAllLines(filePath + "File2.csv");

			var merge = calc.Zip(file1, (c, f) => string.Join(";", c, f));
			var result = merge.Zip(file2, (m, s) => string.Join(";;;", m, s));

			File.WriteAllLines($@"{filePath + "GeneratedCalculation.csv"}", result);
		}

		private static void CalculationsToCsv(List<FileCalculationPropertiesDto> calculationResults, string filePath)
		{
			var csv = new StringBuilder();
			csv.Append("\n\n\n\n\n");
			csv.AppendLine($"BOM;COSTS;");
			foreach (var calculationResult in calculationResults)
			{
				csv.AppendLine($"{calculationResult.BOM};{calculationResult.COSTS};");
			}
			File.WriteAllText(filePath + "Calc.csv", csv.ToString());
		}
	}
}
