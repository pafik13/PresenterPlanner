using System;
using System.Collections.Generic;

namespace PresenterPlanner.Lib.Report
{
	public static class ReportManager
	{
		static ReportManager ()
		{
		}

		public static IList<Report> GetReports ()
		{
			return new List<Report> (ReportRepository.GetReports ());
		}

		public static Report GetReport (int doctorID)
		{
			return ReportRepository.GetReport (doctorID);
		}
		
		public static bool SaveReports (List<Report> saveReports) 
		{
			return ReportRepository.SaveReports (saveReports);
		} 

		public static int GetMinWeekNum () {
			return ReportRepository.GetMinWeekNum ();
		}

		public static int ReportItemCompare (Report r1, Report r2)
		{
			if (r1.doctorID < r2.doctorID) {
				return -1;
			}
			if (r1.doctorID == r2.doctorID) {
				return 0;
			}
			if (r1.doctorID > r2.doctorID) {
				return 1;
			}
			return 0;
		}
	}
}

