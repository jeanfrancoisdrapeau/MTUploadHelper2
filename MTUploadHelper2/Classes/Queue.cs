using System;
using System.Collections.Generic;

namespace MTUploadHelper2
{
	public class Queue
	{
		public class QueueDetail
		{
			public string torrentFile { get; set; }
			public string appType { get; set; }
			public string appVer { get; set; }
			public string appDescrip { get; set; }
			public string appTitle { get; set; }
			public string appTags { get; set; }
			public string appImage { get; set; }

			public bool uploadDone { get; set; }
			public DateTime dateAdded { get; set; }
		}

		public List<QueueDetail> queueList {
			get;
			set;
		}

		public Queue ()
		{
			queueList = new List<QueueDetail> ();
		}
	}
}

