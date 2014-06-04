using System;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace MTUploadHelper2
{
	public class LogDetail: NSObject
	{
		public DateTime DateTime {
			get;
			set;
		}

		public string Action {
			get;
			set;
		}

		public LogDetail (DateTime dt, string action)
		{
			DateTime = dt;
			Action = action;
		}
	}
}

