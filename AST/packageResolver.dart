import 'package:analyzer/file_system/file_system.dart' as file_system;
import 'package:analyzer/file_system/file_system.dart';
import 'package:analyzer/source/embedder.dart';
import 'package:analyzer/source/package_map_resolver.dart';
import 'package:yaml/yaml.dart';
import 'package:package_config/packages.dart' show Packages;
import 'package:package_config/packages_file.dart' as pkgfile show parse;
import 'package:package_config/src/packages_impl.dart' show MapPackages;
import 'dart:io' as io;
import 'package:analyzer/src/context/builder.dart';
import 'package:analyzer/src/source/package_map_resolver.dart';
import 'package:analyzer/src/util/uri.dart';

PackageMapUriResolver packageResolver(
    ResourceProvider provider, String packageName, Folder folder) {
  Map<String, List<Folder>> packageMap = new Map<String, List<Folder>>();

  packageMap.putIfAbsent(packageName, () => [folder]);

  var resolver = new PackageMapUriResolver(provider, packageMap);

  return resolver;
}

EmbedderSdk embeddedResolver(ResourceProvider provider, String projectFolder) {
  // Find package info.
  _PackageInfo packageInfo = _findPackages(provider, projectFolder);

  // Process embedders.
  Map<file_system.Folder, YamlMap> embedderMap =
      new EmbedderYamlLocator(packageInfo.packageMap).embedderYamls;

  var embedder = new EmbedderSdk(embedderMap);

  return embedder;
}

_PackageInfo _findPackages(ResourceProvider provider, String projectFolder) {
  Packages packages;
  Map<String, List<Folder>> packageMap;

  String packageConfigPath = '$projectFolder\\..\\.packages';
  Uri fileUri = new Uri.file(packageConfigPath);
  try {
    io.File configFile = new io.File.fromUri(fileUri).absolute;
    List<int> bytes = configFile.readAsBytesSync();
    Map<String, Uri> map = pkgfile.parse(bytes, configFile.uri);
    packages = new MapPackages(map);
    packageMap = _getPackageMap(packages, provider);
  } catch (e) {
    throw new AssertionError(
        'Unable to read package config data from $packageConfigPath: $e');
  }

  return new _PackageInfo(packages, packageMap);
}

Map<String, List<file_system.Folder>> _getPackageMap(
    Packages packages, ResourceProvider provider) {
  if (packages == null) {
    return null;
  }

  Map<String, List<file_system.Folder>> folderMap =
      new Map<String, List<file_system.Folder>>();
  var pathContext = provider.pathContext;
  packages.asMap().forEach((String packagePath, Uri uri) {
    String path = fileUriToNormalizedPath(pathContext, uri);
    folderMap[packagePath] = [provider.getFolder(path)];
  });
  return folderMap;
}

class _PackageInfo {
  Packages packages;
  Map<String, List<Folder>> packageMap;

  _PackageInfo(this.packages, this.packageMap);
}
