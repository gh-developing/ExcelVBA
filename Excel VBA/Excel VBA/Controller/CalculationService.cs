using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Excel_VBA.Model;

namespace Excel_VBA.Controller
{
	public class CalculationService
	{
		private string _directoryPath = "";
		private readonly ConvertTxtToCsvService _convertTxtToCsvService = new();
		private readonly GenerateNewCsvService _generateNewCsvService = new();
		private FileImportService _fileImportComparison1;
		private FileImportService _fileImportComparison2;

		private List<FileCalculationPropertiesDto> _fileProperties = new List<FileCalculationPropertiesDto>();
		private int _checkOnDifference = 0;

		public void CompareFiles()
		{
			StartText();
			GenerateFiles();

			// TODO: set BOM

			// for every line do:
			for (int i = 0; i < 500; i++)
			{
				var file1 = _fileImportComparison1.FileProperties[i];
				var file2 = _fileImportComparison2.FileProperties[i];

				// compare both sides of kalkulationsstruktur
				if (!string.IsNullOrEmpty(file1.Kalkulationsstruktur) &&
					!string.IsNullOrEmpty(file2.Kalkulationsstruktur))
				{
					if (file1.Kalkulationsstruktur ==
						file2.Kalkulationsstruktur)
					{
						if (file1.Material == file2.Material &&
							file1.Preiseinheit == file2.Preiseinheit &&
							file1.Menge == file2.Menge &&
							file1.Ressource == file2.Ressource)
						{
							// see where new line and how many lines are needed
							SetBomAndCosts("ok", CompareCosts(double.Parse(file1.Preis), double.Parse(file2.Preis)));

						}
						else
						{
							SetBomAndCosts("adjusted", "-");
						}

					}
					else
					{
						SetBomAndCosts("adjusted", "-");

						// see where new line and how many lines are needed
					}
				}
				else
				{
					SetBomAndCosts("-", "-");
				}
			}

			_generateNewCsvService.GenerateCsvFile(_directoryPath, _fileProperties);
		}

		/// <summary>
		/// Generates .csv File with User Input
		/// </summary>
		public void GenerateFiles()
		{
			// File 1 and 2 for the Comparisons
			Console.WriteLine("File Path for comparison 1:");
			var txtFilePath1 = Console.ReadLine();

			Console.WriteLine("File Path for comparison 2:");
			var txtFilePath2 = Console.ReadLine();

			Directory.CreateDirectory(Path.GetDirectoryName(txtFilePath1) + "\\GeneratedFiles\\");
			_directoryPath = Path.GetDirectoryName(txtFilePath1) + "\\GeneratedFiles\\";

			try
			{
				_convertTxtToCsvService.ConvertTxtToCsv(txtFilePath1, _directoryPath + "File1.csv", _directoryPath + "FileComparison1.csv");
				_fileImportComparison1 = new FileImportService(_directoryPath + "FileComparison1.csv");

				_convertTxtToCsvService.ConvertTxtToCsv(txtFilePath2, _directoryPath + "File2.csv", _directoryPath + "FileComparison2.csv");
				_fileImportComparison2 = new FileImportService(_directoryPath + "FileComparison2.csv");
			}
			catch (Exception)
			{
				Console.WriteLine("Can't compare empty File");
			}

			// Compare costs number
			Console.WriteLine("Compare on CHf 1 to CHf 10: [1 - 10]");

			while (!int.TryParse(Console.ReadLine(), out _checkOnDifference))
			{
				Console.WriteLine("Compare on CHf 1 to CHf 10: [1 - 10]");
			}
		}

		private void StartText()
		{
			Console.WriteLine(
				"Comparison of 2 cost calculations \n" +
				"Version 1.0.1 - 06. July 2022 \n \n" +
				"Introductions: \n" +
				"1. Run a SAP cost calculation (Tcode CK11N) according to the document AA9393. Consider the layout! Show at least the following columns in the following order: \n" +
				"\t 1.a. calculation structure \n" +
				"\t 1.b. material \n" +
				"\t 1.c. price unit \n" +
				"\t 1.d. me \n" +
				"\t 1.e. price \n" +
				"\t 1.f. calculation size \n" +
				"\t 1.g. quantity \n" +
				"\t 1.h. bmwe \n" +
				"\t 1.i. total value \n" +
				"\t 1.j. currency \n" +
				"\t 1.k. error status \n" +
				"\t 1.l. pricing strategy (text) \n" +
				"\t 1.m. assembly indicator \n" +
				"\t 1.n. cost type (text) \n" +
				"\t 1.o. version \n" +
				"\t 1.p. calculation variant \n" +
				"\t 1.q. cost type \n" +
				"\t 1.r. resource \n" +
				"2. Export two .txt files out of SAP and make sure that they are in the same directory \n" +
				"3. Submit (enter-key) both file paths (with the directory) in the following input field. \n" +
				"\t 3.a. Open Windows Explorer, go to the file and select it and then click in the toolbar on \"Copy path\". \n" +
				"\t 3.b. Ensure when u enter the file paths that you remove the quotation mark. \n" +
				"4. After you submitted the file paths, please enter the price tolerance for the cost check. \n" +
				"In the end you can submit it with the enter key and you will find your Generated files under ur given directory in the folder \"GeneratedFiles\". \n" +
				"***************************** \n" +
				"Press the enter-key to start \n" +
				"*****************************"
			);

			Console.ReadLine();
		}

		private void SetBomAndCosts(string bom, string costs)
		{
			var fileCalc = new FileCalculationPropertiesDto()
			{
				BOM = bom,
				COSTS = costs
			};

			_fileProperties.Add(fileCalc);
		}

		private void SetNewLineInFile(string filePath, int lineBreak)
		{
			string pattern = @"\A(?:.*\n){" + lineBreak + "}";
			string substitution = "$;;;;;;;;;;;;;;;;;;;;;;;\n";
			string input = File.ReadAllText(filePath);
			RegexOptions options = RegexOptions.Multiline;
			Regex regex = new Regex(pattern, options);

			string result = regex.Replace(input, substitution, 1);
			File.WriteAllText(filePath, result);
			_fileProperties[lineBreak].COSTS = "-";
		}

		private string CompareCosts(double price1, double price2)
		{
			string costtxt = "";
			double calculatedPriceDiff = price2 - price1;
			if (calculatedPriceDiff == 0)
			{
				costtxt = "ok";
			}
			else if (calculatedPriceDiff < _checkOnDifference && calculatedPriceDiff > -_checkOnDifference)
			{
				costtxt = "ok";
			}
			else
			{
				if (calculatedPriceDiff > _checkOnDifference)
				{
					costtxt = "not ok - higher " + Convert.ToInt64(calculatedPriceDiff) + " CHF";
				}
				else
				{
					costtxt = "not ok - lower " + Convert.ToInt64(calculatedPriceDiff) + " CHF";
				}
			}

			return costtxt;
		}
	}
}
