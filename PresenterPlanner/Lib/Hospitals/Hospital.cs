using System;
using PresenterPlanner.Lib.Base;

namespace PresenterPlanner.Lib.Hospitals
{
	public class Hospital: IEntity
	{
		public Hospital ()
		{
		}

		public int ID { get; set; }        // ID больницы/отделения
		public string Name { get; set; }   // Название больницы/отделения
		public string Adress { get; set; } // Адрес больницы/отделения
		public bool IsChosen { get; set; } // 
	}
}

