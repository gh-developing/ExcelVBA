using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel_VBA.Model
{
	public class FileinformationDto
	{
		public string Kalkulationsstruktur { get; set; }
		public string Preiseinheit { get; set; }
		public string ME { get; set; }
		public string Preis { get; set; }
		public string Kalkulationslosgroesse { get; set; }
		public string Menge { get; set; }
		public string BWME { get; set; }
		public string WertGesamt { get; set; }
		public string Waehrung { get; set; }
		public string Ressource { get; set; }
		public string Fehlerstatus { get; set; }
		public string PreisstrategieText { get; set; }
		public string KostenarteText { get; set; }
		public string KennzeichenBaugruppe { get; set; }
	}
}
