﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MntPlus.WPF.Migrations
{
    /// <inheritdoc />
    public partial class mntPlusMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IndividualTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    IsPrimaryLocation = table.Column<bool>(type: "INTEGER", nullable: false),
                    IdParent = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_Locations_IdParent",
                        column: x => x.IdParent,
                        principalTable: "Locations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Parts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PartNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Category = table.Column<string>(type: "TEXT", nullable: true),
                    Image = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsSeeded = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FrequencyType = table.Column<string>(type: "TEXT", nullable: false),
                    Interval = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ScheduleType = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false),
                    DayOfMonth = table.Column<int>(type: "INTEGER", nullable: true),
                    WeekOfMonth = table.Column<int>(type: "INTEGER", nullable: true),
                    DayOfWeek = table.Column<int>(type: "INTEGER", nullable: true),
                    DaysOfWeek = table.Column<string>(type: "TEXT", nullable: true),
                    YearlyNumericSchedule_DayOfMonth = table.Column<int>(type: "INTEGER", nullable: true),
                    Month = table.Column<int>(type: "INTEGER", nullable: true),
                    YearlyOrdinalSchedule_WeekOfMonth = table.Column<int>(type: "INTEGER", nullable: true),
                    YearlyOrdinalSchedule_DayOfWeek = table.Column<int>(type: "INTEGER", nullable: true),
                    YearlyOrdinalSchedule_Month = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    LastName = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CheckListItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CheckListId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IndividualTaskId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckListItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckListItems_CheckLists_CheckListId",
                        column: x => x.CheckListId,
                        principalTable: "CheckLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckListItems_IndividualTasks_IndividualTaskId",
                        column: x => x.IndividualTaskId,
                        principalTable: "IndividualTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AssetParent = table.Column<Guid>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    Category = table.Column<string>(type: "TEXT", nullable: true),
                    LocationId = table.Column<Guid>(type: "TEXT", nullable: true),
                    SerialNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Model = table.Column<string>(type: "TEXT", nullable: true),
                    Make = table.Column<string>(type: "TEXT", nullable: true),
                    PurchaseCost = table.Column<double>(type: "REAL", nullable: true),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    AssetImage = table.Column<byte[]>(type: "BLOB", nullable: true),
                    AssetCommissionDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assets_Assets_AssetParent",
                        column: x => x.AssetParent,
                        principalTable: "Assets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Assets_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    Cost = table.Column<double>(type: "REAL", nullable: true),
                    AvailableQuantity = table.Column<int>(type: "INTEGER", nullable: true),
                    MinimumQuantity = table.Column<int>(type: "INTEGER", nullable: true),
                    MaxQuantity = table.Column<int>(type: "INTEGER", nullable: true),
                    DateReceived = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PartID = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventories_Parts_PartID",
                        column: x => x.PartID,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTeams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TeamId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTeams_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LinkParts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PartId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AssetId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinkParts_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LinkParts_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Meters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    CurrentReading = table.Column<double>(type: "REAL", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Unit = table.Column<string>(type: "TEXT", nullable: true),
                    Frequency = table.Column<int>(type: "INTEGER", nullable: false),
                    FrequencyUnit = table.Column<string>(type: "TEXT", nullable: true),
                    AssetId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meters_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Priority = table.Column<string>(type: "TEXT", nullable: true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    Requester = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserCreatedId = table.Column<Guid>(type: "TEXT", nullable: true),
                    UserAssignedToId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TeamAssignedToId = table.Column<Guid>(type: "TEXT", nullable: true),
                    AssetId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requests_Teams_TeamAssignedToId",
                        column: x => x.TeamAssignedToId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requests_Users_UserAssignedToId",
                        column: x => x.UserAssignedToId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requests_Users_UserCreatedId",
                        column: x => x.UserCreatedId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Priority = table.Column<string>(type: "TEXT", nullable: true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    Requester = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserCreatedId = table.Column<Guid>(type: "TEXT", nullable: true),
                    UserAssignedToId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TeamAssignedToId = table.Column<Guid>(type: "TEXT", nullable: true),
                    AssetId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrders_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkOrders_Teams_TeamAssignedToId",
                        column: x => x.TeamAssignedToId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkOrders_Users_UserAssignedToId",
                        column: x => x.UserAssignedToId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkOrders_Users_UserCreatedId",
                        column: x => x.UserCreatedId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MeterReadings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MeterId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Reading = table.Column<double>(type: "REAL", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeterReadings_Meters_MeterId",
                        column: x => x.MeterId,
                        principalTable: "Meters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeterReadings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MeterSchedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FrequencyType = table.Column<string>(type: "TEXT", nullable: false),
                    Interval = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MeterId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeterSchedule_Meters_MeterId",
                        column: x => x.MeterId,
                        principalTable: "Meters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrderHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    DateChanged = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ChangedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    WorkOrderId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrderHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrderHistories_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkOrderHistories_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreventiveMaintenances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    FrequencyType = table.Column<string>(type: "TEXT", nullable: true),
                    Priority = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    LastPerformed = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NextDue = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateCreation = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserCreatedId = table.Column<Guid>(type: "TEXT", nullable: true),
                    UserAssignedToId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TeamAssignedToId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CheckListId = table.Column<Guid>(type: "TEXT", nullable: true),
                    AssetId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ScheduleId = table.Column<Guid>(type: "TEXT", nullable: true),
                    MeterScheduleId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreventiveMaintenances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreventiveMaintenances_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PreventiveMaintenances_CheckLists_CheckListId",
                        column: x => x.CheckListId,
                        principalTable: "CheckLists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PreventiveMaintenances_MeterSchedule_MeterScheduleId",
                        column: x => x.MeterScheduleId,
                        principalTable: "MeterSchedule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PreventiveMaintenances_Schedule_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PreventiveMaintenances_Teams_TeamAssignedToId",
                        column: x => x.TeamAssignedToId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PreventiveMaintenances_Users_UserAssignedToId",
                        column: x => x.UserAssignedToId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PreventiveMaintenances_Users_UserCreatedId",
                        column: x => x.UserCreatedId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PreventiveMaintenanceHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    DateChanged = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ChangedById = table.Column<Guid>(type: "TEXT", nullable: true),
                    PreventiveMaintenanceId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreventiveMaintenanceHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreventiveMaintenanceHistories_PreventiveMaintenances_PreventiveMaintenanceId",
                        column: x => x.PreventiveMaintenanceId,
                        principalTable: "PreventiveMaintenances",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PreventiveMaintenanceHistories_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetParent",
                table: "Assets",
                column: "AssetParent");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_LocationId",
                table: "Assets",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckListItems_CheckListId",
                table: "CheckListItems",
                column: "CheckListId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckListItems_IndividualTaskId",
                table: "CheckListItems",
                column: "IndividualTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_PartID",
                table: "Inventories",
                column: "PartID");

            migrationBuilder.CreateIndex(
                name: "IX_LinkParts_AssetId",
                table: "LinkParts",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkParts_PartId",
                table: "LinkParts",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_IdParent",
                table: "Locations",
                column: "IdParent");

            migrationBuilder.CreateIndex(
                name: "IX_MeterReadings_MeterId",
                table: "MeterReadings",
                column: "MeterId");

            migrationBuilder.CreateIndex(
                name: "IX_MeterReadings_UserId",
                table: "MeterReadings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Meters_AssetId",
                table: "Meters",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_MeterSchedule_MeterId",
                table: "MeterSchedule",
                column: "MeterId");

            migrationBuilder.CreateIndex(
                name: "IX_PreventiveMaintenanceHistories_ChangedById",
                table: "PreventiveMaintenanceHistories",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_PreventiveMaintenanceHistories_PreventiveMaintenanceId",
                table: "PreventiveMaintenanceHistories",
                column: "PreventiveMaintenanceId");

            migrationBuilder.CreateIndex(
                name: "IX_PreventiveMaintenances_AssetId",
                table: "PreventiveMaintenances",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_PreventiveMaintenances_CheckListId",
                table: "PreventiveMaintenances",
                column: "CheckListId");

            migrationBuilder.CreateIndex(
                name: "IX_PreventiveMaintenances_MeterScheduleId",
                table: "PreventiveMaintenances",
                column: "MeterScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_PreventiveMaintenances_ScheduleId",
                table: "PreventiveMaintenances",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_PreventiveMaintenances_TeamAssignedToId",
                table: "PreventiveMaintenances",
                column: "TeamAssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_PreventiveMaintenances_UserAssignedToId",
                table: "PreventiveMaintenances",
                column: "UserAssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_PreventiveMaintenances_UserCreatedId",
                table: "PreventiveMaintenances",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_AssetId",
                table: "Requests",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_TeamAssignedToId",
                table: "Requests",
                column: "TeamAssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_UserAssignedToId",
                table: "Requests",
                column: "UserAssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_UserCreatedId",
                table: "Requests",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_TeamId",
                table: "UserTeams",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_UserId",
                table: "UserTeams",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderHistories_ChangedById",
                table: "WorkOrderHistories",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrderHistories_WorkOrderId",
                table: "WorkOrderHistories",
                column: "WorkOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_AssetId",
                table: "WorkOrders",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_TeamAssignedToId",
                table: "WorkOrders",
                column: "TeamAssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_UserAssignedToId",
                table: "WorkOrders",
                column: "UserAssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_UserCreatedId",
                table: "WorkOrders",
                column: "UserCreatedId");
            InsertData.InsertDataToDB(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckListItems");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "LinkParts");

            migrationBuilder.DropTable(
                name: "MeterReadings");

            migrationBuilder.DropTable(
                name: "PreventiveMaintenanceHistories");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTeams");

            migrationBuilder.DropTable(
                name: "WorkOrderHistories");

            migrationBuilder.DropTable(
                name: "IndividualTasks");

            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.DropTable(
                name: "PreventiveMaintenances");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "WorkOrders");

            migrationBuilder.DropTable(
                name: "CheckLists");

            migrationBuilder.DropTable(
                name: "MeterSchedule");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Meters");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
