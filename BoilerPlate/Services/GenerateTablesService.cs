using BoilerPlate.Models;
using BoilerPlate.Models.DTO;

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
     
    }
}
