import 'package:analyzer/file_system/file_system.dart';
import 'package:analyzer/source/package_map_resolver.dart';

PackageMapUriResolver packageResolver(
    ResourceProvider provider, String packageName, Folder folder) {
  Map<String, List<Folder>> packageMap = new Map<String, List<Folder>>();

  packageMap.putIfAbsent(packageName, () => [folder]);

  var resolver = new PackageMapUriResolver(provider, packageMap);

  return resolver;
}
