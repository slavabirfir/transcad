namespace BLEntities.Entities
{
    public class City
    {
        public int MunId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? AuthorityId { get; set; }
        public override string ToString()
        {
            return string.Format("{0} ({1})",Name,Code);
        }
    }
}
