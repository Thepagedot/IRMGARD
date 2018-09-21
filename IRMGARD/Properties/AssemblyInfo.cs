using System.Reflection;
using Android;
using Android.App;
using Google.Android.Vending.Licensing;

// Information about this assembly is defined by the following attributes.
// Change them to the values specific to your project.

[assembly: AssemblyTitle ("IRMGARD")]
[assembly: AssemblyDescription ("Hilft Erwachsenen Lesen und Schreiben zu lernen")]
[assembly: AssemblyConfiguration ("")]
[assembly: AssemblyCompany ("KOPF, HAND + FUSS gGmbH")]
[assembly: AssemblyProduct ("IRMGARD-App")]
[assembly: AssemblyCopyright ("© 2018 KOPF, HAND + FUSS")]
[assembly: AssemblyTrademark ("")]
[assembly: AssemblyCulture ("")]

// The assembly version has the format "{Major}.{Minor}.{Build}.{Revision}".
// The form "{Major}.{Minor}.*" will automatically update the build and revision,
// and "{Major}.{Minor}.{Build}.*" will update just the revision.

// TODO Release: Update AssemblyVersion
[assembly: AssemblyVersion ("1.5.1")]

// The following attributes are used to specify the signing key for the assembly,
// if desired. See the Mono documentation for more information about signing.

//[assembly: AssemblyDelaySign(false)]
//[assembly: AssemblyKeyFile("")]

#if DEBUG
[assembly: Application(Debuggable=true)]
#else
[assembly: Application(Debuggable=false)]
#endif

// Required to access Google Play Licensing (com.android.vending.CHECK_LICENSE)
[assembly: UsesPermission(LicenseChecker.Manifest.Permission.CheckLicense)]
// Required to download files from Google Play
[assembly: UsesPermission(Manifest.Permission.Internet)]
// Required to keep CPU alive while downloading files(NOT to keep screen awake)
[assembly: UsesPermission(Manifest.Permission.WakeLock)]
// Required to poll the state of the network connection and respond to changes
[assembly: UsesPermission(Manifest.Permission.AccessNetworkState)]
// Required to check whether Wi-Fi is enabled
[assembly: UsesPermission(Manifest.Permission.AccessWifiState)]
// Required to read and write the expansion files on shared storage
[assembly: UsesPermission(Manifest.Permission.ReadExternalStorage)]
[assembly: UsesPermission(Manifest.Permission.WriteExternalStorage)]
