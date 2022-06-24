using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Excel_VBA.Controller;
using Excel_VBA.Model;
using Microsoft.VisualBasic.FileIO;

namespace Excel_VBA
{
	internal class Program
	{
		static void Main(string[] args)
		{
			// C:\Users\chash\Documents\GitHub\Excel VBA\Excel VBA\LaserschneidmaschineExcelTest.csv

			ComparisonController comparisonController = new ComparisonController();
			comparisonController.compare();
		}
	}
}
