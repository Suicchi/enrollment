using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Enrollment.Migrations
{
    [Migration(2)]
    public class _002_Student : Migration
    {
        public override void Up()
        {
            Create.Table("Students")
                .WithColumn("id").AsInt32().Identity().PrimaryKey().NotNullable()
                .WithColumn("name").AsString(Int32.MaxValue).NotNullable()
                .WithColumn("fathersname").AsString(Int32.MaxValue).NotNullable()
                .WithColumn("mothersname").AsString(Int32.MaxValue).NotNullable()
                .WithColumn("gender").AsString().NotNullable()
                .WithColumn("presentaddress").AsString(Int32.MaxValue).NotNullable()
                .WithColumn("permanentaddress").AsString(Int32.MaxValue).NotNullable()
                .WithColumn("dateofbirth").AsDateTime().NotNullable()
                .WithColumn("religion").AsString()
                .WithColumn("phone").AsString(Int32.MaxValue).NotNullable()
                .WithColumn("email").AsString(Int32.MaxValue)
                .WithColumn("admissiondate").AsDateTime().WithDefaultValue(DateTime.Now);
        }

        public override void Down()
        {
            Delete.Table("Students");
        }

    }
}