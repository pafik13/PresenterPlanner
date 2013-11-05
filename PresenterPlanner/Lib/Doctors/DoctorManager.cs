using System;
using System.Collections.Generic;

namespace PresenterPlanner.Lib.Doctors
{
	public static class DoctorManager
	{
		static DoctorManager ()
		{
		}

		public static Doctor GetDoctor(int id)
		{
			return DoctorRepository.GetDoctor(id);
		}

		public static IList<Doctor> GetDoctors ()
		{
			return new List<Doctor>(DoctorRepository.GetDoctors());
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
	}
}