using System;
using System.Collections.Generic;

namespace Dart2CSharpTranspiler.Dart
{
    public class DartModel : Dictionary<string, IList<CompilationUnit>> { }

    public class CompilationUnit : Element
    {
        public List<ClassElement> enums;
        public List<ClassElement> types;
        public List<PropertyAccessorElement> accessors;
        public List<FunctionElement> functions;
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

    public class MethodElement : ExecutableElement
    {
        public MethodDeclaration methodDeclaration;
    }

    public class TypeDefiningElement : Element
    {
        public DartType type;
    }

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

        public List<PropertyAccessorElement> accessors;
        public InterfaceType supertype;
        public List<ConstructorElement> constructors;
        public List<InterfaceType> allSupertypes;
        public List<InterfaceType> superclassConstraints;

        public List<FieldElement> fields;
    }

    public class FieldElement : PropertyInducingElement
    {
        public bool? isEnumConstant;
    }

    public class PropertyInducingElement : VariableElement
    {
        public PropertyAccessorElement getter;
        public PropertyAccessorElement setter;
    }

    public class PropertyAccessorElement : ExecutableElement
    {
        public bool? isGetter;
        public bool? isSetter;
    }

    public class VariableElement
    {
        public Object constantValue;
        public bool? hasImplicitType;
        public FunctionElement initializer;
        public bool? isConst;
        public bool? isFinal;
        public bool? isStatic;
        public DartType type;
    }

    public class DartType
    {
        public String displayName;

        public bool? isBottom;
        public bool? isDartAsyncFuture;
        public bool? isDartAsyncFutureOr;
        public bool? isDartCoreFunction;
        public bool? isDartCoreNull;
        public bool? isDynamic;
        public bool? isObject;
        public bool? isUndefined;
        public bool? isVoid;
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

    public class FunctionElement : ExecutableElement
    {
        public bool? isEntryPoint;
    }

    public class ParamaterizedType : DartType
    {
        public List<DartType> typeArguments;
        public List<TypeParameterElement> typeParameters;
    }

    public class TypeParameterElement : TypeDefiningElement
    {
        public DartType bound;
    }

    public class TypeParameterType : DartType
    {
        public DartType type;
    }

    public class ConstructorElement : ExecutableElement
    {
        public bool? isConst;
        public bool? isDefaultConstructor;
        public bool? isFactory;
    }

    public class InterfaceType : ParamaterizedType
    {
        public List<PropertyAccessorElement> accessors;
        public List<ConstructorElement> constructors;
        //ClassElement element;
        public List<InterfaceType> interfaces;
        public List<MethodElement> methods;
        public List<InterfaceType> mixins;
        public InterfaceType superclass;
        public List<InterfaceType> superclassConstraints;
    }

    public class MethodDeclaration
    {
        public FunctionBody body;
    }

    public class FunctionBody : AstNode
    {

    }

    public class AstNode
    {
        public Token beginToken;
    }

    public class Token
    {
        public Token next;
        public int kind;
        public String lexeme;
        public bool? isKeyword;
        public bool? isKeywordOrIdentifier;
        public bool? isModifier;
        public bool? isOperator;
    }
}