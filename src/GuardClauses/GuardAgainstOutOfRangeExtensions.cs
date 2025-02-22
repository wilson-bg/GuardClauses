﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Ardalis.GuardClauses;

public static partial class GuardClauseExtensions
{
    /// <summary>
    /// Throws an <see cref="InvalidEnumArgumentException" /> if <paramref name="input"/> is not a valid enum value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="guardClause"></param>
    /// <param name="input"></param>
    /// <param name="parameterName"></param>
    /// <param name="message">Optional. Custom error message</param>
    /// <returns><paramref name="input" /> if the value is not out of range.</returns>
    /// <exception cref="InvalidEnumArgumentException"></exception>
#if NETSTANDARD || NETFRAMEWORK
    public static int EnumOutOfRange<T>(this IGuardClause guardClause,
        int input,
        string parameterName,
        string? message = null) where T : struct, Enum
#else
    public static int EnumOutOfRange<T>(this IGuardClause guardClause,
        int input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null) where T : struct, Enum
#endif
    {
        if (!Enum.IsDefined(typeof(T), input))
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new InvalidEnumArgumentException(parameterName, input, typeof(T));
            }
            throw new InvalidEnumArgumentException(message);
        }

        return input;
    }

    /// <summary>
    /// Throws an <see cref="InvalidEnumArgumentException" /> if <paramref name="input"/> is not a valid enum value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="guardClause"></param>
    /// <param name="input"></param>
    /// <param name="parameterName"></param>
    /// /// <param name="message">Optional. Custom error message</param>
    /// <returns><paramref name="input" /> if the value is not out of range.</returns>
    /// <exception cref="InvalidEnumArgumentException"></exception>
#if NETSTANDARD || NETFRAMEWORK
    public static T EnumOutOfRange<T>(this IGuardClause guardClause,
        T input,
        string parameterName,
        string? message = null) where T : struct, Enum
#else
    public static T EnumOutOfRange<T>(this IGuardClause guardClause,
        T input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null) where T : struct, Enum
#endif
    {
        if (!Enum.IsDefined(typeof(T), input))
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new InvalidEnumArgumentException(parameterName, Convert.ToInt32(input), typeof(T));
            }
            throw new InvalidEnumArgumentException(message);
        }

        return input;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentOutOfRangeException" /> if  any <paramref name="input"/>'s item is less than <paramref name="rangeFrom"/> or greater than <paramref name="rangeTo"/>.
    /// </summary>
    /// <param name="guardClause"></param>
    /// <param name="input"></param>
    /// <param name="parameterName"></param>
    /// <param name="rangeFrom"></param>
    /// <param name="rangeTo"></param>
    /// <param name="message">Optional. Custom error message</param>
    /// <returns><paramref name="input" /> if any item is not out of range.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static IEnumerable<T> OutOfRange<T>(this IGuardClause guardClause, IEnumerable<T> input, string parameterName, T rangeFrom, T rangeTo, string? message = null) where T : IComparable, IComparable<T>
    {
        if (rangeFrom.CompareTo(rangeTo) > 0)
        {
            throw new ArgumentException(message ?? $"{nameof(rangeFrom)} should be less or equal than {nameof(rangeTo)}", parameterName);
        }

        if (input.Any(x => x.CompareTo(rangeFrom) < 0 || x.CompareTo(rangeTo) > 0))
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentOutOfRangeException(parameterName, message ?? $"Input {parameterName} had out of range item(s)");
            }
            throw new ArgumentOutOfRangeException(parameterName, message);
        }

        return input;
    }
    /// <summary>
    /// Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="input" /> is not in the range of valid SqlDateTime values.
    /// </summary>
    /// <param name="guardClause"></param>
    /// <param name="input"></param>
    /// <param name="parameterName"></param>
    /// <param name="message">Optional. Custom error message</param>
    /// <returns><paramref name="input" /> if the value is in the range of valid SqlDateTime values.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
#if NETSTANDARD || NETFRAMEWORK
    public static DateTime OutOfSQLDateRange(this IGuardClause guardClause,
        DateTime input,
        string parameterName,
        string? message = null)
#else
    public static DateTime OutOfSQLDateRange(this IGuardClause guardClause,
        DateTime input,
        [CallerArgumentExpression("input")] string? parameterName = null,
        string? message = null)
#endif
    {
        // System.Data is unavailable in .NET Standard so we can't use SqlDateTime.
        const long sqlMinDateTicks = 552877920000000000;
        const long sqlMaxDateTicks = 3155378975999970000;

        return OutOfRange<DateTime>(guardClause, input, parameterName!, new DateTime(sqlMinDateTicks), new DateTime(sqlMaxDateTicks), message);
    }

    /// <summary>
    /// Throws an <see cref="ArgumentOutOfRangeException" /> if <paramref name="input"/> is less than <paramref name="rangeFrom"/> or greater than <paramref name="rangeTo"/>.
    /// </summary>
    /// <param name="guardClause"></param>
    /// <param name="input"></param>
    /// <param name="parameterName"></param>
    /// <param name="rangeFrom"></param>
    /// <param name="rangeTo"></param>
    /// <param name="message">Optional. Custom error message</param>
    /// <returns><paramref name="input" /> if the value is not out of range.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static T OutOfRange<T>(this IGuardClause guardClause, T input,
        string parameterName,
        T rangeFrom,
        T rangeTo,
        string? message = null) where T : IComparable, IComparable<T>
    {
        if (rangeFrom.CompareTo(rangeTo) > 0)
        {
            throw new ArgumentException(message ?? $"{nameof(rangeFrom)} should be less or equal than {nameof(rangeTo)}", parameterName);
        }

        if (input.CompareTo(rangeFrom) < 0 || input.CompareTo(rangeTo) > 0)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentOutOfRangeException(parameterName, $"Input {parameterName} was out of range");
            }
            throw new ArgumentOutOfRangeException(parameterName, message);
        }

        return input;
    }

}
