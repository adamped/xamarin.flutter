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

import 'package:logging/logging.dart';
import 'dart:mirrors';
import 'dart:convert';
import 'dart:collection';

const Symbol hashCodeSymbol = #hashCode;
const Symbol runtimeTypeSymbol = #runtimeType;

typedef Serializer(value);

List<Symbol> _findGetters(ClassMirror classMirror) {
  List<Symbol> result = [];

  classMirror.instanceMembers
      .forEach((Symbol symbol, MethodMirror methodMirror) {
    if (methodMirror.isGetter &&
        symbol != hashCodeSymbol &&
        symbol != runtimeTypeSymbol) {
      //logger.info("Found getter on instance: $symbol");
      result.add(symbol);
    }
  });

  return result;
}

serialize(value, Serializer serializer, [@deprecated bool debug = false]) {
  //logger.info("Serializing this value via reflection: $value");
  Map result = {};
  InstanceMirror instanceMirror = reflect(value);
  ClassMirror classMirror = instanceMirror.type;

  // Check for toJson
  for (Symbol symbol in classMirror.instanceMembers.keys) {
    if (symbol == #toJson) {
      //logger.info("Running toJson...");
      var result = instanceMirror.invoke(symbol, []).reflectee;
      //logger.info("Result of serialization via reflection: $result");
      return result;
    }
  }

  for (Symbol symbol in _findGetters(classMirror)) {
    String name = MirrorSystem.getName(symbol);
    var valueForSymbol = instanceMirror.getField(symbol).reflectee;
    if (!name.startsWith('_')) {
      try {
        result[name] = serializer(valueForSymbol);
        //logger.info("Set $name to $valueForSymbol");
      } catch (e, st) {
        //logger.info("Could not set $name");
      }
    }
  }

  //logger.info("Result of serialization via reflection: $result");

  return result;
}

bool _isPrimitive(value) {
  return value is num || value is bool || value is String || value == null;
}

/// Serializes any arbitrary Dart datum to JSON. Supports schema validation.
String serializeModel(value) {
  var serialized = serializeObject(value);
  //logger.info('Serialization result: $serialized');
  objects = [];
  return json.encode(serialized);
}

List<dynamic> objects = [];

/// Transforms any Dart datum into a value acceptable to json.encode.
serializeObject(value) {
  if (_isPrimitive(value)) {
    //logger.info("Serializing primitive value: $value");
    return value;
  } else if (value is DateTime) {
    //logger.info("Serializing this DateTime: $value");
    return value.toIso8601String();
  } else if (value is Iterable) {
    //logger.info("Serializing this Iterable: $value");
    return value.map(serializeObject).toList();
  } else if (value is LinkedHashMap) {
    return serializeMap(value);
  } else if (value is Map) {
    //logger.info("Serializing this Map: $value");
    return serializeMap(value);
  } else {
    return serializeNonPrimitiveObject(value);
  }
}

serializeNonPrimitiveObject(value) {
  if (objects.contains(value))
    return "";
  else {
    objects.add(value);
    return serializeObject(serialize(value, serializeObject));
  }
}

/// Recursively transforms a Map and its children into JSON-serializable data.
Map serializeMap(Map value) {
  Map outputMap = {};
  value.forEach((key, value) {
    if (key is String)
      outputMap[key] = serializeObject(value);
    else
      outputMap[key.toString()] = serializeObject(value);
  });
  return outputMap;
}

//final Logger logger = new Logger('json_god');
