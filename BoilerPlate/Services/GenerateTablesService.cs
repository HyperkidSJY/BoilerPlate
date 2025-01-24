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
                query += jsonProperty;
                string count = cnt > 10 ? cnt.ToString() : $"0{cnt}";
                string attribute = $"public {field.DataType} {tableName.Substring(tableName.Length - 3)}F{count} {{ get; set; }}\n";
                query += attribute;
                cnt++;
            }
            string Query = $"public class DTO{tableName}\n{{\n{query}\n}}";
            _objResponse.Data = Query; 
            _objResponse.IsError = false;
            Console.WriteLine(Query);
            return _objResponse;
        }

    }
}
