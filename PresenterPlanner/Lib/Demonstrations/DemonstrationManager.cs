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
	public class DemonstrationManager
	{
		static DemonstrationManager ()
		{
		}

		public static void RefreshDemonstrations() 
		{
			DemonstrationRepository.ReReadDemonstrations();
		}

		public static void CurrentDemonstrationToArchive() 
		{
			DemonstrationRepository.WriteToArchive();
		}

		public static List<string> GetPathsAfterSplit() {
			return DemonstrationRepository.GetSplitFiles();
		}

		public static Demonstration GetDemonstration(int docID, DateTime visitDate)
		{
			return DemonstrationRepository.GetDemonstration(docID, visitDate);
		}

		public static Demonstration GetLastDemonstration(int docID)
		{
			return DemonstrationRepository.GetLastDemonstration(docID);
		}

		public static int SaveDemonstration (Demonstration item)
		{
			return DemonstrationRepository.SaveDemonstration(item);
		}
	}
}

