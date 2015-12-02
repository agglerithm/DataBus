using System.Data;
using System.Data.SqlClient;

namespace DataBus.Proc
{
    public interface IDbParameterWrapper
    {
        string ParameterName { get; }
        object Value { get; set; }
        SqlParameter Parameter { get; }
        byte Precision { get; set; }
        byte Scale { get; set; }
        string SourceColumn { get; set; }
        ParameterDirection Direction { get; set; }
    }

    public class SqlParameterWrapper : IDbParameterWrapper
    {
        private readonly SqlParameter _parameter;

        public SqlParameterWrapper(SqlParameter parameter)
        {
            _parameter = parameter;
        }

        public string ParameterName { get { return _parameter.ParameterName; } }

        public object Value
        {
            get { return _parameter.Value; }
            set { _parameter.Value = value; }
        }

        public SqlParameter Parameter { get { return _parameter; } }

        public byte Precision
        {
            get { return _parameter.Precision; }
            set { _parameter.Precision = value; }
        }

        public byte Scale
        {
            get { return _parameter.Scale; }
            set { _parameter.Scale = value; }
        }

        public string SourceColumn
        {
            get { return _parameter.SourceColumn; }
            set { _parameter.SourceColumn = value; }
        }

        public ParameterDirection Direction
        {
            get { return _parameter.Direction; }
            set { _parameter.Direction = value; }
        }
    }
}