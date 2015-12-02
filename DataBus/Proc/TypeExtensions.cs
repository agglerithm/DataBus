// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DataBus.Proc
{
    /// <summary>
    /// Type related helper methods
    /// </summary>
    public static class TypeExtensions
    {
        public static void SetMemberValue(this object parent, string propName, object val)
        {
            parent.GetType().InvokeMember(propName, BindingFlags.SetProperty, null, parent,
                new Object[] { val });
        }
        public static bool IsAnonymous(this Type type)
        {
            return type.Name.Contains("AnonymousType");
        }    
         

        public static Type FindIEnumerable(this Type seqType)
        {
            if (seqType == null || seqType == typeof(string))
                return null;
            if (seqType.IsArray)
                return typeof(IEnumerable<>).MakeGenericType(seqType.GetGenericElementType());
            if (seqType.IsGenericType)
            {
                foreach (Type ienum in seqType.GetGenericArguments()
                    .Select(arg => typeof(IEnumerable<>).MakeGenericType(arg)).Where(ienum => ienum.IsAssignableFrom(seqType)))
                {
                    return ienum;
                }
            }
            Type[] ifaces = seqType.GetInterfaces();
            if (ifaces != null && ifaces.Length > 0)
            {
                foreach (Type ienum in ifaces.Select(FindIEnumerable).Where(ienum => ienum != null))
                {
                    return ienum;
                }
            }
            if (seqType.BaseType != null && seqType.BaseType != typeof(object))
            {
                return FindIEnumerable(seqType.BaseType);
            }
            return null;
        }

        public static Type GetSequenceType(this Type elementType)
        {
            return typeof(IEnumerable<>).MakeGenericType(elementType);
        }

        public static Type GetGenericElementType(this Type seqType)
        {
            Type ienum = FindIEnumerable(seqType);
            if (ienum == null) return seqType;
            return ienum.GetGenericArguments()[0];
        }

        public static bool IsNullableType(this Type type)
        {
            return type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsNullAssignable(this Type type)
        {
            return !type.IsValueType || IsNullableType(type);
        }

        public static Type GetNonNullableType(this Type type)
        {
            if (IsNullableType(type))
            {
                return type.GetGenericArguments()[0];
            }
            return type;
        }

        public static Type GetNullAssignableType(this Type type)
        {
            if (!IsNullAssignable(type))
            {
                return typeof(Nullable<>).MakeGenericType(type);
            }
            return type;
        }

        public static ConstantExpression GetNullConstant(this Type type)
        {
            return Expression.Constant(null, GetNullAssignableType(type));
        }

        public static Type GetMemberType(this MemberInfo mi)
        {
            FieldInfo fi = mi as FieldInfo;
            if (fi != null) return fi.FieldType;
            PropertyInfo pi = mi as PropertyInfo;
            if (pi != null) return pi.PropertyType;
            EventInfo ei = mi as EventInfo;
            if (ei != null) return ei.EventHandlerType;
            MethodInfo meth = mi as MethodInfo;  // property getters really
            if (meth != null) return meth.ReturnType;
            return null;
        }

        public static object GetDefault(this Type type)
        {
            bool isNullable = !type.IsValueType || type.IsNullableType();
            if (!isNullable)
                return Activator.CreateInstance(type);
            return null;
        }

        public static bool IsReadOnly(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return (((FieldInfo)member).Attributes & FieldAttributes.InitOnly) != 0;
                case MemberTypes.Property:
                    PropertyInfo pi = (PropertyInfo)member;
                    return !pi.CanWrite || pi.GetSetMethod() == null;
                default:
                    return true;
            }
        }

        public static bool IsInteger(this Type type)
        {
            Type nnType = GetNonNullableType(type);
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                default:
                    return false;
            }
        }
    }
}
