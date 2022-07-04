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

		/// <summary>
		/// Converts .txt file into .csv file
		/// </summary>
		/// <param name="csvPath"></param>
		/// <param name="txtPath"></param>
		public void ConvertTxtToCsv(string txtPath, string csvPath, string destPath)
		{
			var csvFilePath = $@"{csvPath}";
			var txtLines = System.IO.File.ReadAllLines($@"{txtPath}");



			var result = string.Join(Environment.NewLine,
				txtLines.Select(x => x.Split('\t'))
					.Select(x => string.Join(";", x)));
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
			var fileText = File.ReadAllLines(csvPath).Skip(6);
			var csv = new StringBuilder();

			foreach (var i in fileText)
			{
				var resultDelFrontSem = _regexBeforeSemicolon.Replace(i, "");
				var resultDelBackSem = _regexAfterSemicolon.Replace(resultDelFrontSem, "$1;");
				var resultPriceUnit = _regexPriceUnit.Match(resultDelBackSem).Value;

				if (resultPriceUnit.Length != 0)
				{
					if (resultPriceUnit[resultPriceUnit.Length - 1] == '0')
					{
						resultDelBackSem = resultDelBackSem.Insert(resultPriceUnit.Length - 2, ";;");
					}
				}

				if (!_regexMaterial.IsMatch(resultDelBackSem) && resultDelBackSem != " ")
				{
					var count = _regexCalcStructureLength.Match(resultDelBackSem).Length;
					resultDelBackSem = resultDelBackSem.Insert(count, ";");
				}

				csv.AppendLine(resultDelBackSem);

				File.WriteAllText(csvPath, csv.ToString());
			}
		}
	}
}