using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Excel_VBA.Model;

namespace Excel_VBA.Controller
{
	public class FileImportController
	{
		List<string> fileRowList = new List<string>();
		List<string> fileColList = new List<string>();
		List<FileinformationDto> _fileinformationDto = new List<FileinformationDto>();

		public List<FileinformationDto> SaveDataIntoDto(string filepath)
		{
			_fileinformationDto = new List<FileinformationDto>();
			fileColList = new List<string>();
			fileRowList = new List<string>();

			FileImport(filepath);
			SplitFileRows();
			for (int j = 0; j < fileColList.Count;)
			{
				var fileinformationElement = new FileinformationDto()
				{
					Kalkulationsstruktur = fileColList[j++],
					Preiseinheit = fileColList[j++],
					ME = fileColList[j++],
					Preis = fileColList[j++],
					Kalkulationslosgroesse = fileColList[j++],
					Menge = fileColList[j++],
					BWME = fileColList[j++],
					WertGesamt = fileColList[j++],
					Waehrung = fileColList[j++],
					Ressource = fileColList[j++],
					Fehlerstatus = fileColList[j++],
					PreisstrategieText = fileColList[j++],
					KostenarteText = fileColList[j++],
					KennzeichenBaugruppe = fileColList[j++],
				};
				_fileinformationDto.Add(fileinformationElement);
			}
			
			return _fileinformationDto;
		}

		private List<string> FileImport(string filepath)
		{
			
				using (var reader = new StreamReader(filepath))
				{
					while (!reader.EndOfStream)
					{
						var line = reader.ReadLine();
						var values = line.Split(';');

						fileRowList.Add(values[0]);
					}
				}

				return fileRowList;


		}

		private List<string> SplitFileRows()
		{
			foreach (var fileRow in fileRowList)
			{
				foreach (var fileCol in fileRow.Split(','))
				{
					fileColList.Add(fileCol);
				}
			}

			return fileColList;
		}
	}
}
