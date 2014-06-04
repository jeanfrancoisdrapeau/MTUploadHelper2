using System;
using System.Collections.Generic;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace MTUploadHelper2
{
	public class QueueDataSource: NSTableViewDataSource
	{
		public List<Queue.QueueDetail> queueList {
			get;
			set;
		}

		public override int GetRowCount (NSTableView tableView)
		{
			if (queueList == null)
				return 0;
			return queueList.Count;
		}

		public override NSObject GetObjectValue (NSTableView tableView, NSTableColumn tableColumn, int row)
		{
			if (tableColumn.Identifier == "DateTime") {
				return new NSString (queueList [row].dateAdded.ToString ());
			} else if (tableColumn.Identifier == "Title") {
				return new NSString (queueList [row].appTitle); 
			} else if (tableColumn.Identifier == "Uploaded") {
				return new NSString (queueList [row].uploadDone.ToString()); 
			} else
				return null;
		}

		public QueueDataSource ()
		{
		}
	}
}

