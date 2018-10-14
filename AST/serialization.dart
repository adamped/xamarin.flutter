// MIT License

// Copyright (c) 2016 Tobe O
// https://github.com/thosakwe/json_god
// Modified 2018 Adam Pedley

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

import 'dart:mirrors';
import 'dart:convert';
import 'dart:collection';

const Symbol hashCodeSymbol = #hashCode;
const Symbol runtimeTypeSymbol = #runtimeType;

typedef Serializer(value, {bool primitiveOnly});

List<Symbol> _findGetters(ClassMirror classMirror) {
  List<Symbol> result = [];

  classMirror.instanceMembers
      .forEach((Symbol symbol, MethodMirror methodMirror) {
    if (methodMirror.isGetter &&
        !methodMirror.isPrivate &&
        symbol != hashCodeSymbol &&
        symbol != runtimeTypeSymbol) {
      result.add(symbol);
    }
  });

  return result;
}

serialize(value, Serializer serializer, {bool primitiveOnly = false}) {
  Map result = {};
  InstanceMirror instanceMirror = reflect(value);
  ClassMirror classMirror = instanceMirror.type;

  for (Symbol symbol in _findGetters(classMirror)) {
    String name = MirrorSystem.getName(symbol);
    var field = instanceMirror.getField(symbol);
    var valueForSymbol = field.reflectee;
    try {
      result[name] = serializer(valueForSymbol, primitiveOnly: primitiveOnly);
    } catch (e, st) {}
  }

  return result;
}

bool _isPrimitive(value) {
  return value is num || value is bool || value is String || value == null;
}

/// Serializes any arbitrary Dart datum to JSON. Supports schema validation.
String serializeModel(value) {
  var serialized = serializeObject(value);
  // Reset list now that object is done
  objects = [];
  return json.encode(serialized);
}

/// Transforms any Dart datum into a value acceptable to json.encode.
serializeObject(value, {bool primitiveOnly = false}) {
  if (_isPrimitive(value)) {
    return value;
  } else if (value is DateTime) {
    return value.toIso8601String();
  } else if (value is Iterable) {
    return value.map(serializeObject).toList();
  } else if (value is LinkedHashMap) {
    return serializeMap(value);
  } else if (value is Map) {
    return serializeMap(value);
  } else if (!primitiveOnly) {
    return serializeNonPrimitiveObject(value);
  } else {
    return null; // Last resort
  }
}

List<Object> objects = [];

serializeNonPrimitiveObject(value) {
  if (objects.contains(value))
    return serializeObject(
        serialize(value, serializeObject, primitiveOnly: true),
        primitiveOnly: true);
  else {
    objects.add(value);
    return serializeObject(serialize(value, serializeObject));
  }
}

/// Recursively transforms a Map and its children into JSON-serializable data.
Map serializeMap(Map value) {
  Map outputMap = {};
  value.forEach((key, value) {
    if (key is String) outputMap[key] = serializeObject(value);
  });
  return outputMap;
}
