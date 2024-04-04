using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MntPlus.API.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    EquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipmentParent = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EquipmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EquipmentCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EquipmentModel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EquipmentMake = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EquipmentNameImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EquipmentImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.EquipmentId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equipment");
        }
    }
}
