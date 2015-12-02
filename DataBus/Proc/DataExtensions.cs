using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Paymetric.Proc;

namespace DataBus.Proc
{
    public static class DataExtensions
    {
        public static ForeignKey GetForeignKey(this MemberInfo mi)
        {
            var attribute = mi.CustomAttributes.FirstOrDefault(a => a.AttributeType == typeof(ForeignKeyAttribute));
            if (attribute == null) return null;
            return new ForeignKey(attribute);
        }

        public static IEnumerable<MemberInfo> GetMappedMembers(this Type rowType)
        {
            var members = new List<MemberInfo>(rowType.GetFields());
            members.AddRange(rowType.GetProperties());
            return members.OrderBy(m => m.Name);
        }

        public static SqlTypeAttribute GetSqlTypeAttribute(this MemberInfo member)
        {
            return (SqlTypeAttribute)member.GetCustomAttributes().FirstOrDefault(a => a.GetType() == typeof(SqlTypeAttribute));
        }

        public static  bool IsTimeStamp(this MemberInfo member)
        {
            var attr = member.CustomAttributes.FirstOrDefault(a => a.AttributeType == typeof(SqlTypeAttribute));
            if (attr == null) return false;
            var arg = (SqlDbType)attr.ConstructorArguments.First().Value;
            return arg == SqlDbType.Timestamp;
        }

 
        public static IDbParameterWrapper GetInputParameter(this PropertyInfo prop, object parent)
        {
            var attr = prop.GetSqlTypeAttribute();
            var val = prop.GetValue(parent);
            var isNullable = prop.PropertyType.IsNullable(); 
            if (attr == null)
                attr = prop.PropertyType.FakeSqlTypeAttribute();
            return new SqlParameterWrapper(new SqlParameter(prop.Name, attr.DbType, attr.Size, ParameterDirection.Input, isNullable, 0, 0, "", DataRowVersion.Default, val));
        }

        public static void AddParameter(this IDbCommand cmd, IDbParameterWrapper parm)
        {
            cmd.Parameters.Add(parm.Parameter);
        }
        public static bool IsNullable(this Type type)
        {
            return type.IsValueType && !type.IsNullableType();
        }

        public static SqlTypeAttribute FakeSqlTypeAttribute(this Type type)
        {
            return new SqlTypeAttribute(type.GetSqlType(),type.GetSize());
        }

        public static int GetSize(this Type type)
        {
            type = type.GetNonNullableType();
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean: 
                case TypeCode.SByte:
                case TypeCode.Byte: 
                case TypeCode.Int16:
                case TypeCode.UInt16: 
                case TypeCode.Int32:
                case TypeCode.UInt32: 
                case TypeCode.Int64:
                case TypeCode.UInt64: 
                case TypeCode.Char: 
                case TypeCode.DateTime:
                    return 0;
                case TypeCode.Single:
                case TypeCode.Double:
                    return 10;
                case TypeCode.String: 
                case TypeCode.Decimal:
                    return 255;
                default:
                    if (type == typeof (byte[]))
                        return 255;
                    if (type == typeof(Guid) || type == typeof(DateTimeOffset) ||type == typeof(TimeSpan))
                        return 0;
                    break;
            }
            return 0;
        }

        public static SqlDbType GetSqlType(this Type type)
        {  
            type = type.GetNonNullableType();
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return SqlDbType.Bit;
                case TypeCode.SByte:
                case TypeCode.Byte:
                    return SqlDbType.TinyInt;
                case TypeCode.Int16:
                case TypeCode.UInt16:
                    return SqlDbType.SmallInt;
                case TypeCode.Int32:
                case TypeCode.UInt32:
                    return SqlDbType.Int;
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return SqlDbType.BigInt;
                case TypeCode.Single:
                case TypeCode.Double:
                    return SqlDbType.Float;
                case TypeCode.String:
                    return SqlDbType.NVarChar;
                case TypeCode.Char:
                    return SqlDbType.NChar;
                case TypeCode.DateTime:
                    return SqlDbType.DateTime;
                case TypeCode.Decimal:
                    return SqlDbType.Decimal;
                default:
                    if (type == typeof(byte[]))
                        return SqlDbType.VarBinary;
                    if (type == typeof(Guid))
                        return SqlDbType.UniqueIdentifier;
                    if (type == typeof(DateTimeOffset))
                        return SqlDbType.DateTimeOffset;
                    if (type == typeof(TimeSpan))
                        return SqlDbType.Time;
                    break;
            }
            return SqlDbType.NVarChar;
        }
        public static bool IsScalar(this Type type)
        {
            type = type.GetNonNullableType();
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Empty:
                case TypeCode.DBNull:
                    return false;
                case TypeCode.Object:
                    return
                        type == typeof(DateTime) ||
                        type == typeof(DateTimeOffset) ||
                        type == typeof(decimal) ||
                        type == typeof(Guid) ||
                        type == typeof(byte[]);
                default:
                    return true;
            }
        }

        public static bool IsScalarOrAnonymous(this Type type)
        {
            return type.IsScalar() || type.IsAnonymous();
        }

        
 
    }
}