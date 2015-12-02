using System;

namespace DataBus.Proc
{
    public class ForeignKeyAttribute : Attribute
    {
        private readonly string _tableName;
        private readonly string _colName;

        public ForeignKeyAttribute(string tableName, string colName, int ndx = 0)
        {
            _tableName = tableName;
            _colName = colName;
        }
    }
}