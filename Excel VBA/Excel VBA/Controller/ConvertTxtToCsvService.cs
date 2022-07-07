using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Excel_VBA.Controller
{
	public class ConvertTxtToCsvService
	{
		private const RegexOptions Options = RegexOptions.Multiline;

		private readonly Regex _regexBeforeSemicolon = new(@"^[^A-Za-z0-9]*;", Options);
		private readonly Regex _regexAfterSemicolon = new(@"^(.*?)\s*;+\s*", Options);
		private readonly Regex _regexMaterial = new(@"^(.*?)\s*[0-9]{7,};\s*", Options);
		private readonly Regex _regexPriceUnit = new(@"^(.*?);\s*[0-9]{1}\s*", Options);
		private readonly Regex _regexCalcStructureLength = new(@"^(.*?)\s*;", Options);

		private readonly Regex _regexCsvFile = new(@"^[^A-Za-z0-9]{1,2};", Options);

		/// <summary>
		/// Converts .txt file into .csv file
		/// </summary>
		/// <param name="csvPath"></param>
		/// <param name="txtPath"></param>
		public void ConvertTxtToCsv(string txtPath, string csvPath, string destPath)
		{
			var csvFilePath = $@"{csvPath}";
			var txtLines = File.ReadAllLines($@"{txtPath}");

			var result = string.Join(Environment.NewLine,
				txtLines.Select(x => x.Split('\t'))
					.Select(x => string.Join(";", x)));


			result = _regexCsvFile.Replace(result, "");
			File.WriteAllText(csvFilePath, result);

			DuplicateCsvFile(csvPath, destPath);
		}

		/// <summary>
		/// Duplicates a .csv File
		/// </summary>
		/// <param name="csvPath"></param>
		/// <param name="destPath"></param>
		public void DuplicateCsvFile(string csvPath, string destPath)
		{
			try
			{
				File.Copy($@"{csvPath}", $@"{destPath}", true);
				RegexCsv($@"{destPath}");
			}
			catch (IOException iox)
			{
				Console.WriteLine(iox.Message);
			}
		}

		/// <summary>
		/// Splits a .csv File with Regex
		/// </summary>
		/// <param name="csvPath"></param>
		public void RegexCsv(string csvPath)
		{
			var fileLines = File.ReadAllLines(csvPath).Skip(6);
			var csv = new StringBuilder();

			foreach (var fileLine in fileLines)
			{
				// Deletes semicolons [;] before Calc. structure
				var resultDelFrontSem = _regexBeforeSemicolon.Replace(fileLine, "");
				// Deletes semicolons [;] after Calc. structure and adds a semicolon
				var resultDelBackSem = _regexAfterSemicolon.Replace(resultDelFrontSem, "$1;");
				// get Price unit
				var resultPriceUnit = _regexPriceUnit.Match(resultDelBackSem).Value;

				// Checks if Price Unit is zero
				if (resultPriceUnit.Length != 0)
				{
					if (resultPriceUnit.Last() == '0')
					{
						// Inserts semicolons to align it
						resultDelBackSem = resultDelBackSem.Insert(resultPriceUnit.Length - 2, ";;");
					}
				}

				// Checks if material exists
				if (!_regexMaterial.IsMatch(resultDelBackSem) && resultDelBackSem != " ")
				{
					var lengthToInsert = _regexCalcStructureLength.Match(resultDelBackSem).Length;
					// Inserts semicolons to align it
					resultDelBackSem = resultDelBackSem.Insert(lengthToInsert, ";");
				}

				// Writes into File
				csv.AppendLine(resultDelBackSem);
				File.WriteAllText(csvPath, csv.ToString());
			}
		}
	}
}