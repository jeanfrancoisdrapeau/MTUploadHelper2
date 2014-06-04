using System;
using System.Net;

namespace MTUploadHelper2
{
	public class WebClientEx : WebClient
	{
		public CookieContainer CookieContainer { get; private set; }

		public WebClientEx()
		{
			CookieContainer = new CookieContainer();
		}

		protected override WebRequest GetWebRequest(Uri address)
		{
			string procName = "GetWebRequest";

			try
			{
				var request = base.GetWebRequest(address);
				if (request is HttpWebRequest)
				{
					(request as HttpWebRequest).CookieContainer = CookieContainer;
				}
				return request;
			} catch (Exception ex) {
				Console.WriteLine (string.Format("ERROR: [{0}]: {1}", procName, ex.Message));
				return null;
			}
		}
	}
}

