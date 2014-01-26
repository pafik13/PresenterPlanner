using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PresenterPlanner.Lib
{
	public class PlannerManager
	{
		public static DateTime[] GetWeeks(int weekNum, DateTime dayIn1stWeek) {
			DateTime[] dt = new DateTime[5*weekNum];
			int offset;
			if (dayIn1stWeek.DayOfWeek == DayOfWeek.Sunday) {
				offset = -6;
			} else {
				offset = 1 - (int)dayIn1stWeek.DayOfWeek;
			}
			DateTime startDay = dayIn1stWeek.AddDays (offset);
			int j = 0;
			for (int i=0; i < 7*weekNum; i++) {
				if ((startDay.AddDays (i).DayOfWeek != DayOfWeek.Saturday) && (startDay.AddDays (i).DayOfWeek != DayOfWeek.Sunday)) {
					dt [j] = startDay.AddDays (i);
					j++;
				}
			}
			return dt;
		}
	}
}

