
// This file was automatically generated.  DO NOT EDIT or else
// your changes could be lost!

#pragma warning disable 1570

using System;
using System.Globalization;
using System.Resources;
using System.Reflection;

namespace Hxlc.Resources {

    /// <summary>
    /// Contains strongly-typed string resources.
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("srgen", "1.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
    internal static partial class SR {

        private static global::System.Resources.ResourceManager _resources;
        private static global::System.Globalization.CultureInfo _currentCulture;
        private static global::System.Func<string, string> _resourceFinder;

        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(_resources, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Hxlc.Resources.SR", typeof(SR).GetTypeInfo().Assembly);
                    _resources = temp;
                }
                return _resources;
            }
        }

        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return _currentCulture;
            }
            set {
                _currentCulture = value;
            }
        }

        private static global::System.Func<string, string> ResourceFinder {
            get {
                if (object.ReferenceEquals(_resourceFinder, null)) {
                    try {
                        global::System.Resources.ResourceManager rm = ResourceManager;
                        _resourceFinder = delegate (string s) {
                            return rm.GetString(s);
                        };
                    } catch (global::System.Exception ex) {
                        _resourceFinder = delegate (string s) {
                            return string.Format("localization error! {0}: {1} ({2})", s, ex.GetType(), ex.Message);
                        };
                    }
                }
                return _resourceFinder;
            }
        }


  /// <summary>Use the given base type for generated templates</summary>
    internal static string UBaseType(

    ) {
        return string.Format(Culture, ResourceFinder("UBaseType") );
    }

  /// <summary>Display additional debug information</summary>
    internal static string UDebug(

    ) {
        return string.Format(Culture, ResourceFinder("UDebug") );
    }

  /// <summary>Define a property value</summary>
    internal static string UDefine(

    ) {
        return string.Format(Culture, ResourceFinder("UDefine") );
    }

  /// <summary>Display this help screen</summary>
    internal static string UHelp(

    ) {
        return string.Format(Culture, ResourceFinder("UHelp") );
    }

  /// <summary>Include all files matching the given pattern, or if a directory, all source files within the directory and its subdirectories</summary>
    internal static string UInclude(

    ) {
        return string.Format(Culture, ResourceFinder("UInclude") );
    }

  /// <summary>Map an XML prefix to a namespace when resolving types</summary>
    internal static string UNamespace(

    ) {
        return string.Format(Culture, ResourceFinder("UNamespace") );
    }

  /// <summary>Don't compile an assembly - just generate source files</summary>
    internal static string UNoCompile(

    ) {
        return string.Format(Culture, ResourceFinder("UNoCompile") );
    }

  /// <summary>Suppress the copyright information</summary>
    internal static string UNoLogo(

    ) {
        return string.Format(Culture, ResourceFinder("UNoLogo") );
    }

  /// <summary>An output path where the assembly file will be written -or- the directory if --no-compile is specified</summary>
    internal static string UOut(

    ) {
        return string.Format(Culture, ResourceFinder("UOut") );
    }

  /// <summary>Reference metadata from the assembly file</summary>
    internal static string UReference(

    ) {
        return string.Format(Culture, ResourceFinder("UReference") );
    }

  /// <summary>Specify verbose output</summary>
    internal static string UVerbose(

    ) {
        return string.Format(Culture, ResourceFinder("UVerbose") );
    }

  /// <summary>Specify verbose output (more detailed)</summary>
    internal static string UVerbose2(

    ) {
        return string.Format(Culture, ResourceFinder("UVerbose2") );
    }

  /// <summary>Specify verbose output (most detailed)</summary>
    internal static string UVerbose3(

    ) {
        return string.Format(Culture, ResourceFinder("UVerbose3") );
    }

  /// <summary>Display the extended version and build information</summary>
    internal static string UVersion(

    ) {
        return string.Format(Culture, ResourceFinder("UVersion") );
    }

    }
}
