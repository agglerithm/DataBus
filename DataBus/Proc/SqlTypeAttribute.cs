using System;
using System.Data;
using System.Data.SqlClient;

namespace DataBus.Proc
{
    public class SqlTypeAttribute : Attribute
    {

        internal SqlDbType DbType { get; private set; }
        internal int Size { get; private set; }
        internal byte Precision { get; private set; }
        public SqlTypeAttribute(SqlDbType sqlDbType)
        {
            DbType = sqlDbType;
        }

        public SqlTypeAttribute(SqlDbType sqlDbType, int size)
        {
            DbType = sqlDbType;
            Size = size;
        }

        public SqlTypeAttribute(SqlDbType sqlDbType, int size, byte precision)
        {
            DbType = sqlDbType;
            Size = size;
            Precision = precision;
        }

        public SqlParameterWrapper CreateParameter(string name)
        {
            return new SqlParameterWrapper(new SqlParameter(name, DbType, Size) { Precision = this.Precision });
        }
    }
}