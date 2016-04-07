using System.Reflection;
using System.Runtime.CompilerServices;
using Android.App;

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

[assembly: AssemblyVersion ("1.0.0")]

// The following attributes are used to specify the signing key for the assembly,
// if desired. See the Mono documentation for more information about signing.

//[assembly: AssemblyDelaySign(false)]
//[assembly: AssemblyKeyFile("")]

#if DEBUG
[assembly: Application(Debuggable=true)]
#else
[assembly: Application(Debuggable=false)]
#endif

