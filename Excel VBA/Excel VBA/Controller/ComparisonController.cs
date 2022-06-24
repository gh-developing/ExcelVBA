using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel_VBA.Model;

namespace Excel_VBA.Controller
{
	public class ComparisonController
	{
		private FileImportController _fileImportController = new FileImportController();
		private List<FileinformationDto> _importFile1 = new List<FileinformationDto>();
		private List<FileinformationDto> _importFile2 = new List<FileinformationDto>();
		private int _checkOnDifference = 0;
		private void generateFiles()
		{
			Console.WriteLine("File Path for comparison 1:");
			string filepath1 = Console.ReadLine();
			Console.WriteLine("File Path for comparison 2:");
			string filepath2 = Console.ReadLine();
			Console.WriteLine("Compare on CHf 1 or CHf 10:");
			string checkOnDifference = Console.ReadLine();

			_checkOnDifference = Int32.Parse(checkOnDifference);
			_importFile1 = _fileImportController.SaveDataIntoDto($@"{filepath1}");
			_importFile2 = _fileImportController.SaveDataIntoDto($@"{filepath2}");
		}

		public void compare()
		{
			generateFiles();
			int length = _importFile1.Count > _importFile2.Count ? _importFile1.Count : _importFile2.Count;
			Console.Write(length);
			for (int i = 0; i < length; i++)
			{
				if (_importFile1[i].Ressource == "" || _importFile2[i].Ressource == "")
				{
					Console.WriteLine("NULL VALUE IN IMPORT FILE 1 or 2 on row " + _importFile1[i].Ressource);
				}
				else
				{

				}
			}
		}

		/*private FileinformationDto setCalculationOutput(string messageBOM, string messageCOSTS, FileinformationDto fileinformationDto)
		{
		public string Id { get; set; }
		public string BOM { get; set; }
		public string COSTS { get; set; }
		fileinformationDto.BOM = messageBOM;
			fileinformationDto.COSTS = messageCOSTS;
			return fileinformationDto;
		}*/

	}
}
