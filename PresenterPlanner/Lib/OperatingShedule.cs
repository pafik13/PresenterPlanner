using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PresenterPlanner.Lib.Doctors;

namespace PresenterPlanner.Lib
{
	public class OperatingShedule
	{
		static List <Tuple<OperatingShedule_SubType, String>> sheduleDays;
		static List <Tuple<OperatingShedule_SubType, String>> sheduleDates;

		static OperatingShedule()
		{
			sheduleDays = new List <Tuple<OperatingShedule_SubType, String>> ();
			sheduleDays.Add(new Tuple<OperatingShedule_SubType, String>(OperatingShedule_SubType.Mon, "Понедельник"));
			sheduleDays.Add(new Tuple<OperatingShedule_SubType, String>(OperatingShedule_SubType.Tue, "Вторник"));
			sheduleDays.Add(new Tuple<OperatingShedule_SubType, String>(OperatingShedule_SubType.Wed, "Среда"));
			sheduleDays.Add(new Tuple<OperatingShedule_SubType, String>(OperatingShedule_SubType.Thu, "Четверг"));
			sheduleDays.Add(new Tuple<OperatingShedule_SubType, String>(OperatingShedule_SubType.Fri, "Пятница"));
			sheduleDays.Add(new Tuple<OperatingShedule_SubType, String>(OperatingShedule_SubType.Sut, "Суббота"));
			sheduleDays.Add(new Tuple<OperatingShedule_SubType, String>(OperatingShedule_SubType.Sun, "Воскресенье"));

			sheduleDates = new List <Tuple<OperatingShedule_SubType, String>> ();
			sheduleDates.Add(new Tuple<OperatingShedule_SubType, String>(OperatingShedule_SubType.Odd, "Четные числа"));
			sheduleDates.Add(new Tuple<OperatingShedule_SubType, String>(OperatingShedule_SubType.Even, "Нечетные числа"));
		}

		public static List<OperatingShedule_SubType> GetOthers ( Doctor doc )
		{
			switch(doc.osType) {
			case OperatingShedule_Type.Dates: { 
					break;
				}
			case OperatingShedule_Type.Days: {
					break;
				}
			}
			return null;
		}
	}

	public class OperatingSheduleItem
	{
		OperatingSheduleItem() {}
		public OperatingShedule_SubType sb;
		public DateTime From;
		public DateTime Till;
	}

	public enum OperatingShedule_Type: byte {Dates, Days};
	public enum OperatingShedule_SubType: byte {Mon = 1, Tue, Wed, Thu, Fri, Sut, Sun, Odd, Even};
}

