using System;

namespace MTUploadHelper2
{
	public class iTunesResult
	{
		public class iTuneJsonResults
		{
			public string trackId { get; set; }
			public string trackName { get; set; }
			public string artistName { get; set; }
			public string version { get; set; }
			public string description { get; set; }
			public string primaryGenreName { get; set; }
			public string artworkUrl60 { get; set; }
			public string artworkUrl100 { get; set; }
			public string trackViewUrl { get; set; }
			public string releaseNotes { get; set; }
			public string averageUserRatingForCurrentVersion { get; set; }
			public string userRatingCountForCurrentVersion { get; set; }
			public string averageUserRating { get; set; }
			public string userRatingCount { get; set; }
		}

		public iTuneJsonResults[] results { get; set; }

		public iTunesResult ()
		{
		}
	}
}

