class ResolvedCompilationUnit extends ResolvedElement {
  List<ResolvedClassElement> enums;
  List<ResolvedClassElement> types;
  List<ResolvedPropertyAccessorElement> accessors;
  List<ResolvedFunctionElement> functions;
}

class ResolvedElement {
  String displayName;
  String documentationComment;
  bool hasAlwaysThrows;
  bool hasDeprecated;
  bool hasFactory;
  bool hasIsTest;
  bool hasIsTestGroup;
  bool hasJS;
  bool hasOverride;
  bool hasProtected;
  bool hasRequired;
  bool hasSealed;
  bool hasVisibleForTemplate;
  bool hasVisibleForTesting;
  int id;
  bool isPrivate;
  bool isPublic;
  bool isSynthetic;
  String name;
}

class ResolvedMethodElement extends ResolvedExecutableElement {
  ResolvedMethodDeclaration methodDeclaration;
}

class ResolvedTypeDefiningElement extends ResolvedElement {
  ResolvedDartType type;
}

class ResolvedExecutableElement extends ResolvedFunctionTypeElement {
  bool hasImplicitReturnType;
  bool isAbstract;
  bool isAsynchronous;
  bool isExternal;
  bool isGenerator;
  bool isOperator;
  bool isStatic;
  bool isSynchronous;
}

class ResolvedFunctionTypeElement extends ResolvedElement {
  List<ResolvedParameterElement> parameters;

  ResolvedDartType returnType;

  ResolvedFunctionType type;
}

class ResolvedClassElement extends ResolvedTypeDefiningElement {
  bool isAbstract;
  bool isEnum;
  bool isMixin;
  bool isMixinApplication;
  bool isOrInheritsProxy;
  bool isProxy;
  bool isValidMixin;
  List<ResolvedDartType> mixins;
  List<ResolvedMethodElement> methods;
  bool hasNonFinalField;
  bool hasReferenceToSuper;
  bool hasStaticMember;

  List<ResolvedPropertyAccessorElement> accessors;
  ResolvedInterfaceType supertype;
  List<ResolvedConstructorElement> constructors;
  List<ResolvedInterfaceType> allSupertypes;
  List<ResolvedInterfaceType> superclassConstraints;

  List<ResolvedFieldElement> fields;
}

class ResolvedFieldElement extends ResolvedPropertyInducingElement {
  bool isEnumConstant;
}

class ResolvedPropertyInducingElement extends ResolvedVariableElement {
  ResolvedPropertyAccessorElement getter;
  ResolvedPropertyAccessorElement setter;
}

class ResolvedPropertyAccessorElement extends ResolvedExecutableElement {
  bool isGetter;
  bool isSetter;
}

class ResolvedVariableElement {
  Object constantValue;
  bool hasImplicitType;
  ResolvedFunctionElement initializer;
  bool isConst;
  bool isFinal;
  bool isStatic;
  ResolvedDartType type;
}

class ResolvedDartType {
  String displayName;

  bool isBottom;
  bool isDartAsyncFuture;
  bool isDartAsyncFutureOr;
  bool isDartCoreFunction;
  bool isDartCoreNull;
  bool isDynamic;
  bool isObject;
  bool isUndefined;
  bool isVoid;
}

class ResolvedFunctionType extends ResolvedDartType {}

class ResolvedParameterElement extends ResolvedElement {
  bool isCovariant;
  bool isInitializingFormal;
  bool isNamed;
  bool isNotOptional;
  bool isOptional;
  bool isOptionalPositional;
  bool isPositional;
}

class ResolvedFunctionElement extends ResolvedExecutableElement {
  bool isEntryPoint;
}

class ResolvedParamaterizedType extends ResolvedDartType {
  List<ResolvedDartType> typeArguments;
  List<ResolvedTypeParameterElement> typeParameters;
}

class ResolvedTypeParameterElement extends ResolvedTypeDefiningElement {
  ResolvedDartType bound;
}

class ResolvedTypeParameterType extends ResolvedDartType {
  ResolvedDartType type;
}

class ResolvedConstructorElement extends ResolvedExecutableElement {
  bool isConst;
  bool isDefaultConstructor;
  bool isFactory;
  bool isStatic;
}

class ResolvedInterfaceType extends ResolvedParamaterizedType {
  List<ResolvedPropertyAccessorElement> accessors;
  List<ResolvedConstructorElement> constructors;
  //ResolvedClassElement element;
  List<ResolvedInterfaceType> interfaces;
  List<ResolvedMethodElement> methods;
  List<ResolvedInterfaceType> mixins;
  ResolvedInterfaceType superclass;
  List<ResolvedInterfaceType> superclassConstraints;
}

class ResolvedMethodDeclaration {
  ResolvedFunctionBody body;
}

class ResolvedFunctionBody extends ResolvedAstNode {

}

class ResolvedAstNode
{
   ResolvedToken beginToken;
}

class ResolvedToken
{
  ResolvedToken next;
  int kind;
  String lexeme;
  bool isKeyword;
  bool isKeywordOrIdentifier;
  bool isModifier;
  bool isOperator;
}