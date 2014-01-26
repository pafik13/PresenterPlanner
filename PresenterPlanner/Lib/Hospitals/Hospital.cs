using System;
using System.Collections.Generic;
using PresenterPlanner.Lib.Base;

namespace PresenterPlanner.Lib.Hospitals
{
	public class Hospital: IEntity
	{
		public Hospital ()
		{
			planners = new List<PlannerItem> ();
		}

		public int ID { get; set; }        // ID больницы/отделения
		public string Name { get; set; }   // Название больницы/отделения
		public string Adress { get; set; } // Адрес больницы/отделения
		public bool IsChosen { get; set; } // 
		public List<PlannerItem> planners { get; set; } //
	}

	public struct PlannerItem
	{
		public int weekNum;
		public DayOfWeek dayOfWeek;
	}
}
