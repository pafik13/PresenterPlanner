using System;
using System.Collections.Generic;
using System.Globalization;

namespace PresenterPlanner.Lib.Doctors
{
	public static class DoctorManager
	{
		static DoctorManager ()
		{
		}

		public static Doctor GetDoctor(int id, bool isNeedNew = true)
		{
			return DoctorRepository.GetDoctor(id, isNeedNew);
		}

		public static IList<Doctor> GetDoctors ()
		{
			return new List<Doctor>(DoctorRepository.GetDoctors());
		}

		public static IList<Doctor> GetDoctors (int hospitalID)
		{
			return new List<Doctor>(DoctorRepository.GetDoctors(hospitalID));
		}

		public static IList<Doctor> GetSelectedDoctors ()
		{
			return new List<Doctor>(DoctorRepository.GetSelectedDoctors());
		}

		public static int SaveDoctor (Doctor item)
		{
			return DoctorRepository.SaveDoctor(item);
		}

		public static int DeleteDoctor(int id)
		{
			return DoctorRepository.DeleteDoctor(id);
		}

		public static int DoctorCompare (Doctor doc1, Doctor doc2)
		{
			return String.CompareOrdinal (doc1.SecondName, doc2.SecondName);
		}
	}
}