using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using LGSE_APIService.DataObjects;

namespace LGSE_APIService.Models
{
    public class LGSE_APIContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to alter your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        private const string connectionStringName = "Name=MS_TableConnectionString";
        public LGSE_APIContext() : base(connectionStringName)
        {
            this.Database.CommandTimeout = 300;
        }
        private static LGSE_APIContext _instance;
        private static object syncLock = new object();
        public static LGSE_APIContext GetIntance()
        {
            //if (_instance == null)
            //{
            //    lock (syncLock)
            //    {
            //        if (_instance == null)
            //        {
            //            _instance = new LGSE_APIContext();
            //        }
            //    }
            //}
            //return _instance;
            _instance = new LGSE_APIContext();
            _instance.Database.CommandTimeout = 300;
            return _instance;
        }

        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(
                new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                    "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));
        }

        public System.Data.Entity.DbSet<LGSE_APIService.DataObjects.CategoriesMstr> CategoriesMstr { get; set; }

        public System.Data.Entity.DbSet<LGSE_APIService.DataObjects.Domain> Domains { get; set; }

        public System.Data.Entity.DbSet<LGSE_APIService.DataObjects.Feature> Features { get; set; }

        public System.Data.Entity.DbSet<LGSE_APIService.DataObjects.Incident> Incidents { get; set; }

        public System.Data.Entity.DbSet<LGSE_APIService.DataObjects.IncidentHistory> IncidentHistory { get; set; }

        public System.Data.Entity.DbSet<LGSE_APIService.DataObjects.Property> Properties { get; set; }

        public System.Data.Entity.DbSet<LGSE_APIService.DataObjects.PropertyStatusMstr> PropertyStatusMstr { get; set; }

        public System.Data.Entity.DbSet<LGSE_APIService.DataObjects.PropertySubStatusMstr> PropertySubStatusMstrs { get; set; }

        public System.Data.Entity.DbSet<LGSE_APIService.DataObjects.PropertyUserMap> PropertyUserMap { get; set; }

        public System.Data.Entity.DbSet<LGSE_APIService.DataObjects.PropertyUserNote> PropertyUserNotes { get; set; }

        public System.Data.Entity.DbSet<LGSE_APIService.DataObjects.PropertyUserStatus> PropertyUserStatus { get; set; }

        public System.Data.Entity.DbSet<LGSE_APIService.DataObjects.Role> Roles { get; set; }

        public System.Data.Entity.DbSet<LGSE_APIService.DataObjects.RolePermission> RolePermissions { get; set; }

        public System.Data.Entity.DbSet<LGSE_APIService.DataObjects.RoleStatusMap> RoleStatusMaps { get; set; }
         
        public System.Data.Entity.DbSet<LGSE_APIService.DataObjects.User> Users { get; set; }

        public System.Data.Entity.DbSet<LGSE_APIService.DataObjects.UserRoleMap> UserRoleMaps { get; set; }

        public System.Data.Entity.DbSet<LGSE_APIService.DataObjects.IncidentOverviewMstr> IncidentOverviewMstrs { get; set; }

        public System.Data.Entity.DbSet<LGSE_APIService.DataObjects.AuditTrial> AuditTrials { get; set; }

           public System.Data.Entity.DbSet<LGSE_APIService.DataObjects.IncidentPropsStatusCounts> IncidentPropsStatusCounts { get; set; }
    }

}
