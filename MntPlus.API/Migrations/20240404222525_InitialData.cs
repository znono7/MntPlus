using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MntPlus.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Equipment",
                columns: new[] { "EquipmentId", "EquipmentCategory", "EquipmentImage", "EquipmentMake", "EquipmentModel", "EquipmentName", "EquipmentNameImage", "EquipmentParent" },
                values: new object[,]
                {
                    { new Guid("208e643b-3694-47ac-ba1d-f57fb3b34930"), "Category B", new byte[0], "Make X", "Model Z", "Equipment 2", "equipment2.jpg", null },
                    { new Guid("ab837407-ff51-4c0c-a9e9-bbce2786a169"), "Category A", new byte[0], "Make Y", "Model X", "Equipment 1", "equipment1.jpg", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Equipment",
                keyColumn: "EquipmentId",
                keyValue: new Guid("208e643b-3694-47ac-ba1d-f57fb3b34930"));

            migrationBuilder.DeleteData(
                table: "Equipment",
                keyColumn: "EquipmentId",
                keyValue: new Guid("ab837407-ff51-4c0c-a9e9-bbce2786a169"));
        }
    }
}
