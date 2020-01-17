
// This file was automatically generated.  DO NOT EDIT or else
// your changes could be lost!

#pragma warning disable 1570

using System;
using System.Globalization;
using System.Resources;
using System.Reflection;

namespace Carbonfrost.Commons.Hxl.Resources {

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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Carbonfrost.Commons.Hxl.Automation.SR", typeof(SR).GetTypeInfo().Assembly);
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


  /// <summary>Assembly is loaded into the reflection-only context and cannot be used.</summary>
    internal static string AssemblyIsReflectionOnly(
    
    ) {
        return string.Format(Culture, ResourceFinder("AssemblyIsReflectionOnly") );
    }

  /// <summary>Attribute `${name}': Could not convert from text or invalid.</summary>
    internal static string CannotCreateAttributeOnConversion(
    object @name
    ) {
        return string.Format(Culture, ResourceFinder("CannotCreateAttributeOnConversion") , @name);
    }

  /// <summary>Cannot find a matching template: `${name}' (${type})</summary>
    internal static string CannotFindMatchingTemplate(
    object @name, object @type
    ) {
        return string.Format(Culture, ResourceFinder("CannotFindMatchingTemplate") , @name, @type);
    }

  /// <summary>Template context does not support loading masters.</summary>
    internal static string CannotLoadAnyMasterTemplates(
    
    ) {
        return string.Format(Culture, ResourceFinder("CannotLoadAnyMasterTemplates") );
    }

  /// <summary>Cannot load master template with name `${name}'.</summary>
    internal static string CannotLoadMasterTemplate(
    object @name
    ) {
        return string.Format(Culture, ResourceFinder("CannotLoadMasterTemplate") , @name);
    }

  /// <summary>Cannot specify the name argument with the given position type.</summary>
    internal static string CannotSpecifyNameArgumentWithPosition(
    
    ) {
        return string.Format(Culture, ResourceFinder("CannotSpecifyNameArgumentWithPosition") );
    }

  /// <summary>Prefix cannot be one of the built-in prefixes.</summary>
    internal static string CannotUseBuiltinPrefixes(
    
    ) {
        return string.Format(Culture, ResourceFinder("CannotUseBuiltinPrefixes") );
    }

  /// <summary>Compilation of source code failed with errors, which have been saved to `${fileName}'.</summary>
    internal static string CompiledWithErrors(
    object @fileName
    ) {
        return string.Format(Culture, ResourceFinder("CompiledWithErrors") , @fileName);
    }

  /// <summary>Directive not defined: ${name}</summary>
    internal static string DirectiveNotDefined(
    object @name
    ) {
        return string.Format(Culture, ResourceFinder("DirectiveNotDefined") , @name);
    }

  /// <summary>Parse failure on server element (${type).</summary>
    internal static string FailedToReadServerElement(
    
    ) {
        return string.Format(Culture, ResourceFinder("FailedToReadServerElement") );
    }

  /// <summary>Invalid syntax for a directive.</summary>
    internal static string InvalidDirective(
    
    ) {
        return string.Format(Culture, ResourceFinder("InvalidDirective") );
    }

  /// <summary>Can't write output to text at this time.</summary>
    internal static string InvalidToWriteTextOutput(
    
    ) {
        return string.Format(Culture, ResourceFinder("InvalidToWriteTextOutput") );
    }

  /// <summary>Line ${line}, pos ${pos}</summary>
    internal static string LineInfo(
    object @line, object @pos
    ) {
        return string.Format(Culture, ResourceFinder("LineInfo") , @line, @pos);
    }

  /// <summary>Name argument must be specified with the given position type.</summary>
    internal static string MustSpecifyNameArgumentWithPosition(
    
    ) {
        return string.Format(Culture, ResourceFinder("MustSpecifyNameArgumentWithPosition") );
    }

  /// <summary>No server attribute could be found for `${fullName}'</summary>
    internal static string NoMatchingServerAttribute(
    object @fullName
    ) {
        return string.Format(Culture, ResourceFinder("NoMatchingServerAttribute") , @fullName);
    }

  /// <summary>No server element could be found for `${fullName}'</summary>
    internal static string NoMatchingServerElement(
    object @fullName
    ) {
        return string.Format(Culture, ResourceFinder("NoMatchingServerElement") , @fullName);
    }

  /// <summary>Not a valid name for a variable: ${varName}</summary>
    internal static string NotValidVariableName(
    object @varName
    ) {
        return string.Format(Culture, ResourceFinder("NotValidVariableName") , @varName);
    }

  /// <summary>Prefix has already been defined: ${prefix}</summary>
    internal static string PrefixAlreadyDefined(
    object @prefix
    ) {
        return string.Format(Culture, ResourceFinder("PrefixAlreadyDefined") , @prefix);
    }

  /// <summary>Prefix does not map to a namespace: ${prefix}</summary>
    internal static string PrefixDoesNotMapToNamespace(
    object @prefix
    ) {
        return string.Format(Culture, ResourceFinder("PrefixDoesNotMapToNamespace") , @prefix);
    }

  /// <summary>Attribute `${name}': No type could be found for the server attribute.</summary>
    internal static string ServerAttributeCannotBeCreated(
    object @name
    ) {
        return string.Format(Culture, ResourceFinder("ServerAttributeCannotBeCreated") , @name);
    }

  /// <summary>Server attribute `${type}' does not define a matching property `${property}'.</summary>
    internal static string ServerAttributePropertyNotFound(
    object @type, object @property
    ) {
        return string.Format(Culture, ResourceFinder("ServerAttributePropertyNotFound") , @type, @property);
    }

  /// <summary>Element `${name}': No type could be found for the server element.</summary>
    internal static string ServerElementCannotBeCreated(
    object @name
    ) {
        return string.Format(Culture, ResourceFinder("ServerElementCannotBeCreated") , @name);
    }

    }
}
