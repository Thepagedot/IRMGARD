using System.Reflection;
using Android;
using Android.App;
using LicenseVerificationLibrary;

// Information about this assembly is defined by the following attributes.
// Change them to the values specific to your project.

[assembly: AssemblyTitle ("IRMGARD")]
[assembly: AssemblyDescription ("Hilft Erwachsenen Lesen und Schreiben zu lernen")]
[assembly: AssemblyConfiguration ("")]
[assembly: AssemblyCompany ("Kopf, Hand und Fuss gGmbh")]
[assembly: AssemblyProduct ("IRMGARD-App")]
[assembly: AssemblyCopyright ("© 2016 KOPF, HAND und FUSS")]
[assembly: AssemblyTrademark ("")]
[assembly: AssemblyCulture ("")]

// The assembly version has the format "{Major}.{Minor}.{Build}.{Revision}".
// The form "{Major}.{Minor}.*" will automatically update the build and revision,
// and "{Major}.{Minor}.{Build}.*" will update just the revision.

// TODO Release: Update AssemblyVersion
[assembly: AssemblyVersion ("1.1.0")]

// The following attributes are used to specify the signing key for the assembly,
// if desired. See the Mono documentation for more information about signing.

//[assembly: AssemblyDelaySign(false)]
//[assembly: AssemblyKeyFile("")]

#if DEBUG
[assembly: Application(Debuggable=true)]
#else
[assembly: Application(Debuggable=false)]
#endif

// Add some common permissions, these can be removed if not needed
[assembly: UsesPermission(Manifest.Permission.Internet)]
[assembly: UsesPermission(Manifest.Permission.WriteExternalStorage)]
[assembly: UsesPermission(Manifest.Permission.AccessNetworkState)]
[assembly: UsesPermission(Manifest.Permission.WakeLock)]
[assembly: UsesPermission(Manifest.Permission.AccessWifiState)]
[assembly: UsesPermission(LicenseChecker.Manifest.Permission.CheckLicense)]
