using System;
using System.Collections.Generic;

namespace Dart2CSharpTranspiler.Dart
{
    public class DartModel : Dictionary<string, IList<CompilationUnit>> { }

    public class CompilationUnit : Element
    {
        public List<ClassElement> enums;
        public List<ClassElement> types;
    }

    public class Element
    {
        public String displayName;
        public String documentationComment;
        public bool? hasAlwaysThrows;
        public bool? hasDeprecated;
        public bool? hasFactory;
        public bool? hasIsTest;
        public bool? hasIsTestGroup;
        public bool? hasJS;
        public bool? hasOverride;
        public bool? hasProtected;
        public bool? hasRequired;
        public bool? hasSealed;
        public bool? hasVisibleForTemplate;
        public bool? hasVisibleForTesting;
        public int? id;
        public bool? isPrivate;
        public bool? isPublic;
        public bool? isSynthetic;
        public String name;
    }

    public class MethodElement : ExecutableElement { }

    public class TypeDefiningElement : Element { }

    public class ExecutableElement : FunctionTypeElement
    {
        public bool? hasImplicitReturnType;
        public bool? isAbstract;
        public bool? isAsynchronous;
        public bool? isExternal;
        public bool? isGenerator;
        public bool? isOperator;
        public bool? isStatic;
        public bool? isSynchronous;
    }

    public class FunctionTypeElement : Element
    {
        public List<ParameterElement> parameters;

        public DartType returnType;

        public FunctionType type;
    }

    public class ClassElement : TypeDefiningElement
    {
        public bool? isAbstract;
        public bool? isEnum;
        public bool? isMixin;
        public bool? isMixinApplication;
        public bool? isOrInheritsProxy;
        public bool? isProxy;
        public bool? isValidMixin;
        public List<DartType> mixins;
        public List<MethodElement> methods;
        public bool? hasNonFinalField;
        public bool? hasReferenceToSuper;
        public bool? hasStaticMember;
    }

    public class DartType
    {
        public String displayName;
    }

    public class FunctionType : DartType { }

    public class ParameterElement : Element
    {
        public bool? isCovariant;
        public bool? isInitializingFormal;
        public bool? isNamed;
        public bool? isNotOptional;
        public bool? isOptional;
        public bool? isOptionalPositional;
        public bool? isPositional;
    }
}
