// WARNING
// This file has been generated automatically by Xamarin Studio to
// mirror C# types. Changes in this file made by drag-connecting
// from the UI designer will be synchronized back to C#, but
// more complex manual changes may not transfer correctly.


#import <Foundation/Foundation.h>
#import <AppKit/AppKit.h>


@interface MainWindowController : NSWindowController {
	NSButton *_btnMASSearch;
	NSPopUpButton *_cmbMASResults;
	NSScrollView *_lstPrepareLog;
	NSTextField *_txtMasSearchString;
	NSTextField *_txtMTTitle;
	NSTextField *_txtReleaseGroup;
	NSTextField *_txtReleaseMulti;
	NSTextField *_txtReleaseName;
	NSTextField *_txtReleaseRegType;
	NSTextField *_txtReleaseTorrentFile;
	NSTextField *_txtReleaseVersion;
    NSButton *_btnOpenInBrowser;
}
@property (assign) IBOutlet NSButton *btnOpenInBrowser;

@property (nonatomic, retain) IBOutlet NSButton *btnMASSearch;

@property (nonatomic, retain) IBOutlet NSPopUpButton *cmbMASResults;

@property (nonatomic, retain) IBOutlet NSScrollView *lstPrepareLog;

@property (nonatomic, retain) IBOutlet NSTextField *txtMasSearchString;

@property (nonatomic, retain) IBOutlet NSTextField *txtMTTitle;

@property (nonatomic, retain) IBOutlet NSTextField *txtReleaseGroup;

@property (nonatomic, retain) IBOutlet NSTextField *txtReleaseMulti;

@property (nonatomic, retain) IBOutlet NSTextField *txtReleaseName;

@property (nonatomic, retain) IBOutlet NSTextField *txtReleaseRegType;

@property (nonatomic, retain) IBOutlet NSTextField *txtReleaseTorrentFile;

@property (nonatomic, retain) IBOutlet NSTextField *txtReleaseVersion;

@end
