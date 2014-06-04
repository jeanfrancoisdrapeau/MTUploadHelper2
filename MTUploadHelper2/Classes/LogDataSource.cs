using System;
using System.Collections.Generic;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace MTUploadHelper2
{
	public class LogDataSource: NSTableViewDataSource
	{
		public List<LogDetail> Logs {
			get;
			set;
		}

		public LogDataSource ()
		{
			Logs = new List<LogDetail>();
		}

		public override int GetRowCount (NSTableView tableView)
		{
			return Logs.Count;
		}

		public override NSObject GetObjectValue (NSTableView tableView, NSTableColumn tableColumn, int row)
		{
			if (tableColumn.Identifier == "DateTime") {
				return new NSString (Logs [row].DateTime.ToString ());
			} else if (tableColumn.Identifier == "Action") {
				return new NSString (Logs [row].Action); 
			} else
				return null;
		}
	}
}

