using System;
using System.Collections.Generic;

namespace PresenterPlanner.Lib.Report
{
	public class Report
	{
		public Report ()
		{
			rItems = new List<ReportItem> ();
		}

		public int doctorID { get; set; }
		public List<ReportItem> rItems { get; set; }

		public int FindVisitCountValue(int weekNum) {
			for (int i = 0; i < rItems.Count; i++) {
				if (rItems [i].weekNum == weekNum) {
					return rItems [i].visitCount;
				}
			}
			return 0;
		}
	}

	public class ReportItem
	{
		public ReportItem ()
		{
		}

		public int weekNum;
		public int visitCount;
	}
}

