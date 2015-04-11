using System.Data.Entity;

namespace JinxiaocunApp.Models
{
    public class JinxiaocunAppContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<JinxiaocunApp.Models.JinxiaocunAppContext>());

        public JinxiaocunAppContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<BDepartment> BDepartments { get; set; }

        public DbSet<BPartMenu> BPartMenus { get; set; }

        public DbSet<BEmplyee> BEmplyees { get; set; }

        public DbSet<BStore> BStores { get; set; }

        public DbSet<BLinkCompany> BLinkCompanies { get; set; }

        public DbSet<BProduct> BProducts { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<PermissionsInRoles> PermissionsInRoles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BEmplyee>()
              .HasMany<Role>(r => r.Roles)
              .WithMany(u => u.Emplyees)
              .Map(m =>
              {
                  m.ToTable("webpages_UsersInRoles");
                  m.MapLeftKey("UserId");
                  m.MapRightKey("RoleId");
              });

            //modelBuilder.Entity<BEmplyee>()
            //    .HasMany<Tender_GongGao>(r => r.tenderGonggaos)
            //    .WithMany(u => u.bemplyees)
            //    .Map(m => {
            //        m.ToTable("Bemp_GongGao");
            //        m.MapLeftKey("UserId");
            //        m.MapRightKey("GongGaoId");
            //    });
            
        }

        public DbSet<Permission> Permissions { get; set; }

        //public DbSet<Tender_CompanyInfo> Tender_CompanyInfo { get; set; }

        public DbSet<Tender_GongGao> Tender_GongGaos { get; set; }

        public DbSet<Tender_GongGao_Item> Tender_GongGao_Items { get; set; }

        public DbSet<Tender_ProjectType> Tender_ProjectTypes { get; set; }

        public DbSet<Tender_ShenQing> Tender_ShenQings { get; set; }

        public DbSet<Tender_ShenQing_Item> Tender_ShenQing_Items { get; set; }

        public DbSet<Tender_PingShen> Tender_PingShens { get; set; }

        public DbSet<Tender_PingShen_Item> Tender_PingShen_Items { get; set; }

        public DbSet<Tender_PingShen_User> Tender_PingShen_Users { get; set; }

        public DbSet<Bemp_GongGao> Bemp_GongGaos { get; set; }

        public DbSet<Tender_ModelManage1> Tender_ModelManage1 { get; set; }

        public DbSet<Tender_ModelCustomer1> Tender_ModelCustomer1 { get; set; }

        public DbSet<Tender_ModelManage2> Tender_ModelManage2 { get; set; }

        public DbSet<Tender_ModelCustomer2> Tender_ModelCustomer2 { get; set; }

        public DbSet<Tender_CompanyType> Tender_CompanyTypes { get; set; }

        public DbSet<Tender_CustomerFiles> Tender_CustomerFiless { get; set; }

        public DbSet<Tender_TongZhi> Tender_TongZhi { get; set; }
    }
}
