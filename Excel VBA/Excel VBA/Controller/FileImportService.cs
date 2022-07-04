using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Excel_VBA.Model;

namespace Excel_VBA.Controller
{
	public class FileImportService
	{
		public List<FilePropertiesDto> FileProperties;
		public FileImportService(string filePath)
		{
			FileProperties = File
				.ReadAllLines(filePath)
				.Skip(2)
				.Select(LineFromCsv)
				.ToList();
		}

		public void ConsoleWriteDto(List<FilePropertiesDto> filePropertiesDto)
		{
			foreach (var fileProperty in filePropertiesDto)
			{
				Console.WriteLine(fileProperty.Kalkulationsstruktur);
				Console.WriteLine(fileProperty.Material);
				Console.WriteLine(fileProperty.Preiseinheit);
				Console.WriteLine(fileProperty.ME);
				Console.WriteLine(fileProperty.Preis);
				Console.WriteLine(fileProperty.Kalkulationslosgroesse);
				Console.WriteLine(fileProperty.Menge);
				Console.WriteLine(fileProperty.BWME);
				Console.WriteLine(fileProperty.WertGesamt);
				Console.WriteLine(fileProperty.Waehrung);
				Console.WriteLine(fileProperty.Fehlerstatus);
				Console.WriteLine(fileProperty.PreisstrategieText);
				Console.WriteLine(fileProperty.KennzeichenBaugruppe);
				Console.WriteLine(fileProperty.KostenarteText);
				Console.WriteLine(fileProperty.Version);
				Console.WriteLine(fileProperty.Kalkulationsvariante);
				Console.WriteLine(fileProperty.Kostenart);
				Console.WriteLine(fileProperty.Ressource);
			}
		}

		private static FilePropertiesDto LineFromCsv(string csvLine)
		{
			var values = csvLine.Split(';');
			var fileProperties = new FilePropertiesDto
			{
				Kalkulationsstruktur = values[0],
				Material = values[1],
				Preiseinheit = values[2],
				ME = values[3],
				Preis = values[4],
				Kalkulationslosgroesse = values[5],
				Menge = values[6],
				BWME = values[7],
				WertGesamt = values[8],
				Waehrung = values[9],
				Fehlerstatus = values[10],
				PreisstrategieText = values[11],
				KennzeichenBaugruppe = values[12],
				KostenarteText = values[13],
				Version = values[14],
				Kalkulationsvariante = values[15],
				Kostenart = values[16],
				Ressource = values[17]
			};

			return fileProperties;
		}
	}
}