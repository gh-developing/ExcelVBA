using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel_VBA.Model;

namespace Excel_VBA.Controller
{
	public class ComparisonService
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
			GenerateFiles();

			// Todo compare both files and save into bom amnd costs

			/*for (int i = 0; i < 3000; i++)
			{
				var file = new FileCalculationPropertiesDto
				{
					BOM = "ok",
					COSTS = "ok",
					File1Properties = _fileImportComparison1.FileProperties[i],
					File2Properties = _fileImportComparison2.FileProperties[i],
				};

				_fileProperties.Add(file);
			}*/

			_generateNewCsvService.GenerateCsvFile(_directoryPath, _fileProperties);
		}

		/// <summary>
		/// Generates .csv File with User Input
		/// </summary>
		public void GenerateFiles()
		{
			// File 1 and 2 for the Comparisons
			for (int i = 1; i < 3; i++)
			{
				Console.WriteLine("File Path for comparison " + i + ":");
				var txtFilePath = Console.ReadLine();
				Directory.CreateDirectory(Path.GetDirectoryName(txtFilePath) + "\\GeneratedFiles\\");
				_directoryPath = Path.GetDirectoryName(txtFilePath) + "\\GeneratedFiles\\";
				try
				{
					_convertTxtToCsvService.ConvertTxtToCsv(txtFilePath, _directoryPath + "File" + i + ".csv",
						_directoryPath + "FileComparison" + i + ".csv");
				}
				catch (Exception e)
				{
					Console.WriteLine("Can't compare empty File \n File " + i + "is empty or not valid.");
				}
			}

			// set File to Field
			_fileImportComparison1 = new FileImportService(_directoryPath + "FileComparison1.csv");
			_fileImportComparison2 = new FileImportService(_directoryPath + "FileComparison2.csv");

			// Compare costs number
			Console.WriteLine("Compare on CHf 1 or CHf 10:");
			string checkOnDifference = Console.ReadLine();

		}

		/// <summary>
		/// clears All Files from Directory with generated Files
		/// </summary>
		public void ClearAllFiles()
		{
			DirectoryInfo directory = new DirectoryInfo(_directoryPath);
			if (directory.GetFiles().Length != 0)
			{
				foreach (FileInfo file in directory.GetFiles())
				{
					file.Delete();
				}
			}
		}
	}
}
