using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel_VBA.Model
{
	public class FileCalculationPropertiesDto
	{
		public string BOM { get; set; }
		public string COSTS { get; set; }

		public FilePropertiesDto File1Properties { get; set; }
		public FilePropertiesDto File2Properties { get; set; }
	}
}
