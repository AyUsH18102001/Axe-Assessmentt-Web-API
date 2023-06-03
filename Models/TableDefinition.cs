namespace AxeAssessmentToolWebAPI.Models
{
    public class TableDefinition
    {
        public string TableName { get; set; }
        public List<string> Columns { get; set; }
        public List<string> DataType { get; set; }
    }

    public class InformationSchema
    {
        public string TABLE_NAME { get; set; }
        public string COLUMN_NAME { get; set; }
        public string DATA_TYPE { get; set; }
    }
}
