namespace DAL.Common
{
    public class ContextConfiguration
    {
        public string ServerName { get; set; }
        public int ServerPort { get; set; }
        public string DatabaseName { get; set; }
        public string DbAdminLogin { get; set; }
        public string DbAdminPassword { get; set; }
    }

}
