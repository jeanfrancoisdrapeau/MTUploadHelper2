using System;

namespace MTUploadHelper2
{
	public class ReleaseInfo
	{
		public bool releaseIsCracked { get; set; }
		public bool releaseIsRetail { get; set; }
		public bool releaseIsKeygen { get; set; }

		public bool releaseIsMultilingual { get; set; }

		public string releaseName { get; set; }
		public string releaseVersion { get; set; }
		public string releaseGroup { get; set; }

		public string releaseWhatsNew { get; set; }
		public string releaseUrl { get; set; }
		public string releaseRating { get; set; }
		public string releaseRatingNb { get; set; }

		public ReleaseInfo ()
		{
		}
	}
}

