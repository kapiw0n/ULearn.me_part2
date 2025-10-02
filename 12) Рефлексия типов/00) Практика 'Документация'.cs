using System;
using System.Linq;
using System.Reflection;

namespace Documentation;
//поправил
public class Specifier<T> : ISpecifier
{
    private static readonly Type _type = typeof(T); 

    public string GetApiDescription()
    {
        return _type.GetCustomAttribute<ApiDescriptionAttribute>()?.Description;
    }

    public string[] GetApiMethodNames()
    {
        return _type
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => m.GetCustomAttribute<ApiMethodAttribute>() != null)
            .Select(m => m.Name)
            .ToArray();
    }

    public string GetApiMethodDescription(string methodName)
    {
        var method = GetMethod(methodName);
        return method?.GetCustomAttribute<ApiDescriptionAttribute>()?.Description;
    }

    public string[] GetApiMethodParamNames(string methodName)
    {
        var method = GetMethod(methodName);
        return method?.GetParameters().Select(p => p.Name).ToArray() ?? Array.Empty<string>();
    }

    public string GetApiMethodParamDescription(string methodName, string paramName)
    {
        var param = GetParameter(methodName, paramName);
        return param?.GetCustomAttribute<ApiDescriptionAttribute>()?.Description;
    }

    public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName)
    {
        var param = GetParameter(methodName, paramName);
        if (param == null) return CreateDefaultParamDescription(paramName);

        var desc = new ApiParamDescription
        {
            ParamDescription = new CommonDescription(
                paramName,
                param.GetCustomAttribute<ApiDescriptionAttribute>()?.Description),
            Required = param.GetCustomAttribute<ApiRequiredAttribute>()?.Required ?? false,
        };

        var intValidation = param.GetCustomAttribute<ApiIntValidationAttribute>();
        if (intValidation != null)
        {
            desc.MinValue = intValidation.MinValue;
            desc.MaxValue = intValidation.MaxValue;
        }

        return desc;
    }

    public ApiMethodDescription? GetApiMethodFullDescription(string methodName)
    {
        var method = GetMethod(methodName);
        if (method == null) return null;

        return new ApiMethodDescription
        {
            MethodDescription = new CommonDescription(
                methodName,
                method.GetCustomAttribute<ApiDescriptionAttribute>()?.Description),
            ParamDescriptions = method.GetParameters()
                .Select(p => GetApiMethodParamFullDescription(methodName, p.Name))
                .ToArray(),
            ReturnDescription = method.ReturnType == typeof(void) 
                ? null 
                : GetReturnDescription(method)
        };
    }

    private ApiParamDescription? GetReturnDescription(MethodInfo method)
    {
        if (method.ReturnType == typeof(void))
            return null;

        var desc = new ApiParamDescription
        {
            Required = method.ReturnParameter?
                .GetCustomAttribute<ApiRequiredAttribute>()?.Required ?? false,
            ParamDescription = new CommonDescription()
        };

        var intValidation = method.ReturnParameter?
            .GetCustomAttribute<ApiIntValidationAttribute>();
        if (intValidation != null)
        {
            desc.MinValue = intValidation.MinValue;
            desc.MaxValue = intValidation.MaxValue;
        }

        return desc;
    }

    private MethodInfo? GetMethod(string methodName)
    {
        return _type
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .FirstOrDefault(m => 
                m.Name == methodName && 
                m.GetCustomAttribute<ApiMethodAttribute>() != null);
    }

    private ParameterInfo? GetParameter(string methodName, string paramName)
    {
        var method = GetMethod(methodName);
        return method?.GetParameters()
            .FirstOrDefault(p => p.Name == paramName);
    }

    private static ApiParamDescription CreateDefaultParamDescription(string paramName)
    {
        return new ApiParamDescription
        {
            ParamDescription = new CommonDescription(paramName),
            Required = false
        };
    }
}