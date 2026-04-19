using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class AddDataInBrand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into Brands (name, description, statu) values ('Appel', 'Mobile  ', 1);insert into Brands (name, description, statu) values ('Samsung', 'Mobile  ', 1);insert into Brands (name, description, statu) values ('Oppo', 'Mobile  ', 0);insert into Brands (name, description, statu) values ('Huawei', 'Mobile  ', 0);insert into Brands (name, description, statu) values ('Appel', 'Mobile  ', 0);insert into Brands (name, description, statu) values ('Samsung', 'Mobile  ', 0);insert into Brands (name, description, statu) values ('Oppo', 'Mobile  ', 1);insert into Brands (name, description, statu) values ('Huawei', 'Mobile  ', 1);insert into Brands (name, description, statu) values ('Appel', 'Mobile  ', 0);insert into Brands (name, description, statu) values ('Samsung', 'Mobile  ', 1);");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from Brands ");
        }
    }
}
