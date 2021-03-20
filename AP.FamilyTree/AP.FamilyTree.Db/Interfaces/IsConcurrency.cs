namespace AP.FamilyTree.Db.Interfaces
{
    public interface IsConcurrency
    {
        public byte[] RowVersion { get; set; }
    }
}
