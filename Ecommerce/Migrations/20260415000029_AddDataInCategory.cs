using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class AddDataInCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into Categories (name, description, statu) values ('Mobile', 'M', 1);insert into Categories (name, description, statu) values ('LabTob', 'L', 0);insert into Categories (name, description, statu) values ('Tab', 'TA', 1);insert into Categories (name, description, statu) values ('TV', 'T', 0);");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from Categories ");

        }
    }
}
