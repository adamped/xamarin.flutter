class ResolvedCompilationUnit extends ResolvedElement {
  List<ResolvedClassElement> enums;
  List<ResolvedClassElement> types;
  List<ResolvedPropertyAccessorElement> accessors;
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

class ResolvedMethodElement extends ResolvedExecutableElement {}

class ResolvedTypeDefiningElement extends ResolvedElement {}

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
}

class ResolvedDartType {
  String displayName;
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

class ResolvedPropertyAccessorElement extends ResolvedExecutableElement {
  bool isGetter;
  bool isSetter;
}
