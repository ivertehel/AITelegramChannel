using FluentResults;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace AiTelegramChannel.ServerHost.Extensions;

public static class LoggerExtensions
{
    public static void TraceEnter<T>(
        this ILogger logger,
        T argument,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string fileName = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var className = fileName.Split("\\").Last().Replace(".cs", "");
        logger.LogDebug($"{className}.{memberName} entered in line {lineNumber} with argument <{JsonConvert.SerializeObject(argument)}>");
    }

    public static void TraceEnter(
        this ILogger logger,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string fileName = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var className = fileName.Split("\\").Last().Replace(".cs", "");
        logger.LogDebug($"{className}.{memberName} entered in line {lineNumber}");
    }

    public static void TraceExit(
        this ILogger logger,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string fileName = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var className = fileName.Split("\\").Last().Replace(".cs", "");
        logger.LogDebug($"{className}.{memberName} exited in line {lineNumber}>");
    }

    public static T TraceExit<T>(
        this ILogger logger,
        T result,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string fileName = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var className = fileName.Split("\\").Last().Replace(".cs", "");
        logger.LogDebug($"{className}.{memberName} exited in line {lineNumber} with result <{JsonConvert.SerializeObject(result)}>");
        return result;
    }

    public static T LogExit<T>(
        this ILogger logger,
        T result,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string fileName = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var className = fileName.Split("\\").Last().Replace(".cs", "");
        logger.LogInformation($"{className}.{memberName} exited in line {lineNumber} with result <{JsonConvert.SerializeObject(result)}>");
        return result;
    }

    public static Result TraceError(
        this ILogger logger,
        Result result,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string fileName = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var className = fileName.Split("\\").Last().Replace(".cs", "");
        logger.LogError($"{className}.{memberName} failed: {result} in line {lineNumber}");
        return result;
    }

    public static void TraceError(
        this ILogger logger,
        Exception ex,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string fileName = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var className = fileName.Split("\\").Last().Replace(".cs", "");
        logger.LogError($"{className}.{memberName} failed: {ex.Message} in line {lineNumber}", ex);
    }

    public static void TraceError(
        this ILogger logger,
        string error,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string fileName = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        var className = fileName.Split("\\").Last().Replace(".cs", "");
        logger.LogError($"{className}.{memberName} failed: {error} in line {lineNumber}");
    }
}