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
                query += $"COMMENT '{field.ColumnName}'";
                cnt++;
                lstFieldNames.Add(query);
            }
            string Query = $"CREATE TABLE {tableName}(\n";
            foreach(string fields in lstFieldNames)
            {
                Query += (fields + "\n");
            }
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
                string jsonProperty = $"[JsonProperty(\"{tableName[-3..]}1{cnt}\")]\n";
                query = jsonProperty;
                string attribute = $"public {field.DataType} {tableName[-3..]}F{cnt} {{ get; set; }}\n";
                query += attribute;
            }
            string Query = $"public class DTO{tableName}\n{{\n{query}";
            _objResponse.Data = Query;
            _objResponse.IsError = false;
            return _objResponse;
        }

    }
}
