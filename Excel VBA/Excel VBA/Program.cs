using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Excel_VBA.Controller;
using Excel_VBA.Model;
using Microsoft.VisualBasic.FileIO;

namespace Excel_VBA
{
	public class Program
	{
		public static void Main(string[] args)
		{
			/*ComparisonService c = new ComparisonService();
			c.CompareFiles();*/

			// FileImportService f = new FileImportService(@"C:\Users\chash\Desktop\FilesCSV\GeneratedFiles\File1.csv");

			ConvertTxtToCsvService c = 
				new ConvertTxtToCsvService();

			c.RegexCsv(@"C:\Users\chash\Desktop\FilesCSV\GeneratedFiles\File1.csv");
		}
	}
}
