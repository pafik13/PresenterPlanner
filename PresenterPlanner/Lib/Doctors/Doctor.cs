using System;
using System.Collections.Generic;
using PresenterPlanner.Lib.Base;

namespace PresenterPlanner.Lib.Doctors
{
	public class Doctor: IEntity
	{
		public Doctor ()
		{
		}

		//[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public char SNChar { get; set; } 	   // Первая буква фамилии
		public string SecondName { get; set; } // Фамилия
		public string FirstName { get; set; }  // Имя
		public string ThirdName { get; set; }  // Отчество
		public bool IsChosen { get; set; }	   // Выбран?
		public int HospitalID { get; set; }	   // Больница
		public string Tel { get; set; }	   	   // Телефон
		public string Email { get; set; }	   // E-mail
		public string Position { get; set; }   // Должность
		public string Speciality { get; set; } // Специальность
		public OperatingShedule_Type osType { get; set; }
		public List<OperatingSheduleItem> OperatingShedule { get; set; }
		public WorkTime_Kind wtKind { get; set; }
		public WorkTime_Days wtDays { get; set; }
		public WorkTime_OddEven	wtOddEven { get; set; }
		public WorkTime_Type chooseNwtType { get; set; } //необходимо в форме редактирования. КРИВО!!!!!

		public string FIO () {
			return (SecondName + ' ' + FirstName + ' ' + ThirdName).Trim();
		}
	}

	public struct WorkTime_OddEven
	{
		public DateTime Odd_From;
		public DateTime Odd_Till;
		public DateTime Even_From;
		public DateTime Even_Till;
	}

	public struct WorkTime_Days
	{
		public DateTime Mon_From;
		public DateTime Mon_Till;
		public DateTime Tue_From;
		public DateTime Tue_Till;
		public DateTime Wed_From;
		public DateTime Wed_Till;
		public DateTime Thu_From;
		public DateTime Thu_Till;
		public DateTime Fri_From;
		public DateTime Fri_Till;
		public DateTime Sut_From;
		public DateTime Sut_Till;
		public DateTime Sun_From;
		public DateTime Sun_Till;
	}

	public enum WorkTime_Kind: byte {OddEven, Days};
	public enum WorkTime_Type: byte {
		Odd_From, Odd_Till, Even_From, Even_Till, 
		Mon_From, Mon_Till, Tue_From, Tue_Till, Wed_From, Wed_Till,
		Thu_From, Thu_Till, Fri_From, Fri_Till, Sut_From, Sut_Till, Sun_From, Sun_Till};
}