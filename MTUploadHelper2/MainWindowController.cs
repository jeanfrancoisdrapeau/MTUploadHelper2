
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using MonoMac.Foundation;
using MonoMac.AppKit;
using Newtonsoft.Json;
using CodeRinseRepeat.Deluge;

namespace MTUploadHelper2
{
	public partial class MainWindowController : MonoMac.AppKit.NSWindowController
	{
		#region Constructors

		// Called when created from unmanaged code
		public MainWindowController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public MainWindowController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Call to load from the XIB/NIB file
		public MainWindowController () : base ("MainWindow")
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
		}

		#endregion

		ReleaseInfo _releaseInfo;
		iTunesResult _itunesObj;
		Queue _uploadQueue;

		public new MainWindow Window {
			get {
				return (MainWindow)base.Window;
			}
		}

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();

			Window.Title += " " + NSBundle.MainBundle.InfoDictionary ["CFBundleVersion"];

			txtReleaseTorrentFile.Changed += event_txtReleaseTorrentFile_Changed;
			btnMASSearch.Activated += event_btnMASSearch_Activated;
			cmbMASResults.Activated += event_cmbMASResults_Activated;
			btnOpenInBrowser.Activated += event_btnOpenInBrowser_Activated;
			btnMTBuildFinalDescrip.Activated += event_btnMTBuildFinalDescrip_Activated;
			btnMTEnqueue.Activated += event_btnMTEnqueue_Activated;
			btnMTDoUpload.Activated += event_btnMTDoUpload_Activated;

			btnMTRemAll.Activated += event_btnMTRemAll_Activated;
			btnMTRemSel.Activated += event_btnMTRemSel_Activated;
			btnMTRemUpl.Activated += event_btnMTRemUpl_Activated;

			cmbMASResults.RemoveAllItems ();

			NSTableView tblViewLogInput = lstPrepareLog.DocumentView as NSTableView;
			tblViewLogInput.DataSource = new LogDataSource();

			NSTableView tblViewQueue = lstQueue.DocumentView as NSTableView;
			tblViewQueue.DataSource = new QueueDataSource();

			NSTableView tblViewLogQueue = lstQueueLog.DocumentView as NSTableView;
			tblViewLogQueue.DataSource = new LogDataSource();

			loadQueue ();
		}

		private void event_txtReleaseTorrentFile_Changed(object sender, EventArgs e)
		{
			string procName = "event_txtReleaseTorrentFile_Changed";

			try
			{
				_releaseInfo = new ReleaseInfo ();

				string fullSceneName = Path.GetFileNameWithoutExtension(txtReleaseTorrentFile.StringValue);

				fullSceneName = fullSceneName.Replace ('.', ' ');
				fullSceneName = fullSceneName.Replace ('_', ' ');
				fullSceneName = fullSceneName.Replace ('-', ' ');

				//Multilingual?
				_releaseInfo.releaseIsMultilingual = fullSceneName.IndexOf ("Multilingual") == -1 && 
					fullSceneName.IndexOf ("Bilingual") == -1 ? false : true;
				fullSceneName = fullSceneName.Replace ("Multilingual", "");
				fullSceneName = fullSceneName.Replace ("Bilingual", "");

				//Cracked, regged, retail, etc.
				_releaseInfo.releaseIsCracked = fullSceneName.IndexOf ("Cracked") == -1 ? false : true;
				_releaseInfo.releaseIsRetail = fullSceneName.IndexOf ("Retail") == -1 ? false : true;
				_releaseInfo.releaseIsKeygen = false;
				if (fullSceneName.IndexOf ("Keyfilemaker") != -1 || 
					fullSceneName.IndexOf ("Keygen") != -1 || fullSceneName.IndexOf ("Keymaker") != -1)
					_releaseInfo.releaseIsKeygen = true;

				int whereIsMacosx = fullSceneName.ToUpper().IndexOf ("MACOSX");
				_releaseInfo.releaseName = string.Empty;
				_releaseInfo.releaseVersion = string.Empty;
				bool parsingVersion = false;

				for (int i = 0; i < whereIsMacosx - 1; i++) {
					if (fullSceneName.ToUpper() [i] == 'V' && char.IsNumber(fullSceneName [i + 1]))
						parsingVersion = true;

					if (parsingVersion) {
						_releaseInfo.releaseVersion += fullSceneName [i];
					} else {
						_releaseInfo.releaseName += fullSceneName [i];
					}
				}

				if (string.IsNullOrEmpty (_releaseInfo.releaseVersion))
					_releaseInfo.releaseVersion = "v1.0";

				_releaseInfo.releaseName = _releaseInfo.releaseName.Trim ();
				_releaseInfo.releaseVersion = _releaseInfo.releaseVersion.Substring(1).Trim().Replace(' ','.');

				string[] fullSceneNameArray = fullSceneName.Split (' ');
				_releaseInfo.releaseGroup = fullSceneNameArray [fullSceneNameArray.Length - 1];

				txtMasSearchString.StringValue = _releaseInfo.releaseName;
				txtReleaseName.StringValue = _releaseInfo.releaseName;
				txtReleaseVersion.StringValue = _releaseInfo.releaseVersion;
				txtReleaseGroup.StringValue = _releaseInfo.releaseGroup;
				txtReleaseMulti.StringValue = _releaseInfo.releaseIsMultilingual.ToString ();
				if (_releaseInfo.releaseIsCracked) txtReleaseRegType.StringValue = "Cracked ";
				if (_releaseInfo.releaseIsRetail) txtReleaseRegType.StringValue = "Retail ";
				if (_releaseInfo.releaseIsKeygen) txtReleaseRegType.StringValue = "Keygen ";

				string crackType = string.Empty;
				if (_releaseInfo.releaseIsCracked)
					crackType = "Crack";
				else if (_releaseInfo.releaseIsKeygen)
					crackType = "Keygen";
				else
					crackType = "Retail";

				txtMTTitle.StringValue = _releaseInfo.releaseName + 
					" [Intel:"+ crackType +"] ["+ _releaseInfo.releaseVersion +"]";

				// Try to get the NFO file
				if (!string.IsNullOrEmpty(txtReleaseTorrentFile.StringValue))
				{
					string pathAndFile = Path.GetDirectoryName(txtReleaseTorrentFile.StringValue) + @"/" +
						Path.GetFileNameWithoutExtension(txtReleaseTorrentFile.StringValue) + @"/" ;

					List<string> files = Directory
						.GetFiles(pathAndFile, "*.*")
						.Where(file => file.ToLower().EndsWith("nfo"))
						.ToList();

					if (files.Count != 0)
						txtMTNfoFile.StringValue = files.First();
				}

			} catch (Exception ex) {
				Console.WriteLine (string.Format("ERROR: [{0}]: {1}", procName, ex.Message));
			}
		}

		private void event_btnMASSearch_Activated(object sender, EventArgs e)
		{
			string procName = "event_btnMASSearch_Activated";

			try
			{
				addLog(DateTime.Now, "[MAS] Sending search to Mac App Store...");

				//https://itunes.apple.com/search?term=apalon+weather+live&limit=10&media=software&entity=macSoftware
				//Build mac app store fetch string
				string fetchString = "https://itunes.apple.com/search?term=";

				fetchString += txtMasSearchString.StringValue.Replace (' ', '+');

				fetchString += "&limit=10&media=software&entity=macSoftware";

				Uri uri = new Uri(fetchString);

				WebClient appStoreClient = new WebClient();
				string appStoreResult = appStoreClient.DownloadString(uri);

				addLog(DateTime.Now, "[MAS] Got result(s) from Mac App Store...");

				if (!string.IsNullOrEmpty(appStoreResult))
				{
					_itunesObj = new iTunesResult ();
					_itunesObj = JsonConvert.DeserializeObject<iTunesResult> (appStoreResult);

					addLog(DateTime.Now, string.Format("[MAS] {0} applications found.", _itunesObj.results.Count()));

					cmbMASResults.RemoveAllItems ();
					foreach (iTunesResult.iTuneJsonResults item in _itunesObj.results) {
						cmbMASResults.AddItem (item.trackId + " : " + item.version + " : " + item.artistName + " " + item.trackName);
					}
				}
			} catch (Exception ex) {
				Console.WriteLine (string.Format("ERROR: [{0}]: {1}", procName, ex.Message));
			}
		}

		public void event_btnOpenInBrowser_Activated(object sender, EventArgs e)
		{
			string id = cmbMASResults.TitleOfSelectedItem.Split (':')[0].Trim();

			//https://itunes.apple.com/ca/app/id795396190
			string url = "https://itunes.apple.com/ca/app/id" + id;
			System.Diagnostics.Process.Start(url);

		}

		public void event_btnMTBuildFinalDescrip_Activated(object sender, EventArgs e)
		{
			string procName = "event_btnMTBuildFinalDescrip_Activated";

			try
			{
				rebuildDescCode ();
			} catch (Exception ex) {
				Console.WriteLine (string.Format("ERROR: [{0}]: {1}", procName, ex.Message));
			}
		}

		public void event_btnMTDoUpload_Activated(object sender, EventArgs e)
		{
			string procName = "event_btnMTDoUpload_Activated";

			CookieContainer container = new CookieContainer();
			WebClientEx client = new WebClientEx (container);

			try
			{
				addQueueLog (DateTime.Now, "[UPLOAD] Starting upload...");

				if (string.IsNullOrEmpty(txtMTUserName.StringValue) || string.IsNullOrEmpty(txtMTPassword.StringValue))
				{
					addQueueLog (DateTime.Now, "[UPLOAD] !! ERROR !! Please provide MT credentials.");
					return;
				}

				addQueueLog (DateTime.Now, "[UPLOAD] Connecting and authenticating...");

				var values = new NameValueCollection {
					{ "username", txtMTUserName.StringValue },
					{ "password", txtMTPassword.StringValue },
					{ "login", "Log in"}
				};

				// Authenticate
				client.UploadValues ("https://mac-torrents.me/login.php", values);

				string tempResponse = client.DownloadString ("https://mac-torrents.me/upload.php");

				string searchFor = "<input type=\"hidden\" name=\"auth\" value=\"";
				int indexSearchFor = tempResponse.IndexOf (searchFor) + searchFor.Length;
				string authString = tempResponse.Substring (indexSearchFor, 32);

				if (_uploadQueue != null)
				{
					foreach (Queue.QueueDetail qd in _uploadQueue.queueList)
					{
						addQueueLog (DateTime.Now, string.Format("[UPLOAD] Uploading {0}...", qd.appTitle));

						if (System.IO.File.Exists(qd.torrentFile))
						{
							FileInfo fInfo = new FileInfo(qd.torrentFile);
							long numBytes = fInfo.Length;

							FileStream fStream = new FileStream(qd.torrentFile, FileMode.Open, FileAccess.Read);
							BinaryReader br = new BinaryReader(fStream);

							FormUpload.FileParameter fp = new FormUpload.FileParameter (br.ReadBytes((int)numBytes),
								Path.GetFileName (qd.torrentFile),
								"application/octet-stream");

							br.Close();
							fStream.Close();

							string releaseType = string.Empty;
							if (qd.appType == "App")
								releaseType = "1";
							if (qd.appType == "Game")
								releaseType = "2";
							if (qd.appType == "Other")
								releaseType = "5";
							if (qd.appType == "E-Books")
								releaseType = "6";

							// Upload torrent
							Dictionary<string, object> paramsDic = new Dictionary<string, object> ();
							paramsDic.Add ("submit", "true");
							paramsDic.Add ("auth", authString);
							paramsDic.Add ("type", releaseType);
							paramsDic.Add ("title", qd.appTitle);
							paramsDic.Add ("tags", qd.appTags);
							paramsDic.Add ("image", qd.appImage);
							paramsDic.Add ("desc", qd.appDescrip);
							paramsDic.Add ("file_input", fp);

							if (qd.appVer == "10.6") {
								paramsDic.Add ("macos[]", new string[] {"1", "2", "3", "4", "5"});
							}
							if (qd.appVer == "10.7")
							{
								paramsDic.Add ("macos[]", new string[] {"2", "3", "4", "5"});
							}
							if (qd.appVer == "10.8")
							{
								paramsDic.Add ("macos[]", new string[] {"3", "4", "5"});
							}			
							if (qd.appVer == "10.9")
							{
								paramsDic.Add ("macos[]", new string[] {"4", "5"});
							}
							if (qd.appVer == "10.10")
							{
								paramsDic.Add ("macos[]", "5");
							}

							//Upload torrent
							HttpWebResponse resp = FormUpload.MultipartFormDataPost (
								"https://mac-torrents.me/upload.php",
								"MTUploaderHelper2.NET",
								paramsDic,
								container);

							string localFile = Path.GetDirectoryName(qd.torrentFile) + @"/" + Path.GetFileNameWithoutExtension(qd.torrentFile) + "_mt.torrent";

							//from https://mac-torrents.me/torrents.php?id=1886
							// to  https://mac-torrents.me/torrents.php?action=download&id=1887

							int numIndex = resp.ResponseUri.AbsoluteUri.IndexOf("=");
							string strNum = resp.ResponseUri.AbsoluteUri.Substring(numIndex + 1);
							int dlId;

							int.TryParse(strNum, out dlId);
							dlId += 1;
							string remoteFile = string.Format("https://mac-torrents.me/torrents.php?action=download&id={0}", dlId);

							addQueueLog (DateTime.Now, string.Format("[UPLOAD] Downloading torrent file {0}...", localFile));

							client.DownloadFile(remoteFile, localFile);

							if (cmbDelugeServer.StringValue.Length != 0 &&
								txtPathOnServer.StringValue.Length != 0)
							{
								addQueueLog (DateTime.Now, string.Format("[DELUGE] Adding torrent {0}...", localFile));

								string host = string.Empty, port = string.Empty, password = string.Empty;

								switch (cmbDelugeServer.StringValue)
								{
									case "Pasta":
									host = @"http://pasta.whatbox.ca";
									port = "52114";
									password = "$N0feaR!";
									break;

									case "Peanut":
									host = @"http://peanut.whatbox.ca";
									port = "39092";
									password = "$N0feaR!";
									break;
								}

								var dclient = new DelugeClient (host, int.Parse(port));
								dclient.Login (password);
								dclient.AddTorrentFile(localFile, txtPathOnServer.StringValue);
							}
								
							qd.uploadDone = true;

							var tv = lstQueue.DocumentView as NSTableView;
							var ds = tv.DataSource as QueueDataSource;
							ds.queueList = _uploadQueue.queueList;
							tv.ReloadData();

							NSRunLoop.Current.RunUntil(DateTime.Now.AddMilliseconds(10000));
						} else {
							addQueueLog (DateTime.Now, string.Format("[UPLOAD] !! ERROR !! {0} not found, skipping.", qd.torrentFile));
						}
					}
				}

				addQueueLog (DateTime.Now, "[UPLOAD] Logging out...");
				//client.DownloadString ("https://mac-torrents.me/logout.php?auth=" + authString);

			} catch (Exception ex) {
				Console.WriteLine (string.Format("ERROR: [{0}]: {1}", procName, ex.Message));
			} finally {
				client.Dispose ();
				client = null;

				saveQueue ();

				addQueueLog (DateTime.Now, "[UPLOAD] Done.");
			}
		}

		public void event_cmbMASResults_Activated(object sender, EventArgs e)
		{
			string procName = "event_cmbMASResults_Activated";

			try
			{
				if (_releaseInfo == null)
					return;

				string id = cmbMASResults.TitleOfSelectedItem.Split (':')[0].Trim();

				iTunesResult.iTuneJsonResults first = _itunesObj.results.First(o => o.trackId.Equals(id));

				//txtCompCode.StringValue = string.Empty;
				txtMTTags.StringValue = first.primaryGenreName.Replace(' ',',');
				txtMTImageUrl.StringValue = first.artworkUrl100;
				txtMTDescrip.Value = first.description;

				_releaseInfo.releaseWhatsNew = first.releaseNotes;
				_releaseInfo.releaseUrl = first.trackViewUrl;
				_releaseInfo.releaseRating = first.averageUserRating;
				_releaseInfo.releaseRatingNb = first.userRatingCount;
				//rebuildDescCode ();
			} catch (Exception ex) {
				Console.WriteLine (string.Format("ERROR: [{0}]: {1}", procName, ex.Message));
			}
		}

		private void event_btnMTEnqueue_Activated(object sender, EventArgs e)
		{
			string procName = "event_btnMTEnqueue_Activated";

			try
			{
				addLog(DateTime.Now, string.Format("[QUEUE] Adding [{0}] to upload queue...", txtMTTitle.StringValue));

				if (_uploadQueue == null)
				{
					_uploadQueue = new Queue();
				}

				Queue.QueueDetail queueItem = new Queue.QueueDetail();
				queueItem.torrentFile = txtReleaseTorrentFile.StringValue;
				queueItem.appDescrip = txtMTFinalDescrip.Value;
				queueItem.appImage = txtMTImageUrl.StringValue;
				queueItem.appTags = txtMTTags.StringValue;
				queueItem.appTitle = txtMTTitle.StringValue;
				queueItem.appType = cmbMTAppType.TitleOfSelectedItem;
				queueItem.appVer = cmbMTOsxVersion.TitleOfSelectedItem;

				queueItem.uploadDone = false;
				queueItem.dateAdded = DateTime.Now;

				_uploadQueue.queueList.Add(queueItem);

				var tv = lstQueue.DocumentView as NSTableView;
				var ds = tv.DataSource as QueueDataSource;
				ds.queueList = _uploadQueue.queueList;

				tv.ReloadData();

				txtReleaseTorrentFile.StringValue = string.Empty;
				txtReleaseName.StringValue = string.Empty;
				txtReleaseGroup.StringValue = string.Empty;
				txtReleaseRegType.StringValue = string.Empty;
				txtReleaseVersion.StringValue = string.Empty;
				txtReleaseMulti.StringValue = string.Empty;

				txtMTTitle.StringValue = string.Empty;
				txtMTTags.StringValue = string.Empty;
				txtMTImageUrl.StringValue = string.Empty;
				txtMTDescrip.Value = string.Empty;
				txtMTNfoFile.StringValue = string.Empty;
				txtMTFinalDescrip.Value = string.Empty;

				saveQueue();

				addLog(DateTime.Now, "[QUEUE] Done.");

			} catch (Exception ex) {
				Console.WriteLine (string.Format("ERROR: [{0}]: {1}", procName, ex.Message));
			}
		}

		private void event_btnMTRemAll_Activated(object sender, EventArgs e)
		{
			addQueueLog(DateTime.Now, "[QUEUE] Creating a new queue...");

			_uploadQueue = null;

			var tv = lstQueue.DocumentView as NSTableView;
			var ds = tv.DataSource as QueueDataSource;
			ds.queueList = null;

			tv.ReloadData();

			saveQueue ();
		}

		private void event_btnMTRemSel_Activated(object sender, EventArgs e)
		{
			if (_uploadQueue == null)
				return;

			addQueueLog(DateTime.Now, "[QUEUE] Removing selected items from queue...");

			var tv = lstQueue.DocumentView as NSTableView;
			int rowIndex = tv.SelectedRow;

			_uploadQueue.queueList.RemoveAt (rowIndex);

			var ds = tv.DataSource as QueueDataSource;
			ds.queueList = _uploadQueue.queueList;

			tv.ReloadData();

			saveQueue ();
		}

		private void event_btnMTRemUpl_Activated(object sender, EventArgs e)
		{
			if (_uploadQueue == null)
				return;

			addQueueLog(DateTime.Now, "[QUEUE] Removing uploaded items from queue...");

			Queue tempQueue = new Queue ();

			foreach (Queue.QueueDetail qd in _uploadQueue.queueList) {
				if (qd.uploadDone != true) {

					Queue.QueueDetail qa = qd;
					tempQueue.queueList.Add (qa);

				}
			}

			_uploadQueue = tempQueue;

			var tv = lstQueue.DocumentView as NSTableView;
			var ds = tv.DataSource as QueueDataSource;
			ds.queueList = _uploadQueue.queueList;

			tv.ReloadData();

			saveQueue ();
		}

		private void saveQueue()
		{
			addQueueLog(DateTime.Now, "[QUEUE] Saving queue to file...");

			var documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
			var filename = Path.Combine (documents, "mtuploadhelper2_queue.xml");

			XmlSerializer serializer = new XmlSerializer (typeof(Queue));
			TextWriter textWriter = new StreamWriter (filename);
			serializer.Serialize (textWriter, _uploadQueue);
			textWriter.Close ();
		}

		private void loadQueue()
		{
			addQueueLog(DateTime.Now, "[QUEUE] Restoring queue from file...");

			var documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
			var filename = Path.Combine (documents, "mtuploadhelper2_queue.xml");

			if (System.IO.File.Exists (filename)) {
				XmlSerializer deserializer = new XmlSerializer (typeof(Queue));
				TextReader textReader = new StreamReader (filename, Encoding.UTF8);
				_uploadQueue = (Queue)deserializer.Deserialize (textReader);
				textReader.Close ();

				if (_uploadQueue != null) {
					var tv = lstQueue.DocumentView as NSTableView;
					var ds = tv.DataSource as QueueDataSource;
					ds.queueList = _uploadQueue.queueList;

					tv.ReloadData ();
				}
			}
		}

		private void addLog(DateTime datetime, string action)
		{
			var l = new LogDetail(datetime, action);

			var tv = lstPrepareLog.DocumentView as NSTableView;
			var ds = tv.DataSource as LogDataSource;
			ds.Logs.Add(l);

			tv.ReloadData();

			tv.ScrollRowToVisible (ds.Logs.Count - 1);

			NSRunLoop.Current.RunUntil(DateTime.Now.AddMilliseconds(1));
		}

		private void addQueueLog(DateTime datetime, string action)
		{
			var l = new LogDetail(datetime, action);

			var tv = lstQueueLog.DocumentView as NSTableView;
			var ds = tv.DataSource as LogDataSource;
			ds.Logs.Add(l);

			tv.ReloadData();

			tv.ScrollRowToVisible (ds.Logs.Count - 1);

			NSRunLoop.Current.RunUntil(DateTime.Now.AddMilliseconds(1));
		}

		public void rebuildDescCode()
		{
			string procName = "rebuildDescCode";

			try
			{
				if (_releaseInfo == null)
					return;

				//MacTorrents Code
				string typeOfCrack = string.Empty;
				if (_releaseInfo.releaseIsCracked)
					typeOfCrack = "Crack";
				if (_releaseInfo.releaseIsKeygen)
					typeOfCrack = "Keygen";
				if (_releaseInfo.releaseIsRetail)
					typeOfCrack = "Retail";

				string appStoreRating = string.Empty;
				if (_releaseInfo.releaseRatingNb != "0") {
					appStoreRating = (string.IsNullOrEmpty (_releaseInfo.releaseRating) ? "0" : _releaseInfo.releaseRating) +
						" (" + (string.IsNullOrEmpty (_releaseInfo.releaseRatingNb) ? "0" : _releaseInfo.releaseRatingNb) + " users)";
				} else {
					appStoreRating = "Unknown";
				}

				string finalCode = "[b]Name:[/b] " + _releaseInfo.releaseName + "\n";
				finalCode += "[b]Version:[/b] - " + _releaseInfo.releaseVersion + "\n\n";
				finalCode += "[b]Mac Platform:[/b] Intel\n";
				finalCode += "[b]Includes:[/b] " + typeOfCrack + "*\n\n";
				finalCode += "[b]OS version:[/b] " + cmbMTOsxVersion.TitleOfSelectedItem + " or higher\n\n";
				finalCode += "[b]Link for more information:[/b] " + _releaseInfo.releaseUrl + "\n";
				finalCode += "[b]Rating:[/b] " + appStoreRating + "\n\n";
				finalCode += "[b]Info:[/b]\n\n" + txtMTDescrip.Value + "\n\n";
				finalCode += "[b]What's New:[/b] \n" + 
					(string.IsNullOrEmpty(_releaseInfo.releaseWhatsNew) ? "Unknown or First version" : _releaseInfo.releaseWhatsNew) + "\n\n";
				finalCode += "[b]*" + typeOfCrack + " courtesy of " + _releaseInfo.releaseGroup + "[/b]\n";
				finalCode += "[b]Original Scene Files:[/b] Yes\n\n";

				if (!string.IsNullOrEmpty (txtMTNfoFile.StringValue)) {
					using (StreamReader sr = new StreamReader (txtMTNfoFile.StringValue, Encoding.GetEncoding(28591))) {
						string line = sr.ReadToEnd ();

						finalCode += "[b]NFO:[/b]\n" +
							"[hide][pre]" +
							line +
							"[/pre][/hide]\n\n";
					}
				}

				finalCode += "[b]Unzip/Unrar HowTo:[/b]\n" +
					"[hide][pre]" +
					"Option A:\n" +
					"- Download and install my app\n" +
					"  https://mac-torrents.me/forums.php?action=viewthread&threadid=102\n" +
					"- Make a shortcut to it in the launchpad\n" +
					"- Drag&drop any Zip or Rar files on the shortcut\n" +
					"- The app will extract all the files in a new folder\n" +
					"- Open the new folder\n" +
					"- Repeat the process until everything has been extracted.\n\n" + 
					"Option B:\n" +
					"- Open the terminal (CLI)\n" +
					"- Install Brew **DO THIS ONLY THE FIRST TIME**\n" +
					"  ruby -e \"$(curl -fsSL https://raw.github.com/Homebrew/homebrew/go/install)\"\n" +
					"- Install Unrar **DO THIS ONLY THE FIRST TIME**\n" +
					"  brew install unrar\n" +
					"- Change directory to where you downloaded the torrent\n" +
					"  ie. cd /Users/Jeff/Downloads/Torrent/Bob.Came.In.Pieces.v1.5.MacOSX-WaLMaRT\n" +
					"- To unzip files, use this:\n" +
					"  unzip -u -o \\*.zip\n" +
					"- To unrar files, use this:\n" +
					"  unrar e -y first_rar_file.rar\n" +
					"- Repeat the process until everything has been extracted.\n" +
					"[/pre][/hide]\n\n" +
					"[b]Install HowTo:[/b]\n" +
					"[hide][pre]" +
					"Most of my uploads are divided into three categories:\n" +
					"1. Retail/Patched\n" +
					"- Once everything has been extracted, simply drag&drop the app in your Applications folder\n" +
					"- Done!\n" +
					"2. Keygen\n" +
					"- Just like 1., except that mostly on the first run, the application will ask you for a\n" +
					"  registration key.\n" +
					"- Use the provided keygen to generate an activation key\n" +
					"- Usually copy/paste that key in the application and you're done.\n" +
					"3. Crack\n" +
					"- Just like 1., but sometimes the crack will need to be applied manually\n" +
					"- Copy/paste the cracked file(s) in the application folder and overwrite existing files.\n" +
					"- 'chmod +x' the replaced file just to make sure it's an executable.\n" +
					"- Done!\n" +
					"[/pre][/hide]\n\n" +
					"As always, if you need any help, just PM me!";

				txtMTFinalDescrip.Value = finalCode;
			} catch (Exception ex) {
				Console.WriteLine (string.Format("ERROR: [{0}]: {1}", procName, ex.Message));
			}
		}
	}
}

