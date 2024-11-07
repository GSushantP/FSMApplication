using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTechnicianServiceRequestRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_Technicians_TechnicianId",
                table: "ServiceRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_Technicians_TechnicianId",
                table: "ServiceRequests",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_Technicians_TechnicianId",
                table: "ServiceRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_Technicians_TechnicianId",
                table: "ServiceRequests",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "Id");
        }
    }
}
