using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MntPlus.WPF.Migrations
{
    /// <inheritdoc />
    public partial class MntPlusMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assignees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentClasses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    EquipmentClassName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentDepartments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DepartmentName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentDepartments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    EquipmentStatusName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    EquipmentTypeName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrganizationName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    EquipmentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EquipmentParent = table.Column<Guid>(type: "TEXT", nullable: true),
                    EquipmentName = table.Column<string>(type: "TEXT", nullable: false),
                    EquipmentTypeId = table.Column<Guid>(type: "TEXT", nullable: true),
                    EquipmentDescription = table.Column<string>(type: "TEXT", nullable: true),
                    EquipmentOrganizationId = table.Column<Guid>(type: "TEXT", nullable: true),
                    EquipmentDepartmentId = table.Column<Guid>(type: "TEXT", nullable: true),
                    EquipmentClassId = table.Column<Guid>(type: "TEXT", nullable: true),
                    EquipmentSite = table.Column<string>(type: "TEXT", nullable: true),
                    EquipmentStatusId = table.Column<Guid>(type: "TEXT", nullable: true),
                    EquipmentMake = table.Column<string>(type: "TEXT", nullable: true),
                    EquipmentSerialNumber = table.Column<string>(type: "TEXT", nullable: true),
                    EquipmentModel = table.Column<string>(type: "TEXT", nullable: true),
                    EquipmentCost = table.Column<double>(type: "REAL", nullable: true),
                    EquipmentCommissionDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EquipmentAssignedToId = table.Column<Guid>(type: "TEXT", nullable: true),
                    EquipmentNameImage = table.Column<string>(type: "TEXT", nullable: true),
                    EquipmentImage = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.EquipmentId);
                    table.ForeignKey(
                        name: "FK_Equipment_Assignees_EquipmentAssignedToId",
                        column: x => x.EquipmentAssignedToId,
                        principalTable: "Assignees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Equipment_EquipmentClasses_EquipmentClassId",
                        column: x => x.EquipmentClassId,
                        principalTable: "EquipmentClasses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Equipment_EquipmentDepartments_EquipmentDepartmentId",
                        column: x => x.EquipmentDepartmentId,
                        principalTable: "EquipmentDepartments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Equipment_EquipmentStatuses_EquipmentStatusId",
                        column: x => x.EquipmentStatusId,
                        principalTable: "EquipmentStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Equipment_EquipmentTypes_EquipmentTypeId",
                        column: x => x.EquipmentTypeId,
                        principalTable: "EquipmentTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Equipment_Organizations_EquipmentOrganizationId",
                        column: x => x.EquipmentOrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_EquipmentAssignedToId",
                table: "Equipment",
                column: "EquipmentAssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_EquipmentClassId",
                table: "Equipment",
                column: "EquipmentClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_EquipmentDepartmentId",
                table: "Equipment",
                column: "EquipmentDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_EquipmentOrganizationId",
                table: "Equipment",
                column: "EquipmentOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_EquipmentStatusId",
                table: "Equipment",
                column: "EquipmentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_EquipmentTypeId",
                table: "Equipment",
                column: "EquipmentTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "Assignees");

            migrationBuilder.DropTable(
                name: "EquipmentClasses");

            migrationBuilder.DropTable(
                name: "EquipmentDepartments");

            migrationBuilder.DropTable(
                name: "EquipmentStatuses");

            migrationBuilder.DropTable(
                name: "EquipmentTypes");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
