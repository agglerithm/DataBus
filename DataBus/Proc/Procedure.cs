using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using DataBus.Data;
using DataBus.Proc; 

namespace Paymetric.Proc
{
    public class Procedure<P, E> :  IExecutable<P,E> where E : new()
    { 
 

        //            public SessionProcedure(QueryProvider provider, Expression expression) : base(provider, expression)
        //            {
        //            }
        public int Execute(P parameters, IDataContext session)
        {
            Parameters = parameters;
            return (int) callNonQuery(session);
        }

        private int callNonQuery(IDataContext session)
        {
            var command = buildCommand(session, Parameters);
            int retVal = 0;
            session.WithinTransaction(
            ()
        =>
                retVal = command.ExecuteNonQuery());
            return retVal;
        }

        private IDbCommand buildCommand(IDataContext connection, P parameters)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = typeof (P).Name;
            var parms = buildParameters(parameters);
            foreach (var parm in parms)
            {
                cmd.AddParameter(parm);
            }
            return cmd;
        }

        private IEnumerable<IDbParameterWrapper> buildParameters(P parameters)
        {
            var props = parameters.GetType().GetProperties();
            return props.ToList().Select(p => p.GetInputParameter(parameters));
        }

        public IEnumerable<E> ExecuteQuery(P parameters, IDataContext session)
        {
            ReturnsRecords = true;
            Parameters = parameters;
            IEnumerable<E> retVal = null;
            session.WithinTransaction(
                () =>
                {
                    var reader = buildCommand(session, parameters).ExecuteReader();
                    retVal = reader.IsClosed ? new List<E>() : mapList(reader).ToList();
                });
            return retVal;
        }
 
        public Type ReturnType { get { return typeof (E); } }
 

 

        private IEnumerable<E> mapList(IDataReader reader)
        { 
            var fieldReader = new DbFieldReader(reader);
            do
            {
                while (reader.Read())
                {
                    yield return getEntity(fieldReader);
                }
            } while (reader.NextResult());
        }

        private E getEntity(DbFieldReader fieldReader)
        {
            var entity = new E();
            var props = typeof (E).GetProperties();
            foreach(var prop in props)
            {
                int i = fieldReader.GetOrdinal(prop.Name);
                entity.SetMemberValue(props[i].Name,fieldReader.Read(i,props[i].PropertyType));
            }
            return entity;
        }

        public P Parameters { get; private set; }
        public bool ReturnsRecords { get; private set; }
        public Expression Definition { get { return Expression.Constant(typeof (P)); }}
 
        int IExecutable<P, E>.Execute(P parameters, IDataContext session)
        {
            return Execute(parameters, session);
        }

        E IExecutable<P, E>.ExecuteScalar(P parameters, IDataContext session)
        {
            return callScalar(parameters, session);
        }

        private E callScalar(P parameters, IDataContext session)
        {
            var command = buildCommand(session, parameters);
            return mapScalar(command.ExecuteReader());
        }

        private E mapScalar(IDataReader reader)
        {
            var fieldReader = new DbFieldReader(reader);
            return !reader.Read() ? default(E) : fieldReader.ReadValue<E>(0);
        }


        protected class DbFieldReader : FieldReader
        {
            IDataReader reader;  

            public DbFieldReader(IDataReader reader)
            {
                this.reader = reader; 
                if(!reader.IsClosed)
                    this.Init();
            }

            protected override int FieldCount
            {
                get
                {
                     return this.reader.FieldCount;
                }
            }

            protected override Type GetFieldType(int ordinal)
            {
                return this.reader.GetFieldType(ordinal);
            }

            protected override bool IsDBNull(int ordinal)
            {
                return this.reader.IsDBNull(ordinal);
            }

            protected override T GetValue<T>(int ordinal)
            {
                return (T)Convert(this.reader.GetValue(ordinal), typeof(T));
            }

            protected override Byte GetByte(int ordinal)
            {
                return this.reader.GetByte(ordinal);
            }

            protected override Char GetChar(int ordinal)
            {
                return this.reader.GetChar(ordinal);
            }

            protected override DateTime GetDateTime(int ordinal)
            {
                return this.reader.GetDateTime(ordinal);
            }

            protected override Decimal GetDecimal(int ordinal)
            {
                return this.reader.GetDecimal(ordinal);
            }

            protected override Double GetDouble(int ordinal)
            {
                return this.reader.GetDouble(ordinal);
            }

            protected override Single GetSingle(int ordinal)
            {
                return this.reader.GetFloat(ordinal);
            }

            protected override Guid GetGuid(int ordinal)
            {
                return this.reader.GetGuid(ordinal);
            }

            protected override Int16 GetInt16(int ordinal)
            {
                return this.reader.GetInt16(ordinal);
            }

            protected override Int32 GetInt32(int ordinal)
            {
                return this.reader.GetInt32(ordinal);
            }

            protected override Int64 GetInt64(int ordinal)
            {
                return this.reader.GetInt64(ordinal);
            }

            protected override String GetString(int ordinal)
            {
                return this.reader.GetString(ordinal);
            }

            private  object Convert(object value, Type type)
            {
                if (value == null)
                {
                    return type.GetDefault();
                }
                type = type.GetNonNullableType();
                Type vtype = value.GetType();
                if (type != vtype)
                {
                    if (type.IsEnum)
                    {
                        if (vtype == typeof(string))
                        {
                            return Enum.Parse(type, (string)value);
                        }
                        else
                        {
                            Type utype = Enum.GetUnderlyingType(type);
                            if (utype != vtype)
                            {
                                value = System.Convert.ChangeType(value, utype);
                            }
                            return Enum.ToObject(type, value);
                        }
                    }
                    return System.Convert.ChangeType(value, type);
                }
                return value;
            }

            public object Read(int i, Type propertyType)
            {
                return Convert(this.reader.GetValue(i), propertyType);
            }

            public int GetOrdinal(string name)
            {
                return reader.GetOrdinal(name);
            }
        }
    }


}