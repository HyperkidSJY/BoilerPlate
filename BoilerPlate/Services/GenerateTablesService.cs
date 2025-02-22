﻿using BoilerPlate.Models;
using BoilerPlate.Models.DTO;
using System.Globalization;

namespace BoilerPlate.Services
{
    public class GenerateTablesService
    {
        Response _objResponse;
        public GenerateTablesService()
        {
            _objResponse = new Response();
        }
        public Response GenerateTableQuery(List<DTOTableDefinition> lstDTOTableDefinition, string tableName)
        {
            List<String> lstFieldNames = new();
            int cnt = 1;
            int totalFields = lstDTOTableDefinition.Count;
            foreach (DTOTableDefinition field in lstDTOTableDefinition)
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
                if (field.HasZeroFlag)
                {
                    query += " NOT NULL";
                }
                if (field.DefaultExpression != null)
                {
                    query += $" Default {FormatDefaultExpression(field.DataType, field.DefaultExpression)}";
                }
                query += $" COMMENT '{field.ColumnName}'";
                if (cnt < totalFields)
                {
                    query += ",";
                }
                cnt++;
                lstFieldNames.Add(query);
            }
            string Query = $"CREATE TABLE {tableName}(\n";
            foreach (string fields in lstFieldNames)
            {
                Query += (fields + "\n");
            }
            Query += ")";
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
                string count = cnt > 10 ? cnt.ToString() : $"0{cnt}";
                string jsonProperty = $"\t[JsonProperty(\"{tableName.Substring(tableName.Length - 3)}1{count}\")]\n";
                query += jsonProperty;
                string csharpType = GetCSharpDataType(field.DataType.ToLower());
                string attribute = $"\tpublic {csharpType} {tableName.Substring(tableName.Length - 3)}F{count} {{ get; set; }}\n\n";
                query += attribute;
                cnt++;
            }
            string Query = $"public class DTO{tableName}\n{{\n{query}}}";
            _objResponse.Data = Query;
            _objResponse.IsError = false;
            Console.WriteLine(Query);
            return _objResponse;
        }

        public Response GeneratePoco(List<DTOTableDefinition> lstDTOTableDefinition, string tableName)
        {
            int cnt = 1;
            string query = "";

            foreach (DTOTableDefinition field in lstDTOTableDefinition)
            {

                string properties = "";
                List<string> annotations = new List<string>();

                if (field.IsPrimaryKey)
                {
                    annotations.Add("PrimaryKey");
                }
                if (field.IsAutoIncrement)
                {
                    annotations.Add("AutoIncrement");
                }
                if (field.IsUnique)
                {
                    annotations.Add("Unique");
                }
                if (field.IsNotNull)
                {
                    annotations.Add("ValidateNotNull");
                }

                if (annotations.Count > 0)
                {
                    properties = $"[{string.Join(" , ", annotations)}]";
                }

                string count = cnt > 10 ? cnt.ToString() : $"0{cnt}";
                string csharpType = GetCSharpDataType(field.DataType.ToLower());
                string attribute = $"\tpublic {csharpType} {tableName.Substring(tableName.Length - 3)}F{count} {{ get; set; }}\n\n";

                query += $"\t{properties}\n{attribute}";
                cnt++;
            }

            string Query = $"public class {tableName}\n{{\n{query}}}";
            _objResponse.Data = Query;
            _objResponse.IsError = false;
            Console.WriteLine(Query);
            return _objResponse;
        }

        public string FormatDefaultExpression(string sqlDataType, string defaultExpression)
        {
            var typesRequiringQuotes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "char",
                "nchar",
                "varchar",
                "nvarchar",
                "text",
                "ntext",
                "xml",
                "uniqueidentifier"
            };

            // Check if data type needs quotes for the default expression
            if (typesRequiringQuotes.Contains(sqlDataType))
            {
                return $"'{defaultExpression}'"; // Wrap in quotes for string-like types
            }

            return defaultExpression; // No quotes for numeric types
        }

        public static string GetCSharpDataType(string sqlDataType)
        {
            // Strip size or other annotations from SQL types like VARCHAR(50), INT, etc.
            string cleanedSqlType = sqlDataType.Split('(')[0].Trim();

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
                { "xml", "string" },
                { "boolean", "bool" },  // Special handling for BOOLEAN type
            };

            // Default to 'object' if no match found
            return typeMappings.TryGetValue(cleanedSqlType, out var csharpType)
                ? csharpType
                : "object";
        }


    }
}
