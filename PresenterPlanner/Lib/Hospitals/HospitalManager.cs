using System;
using System.Collections.Generic;

namespace PresenterPlanner.Lib.Hospitals
{
	public static class HospitalManager
	{
		static HospitalManager ()
		{
		}

		public static Hospital GetHospital(int id)
		{
			return HospitalRepository.GetHospital(id);
		}

		public static IList<Hospital> GetHospitals ()
		{
			return new List<Hospital>(HospitalRepository.GetHospitals());
		}

		public static IList<Hospital> GetSelectedHospitals ()
		{
			return new List<Hospital>(HospitalRepository.GetSelectedHospitals());
		}

		public static IList<Hospital> GetAvailableHospitals (int weekNum, DayOfWeek dayOfWeek)
		{
			return new List<Hospital>(HospitalRepository.GetAvailableHospitals(weekNum, dayOfWeek));
		}

		public static IList<Hospital> GetChoosenHospitals (int weekNum, DayOfWeek dayOfWeek)
		{
			return new List<Hospital>(HospitalRepository.GetChoosenHospitals(weekNum, dayOfWeek));
		}

		public static int SaveHospital (Hospital item)
		{
			return HospitalRepository.SaveHospital(item);
		}

		public static int DeleteHospital(int id)
		{
			return HospitalRepository.DeleteHospital(id);
		}

		public static int HospitalCompare (Hospital hosp1, Hospital hosp2)
		{
			return String.CompareOrdinal (hosp1.Name, hosp2.Name);
		}
	}
}