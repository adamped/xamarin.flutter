using System;
using System.Collections.Generic;

namespace Dart2CSharpTranspiler.Dart
{
    public class DartModel : Dictionary<string, IList<DartFile>> { }

    [Flags]
	public enum VisibilityType
	{
		Public = 1,
		Private = 2
	}

	public class DartFile
	{
		public string Folder { get; set; }
		public string Name { get; set; }
		public IList<DartClass> Classes { get; set; }
		public IList<DartImport> Imports { get; set; }
		public IList<Section> Sections { get; set; }
	}

	public enum ImportType
	{
		Package,
		Dart,
		File
	}

	public class DartImport
	{
		public string Alias { get; set; }
		public ImportType Type { get; set; }
		public string Name { get; set; }
		public string Raw { get; set; }
		public string Complete { get; set; }
		public IList<string> ScopedVariables { get; set; } = new List<string>();
	}

	public class Property
	{
		public string Name { get; set; }
		public string GetRawCode { get; set; }
		public string SetRawCode { get; set; }
		public string ReturnType { get; set; }
		public VisibilityType Visibility { get; set; }
	}

	public class DartClass
	{
		public string Raw { get; set; }
		public bool IsAbstract { get; set; } = false;
		public bool IsImmutable { get; set; } = false;
		public IDictionary<string, string> GenericConstraints { get; set; } // e.g. Animation<T extends num>
		public VisibilityType VisibilityType { get; set; }
		public string Filename { get; set; }
		public string RawName { get; set; }
		public string Name { get; set; }
		public Extend Extends { get; set; }
		public List<string> Implements { get; set; }
		public List<Section> Sections { get; set; }
		public List<Property> Properties { get; set; } = new List<Property>();
	}

	public class Extend
	{
		public string Name { get; set; }
		public List<string> Mixins { get; set; }
	}

	public class Section
	{
		public string Raw { get; set; }
		public string Name { get; set; }
		public string DeprecatedMessage { get; set; }
		public string ReturnType { get; set; }
		public string RawCode { get; set; }
		public string ConstructorRawCode { get; set; }
		public bool NoBody { get; set; } = false;
		public bool IsAbstract { get; set; } = false;
		public bool IsConstant { get; set; } = false;
		public bool IsStatic { get; set; } = false;
		public bool IsFactory { get; set; } = false;
		public bool IsFinal { get; set; } = false;
		public MethodAttribute Attribute { get; set; }
		public SectionType Type { get; set; }
		public bool IsPropertyBacking { get; set; }
		public List<Parameter> Parameters { get; set; }
		public VisibilityType VisibilityType { get; set; }
		public string RawParameter { get; set; }
		public List<Parameter> SuperCall { get; set; }

		// ** These are actual C# based properties 
		//    Calculated in post-processing        **//
		public bool IsOverride { get; set; }
		public bool IsVirtual { get; set; }
	}

	public class Parameter
	{
		public string Name { get; set; }
		public string Value { get; set; }
		public string Type { get; set; }
		/// <summary>
		/// This means the parameters were inside the { ... }
		/// </summary>
		public bool IsOptional { get; set; }
		/// <summary>
		/// Either a normal parameter, or is flagged with @required in optional parameter list
		/// </summary>
		public bool IsRequired { get; set; }
		public bool IsAutoAssign { get; set; }
		public string DefaultValue { get; set; }
		public bool IsCovariant { get; set; }
		public bool IsDeprecated { get; set; }
		public bool IsFinal { get; set; }
		public bool IsValue { get; set; }
	}

	public enum SectionType
	{
		PropertyGet,
		PropertySet,
		Method,
		Constructor,
		Field,
		Operator,
		Typedef,
		Enum,
		Export
	}


	[Flags]
	public enum MethodAttribute
	{
		Override = 1,
		Protected = 2,
		Immutable = 4,
		Deprecated = 8
	}
}
