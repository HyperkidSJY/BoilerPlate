namespace BoilerPlate.Models.DTO
{
    public class DTOTableDefinition
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsNotNull { get; set; }
        public bool IsUnique { get; set; }
        public bool IsAutoIncrement { get; set; }
        public bool HasZeroFlag { get; set; }
        public string? DefaultExpression { get; set; }
    }
}
