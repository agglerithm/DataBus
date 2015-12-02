using System.Reflection;

namespace DataBus.Proc
{
    public class ForeignKey
    {
        public ForeignKey(CustomAttributeData data)
        {
            TableName = (string)data.ConstructorArguments[0].Value;
            ColumnName = (string)data.ConstructorArguments[1].Value;
            FullName = TableName + "." + ColumnName;
            Index = (int)data.ConstructorArguments[2].Value;
        }

        public string FullName { get; private set; }

        public string TableName { get; private set; }
        public string ColumnName { get; private set; }
        public int Index { get; private set; }

    }
}