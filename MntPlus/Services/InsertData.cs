using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public static class InsertData
    {
        public static void InsertDataToDB(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.InsertData(
                                        table: "Users",
                                        columns: ["Id", "FirstName", "LastName", "Email", "PhoneNumber", "UserName", "Password", "Status", "CreatedAt"],
                                        values: new object[,]
                                        {
                                                   { Guid.Parse("B04DD1F2-5FF9-4EA0-B7DE-58F5234D426E"), "Lamine", "Belkheir", "mail@d","01264","lamine","a123456","Actif",DateTime.Now}
                                        });


            migrationBuilder.InsertData(
                           table: "Roles",
                           columns: ["Id", "Name", "IsSeeded"],
                           values: new object[,]
                           {
           { Guid.Parse("F62520DE-F650-41C0-9FCA-D2D0B216612F"), "Ingénieur GMAO", true },
           { Guid.NewGuid(), "Responsable", true },
           { Guid.NewGuid(), "Demandeur", true }

                           });

            migrationBuilder.InsertData(
                              table: "UserRoles",
                              columns: ["Id", "UserId", "RoleId"],
                         values: new object[,]
                        {
          { Guid.NewGuid(), Guid.Parse("B04DD1F2-5FF9-4EA0-B7DE-58F5234D426E"), Guid.Parse("F62520DE-F650-41C0-9FCA-D2D0B216612F") },


                          });

            //insert into Locations table
            migrationBuilder.InsertData(
                               table: "Locations",
                                              columns: ["Id", "Name", "Address", "IsPrimaryLocation", "IdParent", "CreatedAt"],
                                                             values: new object[,]
                                                             {
          { Guid.NewGuid(), "Aflou", "Cite Bouaeea kada", true, null, DateTime.Now },
          { Guid.NewGuid(), "Location 2", "Address 2", true, null, DateTime.Now }

                });

            migrationBuilder.InsertData(
                                        table: "Assets",
                                        columns: ["Id", "AssetParent", "Name", "Description", "Status", "Category", "LocationId", "SerialNumber", "Model", "Make", "PurchaseCost", "ImagePath", "AssetImage", "AssetCommissionDate", "CreatedDate", "PurchaseDate"],
                                        values: new object[,]
                                        {
          { Guid.NewGuid(), null, "Asset 1", "Description 1", "Active", "Category 1",null, "Serial 1", "Model 1", "Make 1", 1000, "Path 1", null, DateTime.Now, DateTime.Now, DateTime.Now },
          { Guid.NewGuid(), null, "Asset 2", "Description 2", "Active", "Category 2", null, "Serial 2", "Model 2", "Make 2", 2000, "Path 2", null, DateTime.Now, DateTime.Now, DateTime.Now }

                });
        }
    }
}
