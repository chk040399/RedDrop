using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;

namespace BD.SharedKernel;

//Usage samples
/*
 	//EnumHelper.RegisterAllEnums(Assembly.GetExecutingAssembly(),"BD.PublicPortal.Core.Entities.Enums");
   EnumHelper.RegisterAllEnums(Assembly.GetAssembly(typeof(IAssemblyMarquer)),"BD.PublicPortal.Core.Entities.Enums");
   
   
   
    // Core
   
   EnumHelper.GetEnumValueNameMapByNames("DonorAvailability").Dump();
   
   
   
   
   EnumHelper.GetEnumValueNameMapByNames("DonorAvailability,DonorContactMethod").Dump();
   EnumHelper.ToJson(EnumHelper.GetEnumValueNameMapByNames("DonorAvailability,DonorContactMethod")).Dump();
   
   EnumHelper.GetEnumValueNameMapByNames("DonorAvailability").Dump();
   
   EnumHelper.GetEnumValueNameMapByNames("BloodDonationRequestEvolutionStatus").Dump();
   
   
   EnumHelper.ToJson(EnumHelper.GetEnumValueNameMapByNames("BloodDonationRequestEvolutionStatus")).Dump();
 */

public static class EnumHelper
{
  private static readonly Dictionary<string, Type> _registeredEnums = new(StringComparer.OrdinalIgnoreCase);

  // Register a single enum type manually
  public static void Register<TEnum>() where TEnum : Enum
  {
    RegisterType(typeof(TEnum));
  }

  // Register all enum types in a given assembly, optionally filtered by namespace
  public static void RegisterAllEnums(Assembly assembly, string? @namespace = null)
  {
    var enumTypes = assembly.GetTypes()
      .Where(t => t.IsEnum && (@namespace == null || t.Namespace == @namespace));

    foreach (var enumType in enumTypes)
    {
      RegisterType(enumType);
    }
  }

  // Internal registration method
  private static void RegisterType(Type type)
  {
    if (!type.IsEnum)
      throw new ArgumentException($"{type.FullName} is not an enum type.");

    _registeredEnums[type.Name] = type;
    _registeredEnums[type.FullName!] = type;
  }

  // Get friendly name map for one enum
  public static Dictionary<int, string> GetEnumValueNameMap<TEnum>() where TEnum : Enum
  {
    return Enum.GetValues(typeof(TEnum))
           .Cast<TEnum>()
           .ToDictionary(
             value => Convert.ToInt32(value),
             value => GetDisplayName(value)
           );
  }

  // Get friendly name maps for a batch of enum types
  public static Dictionary<string, Dictionary<int, string>> GetEnumValueNameMapBatch(params Type[] enumTypes)
  {
    var result = new Dictionary<string, Dictionary<int, string>>();

    foreach (var enumType in enumTypes)
    {
      if (!enumType.IsEnum)
        throw new ArgumentException($"{enumType.FullName} is not an enum type.");

      var values = Enum.GetValues(enumType).Cast<Enum>();
      var valueMap = values.ToDictionary(
        value => Convert.ToInt32(value),
        value => GetDisplayName(value)
      );

      result[enumType.Name] = valueMap;
    }

    return result;
  }

  // Get maps for comma-separated enum names (using previously registered enums)
  public static Dictionary<string, Dictionary<int, string>> GetEnumValueNameMapByNames(string commaSeparatedEnumNames)
  {
    var names = commaSeparatedEnumNames.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    var matchedTypes = new List<Type>();

    foreach (var name in names)
    {
      if (_registeredEnums.TryGetValue(name, out var type))
      {
        matchedTypes.Add(type);
      }
      else
      {
        throw new KeyNotFoundException($"Enum type '{name}' was not registered.");
      }
    }

    return GetEnumValueNameMapBatch(matchedTypes.ToArray());
  }

  // Serialize a single enum map to JSON
  public static string ToJson(Dictionary<int, string> enumMap)
  {
    return JsonSerializer.Serialize(enumMap, new JsonSerializerOptions { WriteIndented = true });
  }

  // Serialize a batch enum map to JSON
  public static string ToJson(Dictionary<string, Dictionary<int, string>> batchEnumMap)
  {
    return JsonSerializer.Serialize(batchEnumMap, new JsonSerializerOptions { WriteIndented = true });
  }

  // Utility to get display name
  private static string GetDisplayName(Enum value)
  {
    var memberInfo = value.GetType().GetMember(value.ToString()).FirstOrDefault();
    if (memberInfo == null) return value.ToString();

    var displayAttr = memberInfo.GetCustomAttribute<DisplayAttribute>();
    return displayAttr?.Name ?? value.ToString();
  }
}


