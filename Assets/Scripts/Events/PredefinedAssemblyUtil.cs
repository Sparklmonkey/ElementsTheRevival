using System;
using System.Collections.Generic;

public static class PredefinedAssemblyUtil {
    private enum AssemblyType {
        AssemblyCSharp,
        AssemblyCSharpEditor,
        AssemblyCSharpEditorFirstPass,
        AssemblyCSharpFirstPass
    }

    private static AssemblyType? GetAssemblyType(string assemblyName) {
        return assemblyName switch {
            "Assembly-CSharp" => AssemblyType.AssemblyCSharp,
            "Assembly-CSharp-Editor" => AssemblyType.AssemblyCSharpEditor,
            "Assembly-CSharp-Editor-firstpass" => AssemblyType.AssemblyCSharpEditorFirstPass,
            "Assembly-CSharp-firstpass" => AssemblyType.AssemblyCSharpFirstPass,
            _ => null
        };
    }

    private static void AddTypesFromAssembly(Type[] assemblyTypes, Type interfaceType, ICollection<Type> results) {
        if (assemblyTypes == null) return;
        for (var i = 0; i < assemblyTypes.Length; i++) {
            var type = assemblyTypes[i];
            if (type != interfaceType && interfaceType.IsAssignableFrom(type)) {
                results.Add(type);
            }
        }
    }
    
    public static List<Type> GetTypes(Type interfaceType) {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        
        var assemblyTypes = new Dictionary<AssemblyType, Type[]>();
        var types = new List<Type>();
        for (var i = 0; i < assemblies.Length; i++) {
            var assemblyType = GetAssemblyType(assemblies[i].GetName().Name);
            if (assemblyType != null) {
                assemblyTypes.Add((AssemblyType) assemblyType, assemblies[i].GetTypes());
            }
        }
        
        assemblyTypes.TryGetValue(AssemblyType.AssemblyCSharp, out var assemblyCSharpTypes);
        AddTypesFromAssembly(assemblyCSharpTypes, interfaceType, types);

        assemblyTypes.TryGetValue(AssemblyType.AssemblyCSharpFirstPass, out var assemblyCSharpFirstPassTypes);
        AddTypesFromAssembly(assemblyCSharpFirstPassTypes, interfaceType, types);
        
        return types;
    }
}