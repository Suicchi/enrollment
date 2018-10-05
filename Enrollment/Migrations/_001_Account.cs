using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Enrollment.Migrations
{
    [Migration(1)]
    public class _001_Account : Migration
    {
        public override void Up()
        {
            Create.Table("Accounts")
                .WithColumn("id").AsInt32().Identity().PrimaryKey().NotNullable()
                .WithColumn("username").AsString(128).Unique().NotNullable()
                //.WithColumn("email").AsString(256).Unique()
                .WithColumn("password").AsString(128).NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Admins");
        }
    }
}