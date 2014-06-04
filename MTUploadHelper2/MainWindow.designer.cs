// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;
using System.CodeDom.Compiler;

namespace MTUploadHelper2
{
	[Register ("MainWindowController")]
	partial class MainWindowController
	{
		[Outlet]
		MonoMac.AppKit.NSButton btnMASSearch { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton btnMTBuildFinalDescrip { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton btnMTDoUpload { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton btnMTEnqueue { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton btnOpenInBrowser { get; set; }

		[Outlet]
		MonoMac.AppKit.NSPopUpButton cmbMASResults { get; set; }

		[Outlet]
		MonoMac.AppKit.NSPopUpButton cmbMTAppType { get; set; }

		[Outlet]
		MonoMac.AppKit.NSPopUpButton cmbMTOsxVersion { get; set; }

		[Outlet]
		MonoMac.AppKit.NSScrollView lstPrepareLog { get; set; }

		[Outlet]
		MonoMac.AppKit.NSScrollView lstQueue { get; set; }

		[Outlet]
		MonoMac.AppKit.NSScrollView lstQueueLog { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtMasSearchString { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextView txtMTDescrip { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextView txtMTFinalDescrip { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtMTImageUrl { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtMTNfoFile { get; set; }

		[Outlet]
		MonoMac.AppKit.NSSecureTextField txtMTPassword { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtMTTags { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtMTTitle { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtMTUserName { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtReleaseGroup { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtReleaseMulti { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtReleaseName { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtReleaseRegType { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtReleaseTorrentFile { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtReleaseVersion { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (lstQueueLog != null) {
				lstQueueLog.Dispose ();
				lstQueueLog = null;
			}

			if (txtMTUserName != null) {
				txtMTUserName.Dispose ();
				txtMTUserName = null;
			}

			if (txtMTPassword != null) {
				txtMTPassword.Dispose ();
				txtMTPassword = null;
			}

			if (btnMTDoUpload != null) {
				btnMTDoUpload.Dispose ();
				btnMTDoUpload = null;
			}

			if (btnMASSearch != null) {
				btnMASSearch.Dispose ();
				btnMASSearch = null;
			}

			if (btnMTBuildFinalDescrip != null) {
				btnMTBuildFinalDescrip.Dispose ();
				btnMTBuildFinalDescrip = null;
			}

			if (btnMTEnqueue != null) {
				btnMTEnqueue.Dispose ();
				btnMTEnqueue = null;
			}

			if (btnOpenInBrowser != null) {
				btnOpenInBrowser.Dispose ();
				btnOpenInBrowser = null;
			}

			if (cmbMASResults != null) {
				cmbMASResults.Dispose ();
				cmbMASResults = null;
			}

			if (cmbMTAppType != null) {
				cmbMTAppType.Dispose ();
				cmbMTAppType = null;
			}

			if (cmbMTOsxVersion != null) {
				cmbMTOsxVersion.Dispose ();
				cmbMTOsxVersion = null;
			}

			if (lstPrepareLog != null) {
				lstPrepareLog.Dispose ();
				lstPrepareLog = null;
			}

			if (lstQueue != null) {
				lstQueue.Dispose ();
				lstQueue = null;
			}

			if (txtMasSearchString != null) {
				txtMasSearchString.Dispose ();
				txtMasSearchString = null;
			}

			if (txtMTDescrip != null) {
				txtMTDescrip.Dispose ();
				txtMTDescrip = null;
			}

			if (txtMTFinalDescrip != null) {
				txtMTFinalDescrip.Dispose ();
				txtMTFinalDescrip = null;
			}

			if (txtMTImageUrl != null) {
				txtMTImageUrl.Dispose ();
				txtMTImageUrl = null;
			}

			if (txtMTNfoFile != null) {
				txtMTNfoFile.Dispose ();
				txtMTNfoFile = null;
			}

			if (txtMTTags != null) {
				txtMTTags.Dispose ();
				txtMTTags = null;
			}

			if (txtMTTitle != null) {
				txtMTTitle.Dispose ();
				txtMTTitle = null;
			}

			if (txtReleaseGroup != null) {
				txtReleaseGroup.Dispose ();
				txtReleaseGroup = null;
			}

			if (txtReleaseMulti != null) {
				txtReleaseMulti.Dispose ();
				txtReleaseMulti = null;
			}

			if (txtReleaseName != null) {
				txtReleaseName.Dispose ();
				txtReleaseName = null;
			}

			if (txtReleaseRegType != null) {
				txtReleaseRegType.Dispose ();
				txtReleaseRegType = null;
			}

			if (txtReleaseTorrentFile != null) {
				txtReleaseTorrentFile.Dispose ();
				txtReleaseTorrentFile = null;
			}

			if (txtReleaseVersion != null) {
				txtReleaseVersion.Dispose ();
				txtReleaseVersion = null;
			}
		}
	}

	[Register ("MainWindow")]
	partial class MainWindow
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
