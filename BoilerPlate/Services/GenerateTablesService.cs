using BoilerPlate.Models;
using BoilerPlate.Models.DTO;
using System.Diagnostics.Metrics;

namespace BoilerPlate.Services
{
    public class GenerateTablesService
    {
        Response _objResponse;
        public GenerateTablesService() { 
            _objResponse = new Response();
        }
        public Response GenerateTableQuery(List<DTOTableDefinition> lstDTOTableDefinition , string tableName)
        {
            List<String> lstFieldNames = new();
            int cnt = 1;
            foreach(DTOTableDefinition field in lstDTOTableDefinition)
            {
                string query = $"{tableName.Substring(tableName.Length - 3)}F0{cnt} {field.DataType}";
                if (field.IsPrimaryKey)
                {
                    query += " PRIMARY KEY";
                }
                if (field.IsAutoIncrement)
                {
                    query += " AUTO_INCREMENT";
                }
                if (field.IsUnique)
                {
                    query += " UNIQUE";
                }
                if (field.IsNotNull)
                {
                    query += " NOT NULL";
                }
                if (field.HasZeroFlag) {
                    query += " NOT NULL";
                }
                if(field.DefaultExpression != null)
                {
                    query += $" Default";
                }
                query += $" COMMENT '{field.ColumnName}'";
                cnt++;
                lstFieldNames.Add(query);
            }
            string Query = $"CREATE TABLE {tableName}(\n";
            foreach(string fields in lstFieldNames)
            {
                Query += (fields + "\n");
            }
            Query += "}";
            Console.WriteLine(Query);
            _objResponse.Data = Query;
            _objResponse.IsError = false;
            return _objResponse;
        }
        
        public Response GenerateDTO(List<DTOTableDefinition> lstDTOTableDefinition, string tableName)
        {
            int cnt = 1;
            string query = "";
            foreach (DTOTableDefinition field in lstDTOTableDefinition)
            {
                string jsonProperty = $"[JsonProperty(\"{tableName.Substring(tableName.Length - 3)}1{cnt}\")]\n";
                //if(query ==  "") {
                //    query = jsonProperty;
                //}
                //else
                //{
                //    query += jsonProperty;
                //}
                query += jsonProperty;
                string attribute = $"public {field.DataType} {tableName.Substring(tableName.Length - 3)}F{cnt} {{ get; set; }}\n";
                query += attribute;
                cnt++;
            }
            string Query = $"public class DTO{tableName}\n{{\n{query}";
            _objResponse.Data = Query; 
            _objResponse.IsError = false;
            Console.WriteLine(Query);
            return _objResponse;
        }

        public static string GetCSharpDataType(string sqlDataType)
        {
            var typeMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // Numeric types
            { "bigint", "long" },
            { "binary", "byte[]" },
            { "bit", "bool" },
            { "char", "string" },
            { "date", "DateTime" },
            { "datetime", "DateTime" },
            { "datetime2", "DateTime" },
            { "datetimeoffset", "DateTimeOffset" },
            { "decimal", "decimal" },
            { "float", "double" },
            { "image", "byte[]" },
            { "int", "int" },
            { "money", "decimal" },
            { "nchar", "string" },
            { "ntext", "string" },
            { "numeric", "decimal" },
            { "nvarchar", "string" },
            { "real", "float" },
            { "rowversion", "byte[]" },
            { "smalldatetime", "DateTime" },
            { "smallint", "short" },
            { "smallmoney", "decimal" },
            { "sql_variant", "object" },
            { "text", "string" },
            { "time", "TimeSpan" },
            { "timestamp", "byte[]" },
            { "tinyint", "byte" },
            { "uniqueidentifier", "Guid" },
            { "varbinary", "byte[]" },
            { "varchar", "string" },
            { "xml", "string" }
        };
            //string csharpType = SqlToCSharpMapper.GetCSharpDataType(sqlType);
            return typeMappings.TryGetValue(sqlDataType, out var csharpType)
                ? csharpType
                : "object"; // Default to 'object' if no match found
        }


    }
}
