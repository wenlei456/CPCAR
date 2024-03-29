﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAO
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CarPartsEntities : DbContext
    {
        public CarPartsEntities()
            : base("name=CarPartsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<CarsInfo> CarsInfo { get; set; }
        public DbSet<ExpRecord> ExpRecord { get; set; }
        public DbSet<PartAttr> PartAttr { get; set; }
        public DbSet<PartBrand> PartBrand { get; set; }
        public DbSet<PartStock> PartStock { get; set; }
        public DbSet<WebsiteMenu> WebsiteMenu { get; set; }
        public DbSet<MemberLevel> MemberLevel { get; set; }
        public DbSet<FriendCode> FriendCode { get; set; }
        public DbSet<AddressBook> AddressBook { get; set; }
        public DbSet<RebateDraw> RebateDraw { get; set; }
        public DbSet<AdminAccount> AdminAccount { get; set; }
        public DbSet<PartsCategory> PartsCategory { get; set; }
        public DbSet<PartsExtend> PartsExtend { get; set; }
        public DbSet<LogInfo> LogInfo { get; set; }
        public DbSet<Active> Active { get; set; }
        public DbSet<ActiveAttr> ActiveAttr { get; set; }
        public DbSet<OrderPay> OrderPay { get; set; }
        public DbSet<OrderProList> OrderProList { get; set; }
        public DbSet<RebateRecord> RebateRecord { get; set; }
        public DbSet<Voucher> Voucher { get; set; }
        public DbSet<ActivePros> ActivePros { get; set; }
        public DbSet<ActiveImg> ActiveImg { get; set; }
        public DbSet<CarM> CarM { get; set; }
        public DbSet<HotTable> HotTable { get; set; }
        public DbSet<ImgStock> ImgStock { get; set; }
        public DbSet<MemberBase> MemberBase { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<information> information { get; set; }
        public DbSet<PhoneMsg> PhoneMsg { get; set; }
        public DbSet<Addship> Addship { get; set; }
        public DbSet<tml> tml { get; set; }
        public DbSet<Parts> Parts { get; set; }
        public DbSet<CrowdFunding> CrowdFunding { get; set; }
        public DbSet<comments> comments { get; set; }
    }
}
