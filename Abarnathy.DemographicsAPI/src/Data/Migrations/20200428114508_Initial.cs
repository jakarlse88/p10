﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Abarnathy.DemographicsAPI.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StreetName = table.Column<string>(maxLength: 40, nullable: false),
                    HouseNumber = table.Column<string>(maxLength: 6, nullable: false),
                    Town = table.Column<string>(maxLength: 40, nullable: false),
                    State = table.Column<string>(maxLength: 20, nullable: false),
                    ZIPCode = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhoneNumber",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneNumber", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sex",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sex", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GivenName = table.Column<string>(maxLength: 50, nullable: false),
                    FamilyName = table.Column<string>(maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    SexId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patient_Sex",
                        column: x => x.SexId,
                        principalTable: "Sex",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientAddress",
                columns: table => new
                {
                    PatientId = table.Column<int>(nullable: false),
                    AddressId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAddress", x => new { x.PatientId, x.AddressId });
                    table.ForeignKey(
                        name: "FK_PatientAddress_Address",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientAddress_Patient",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientPhoneNumbers",
                columns: table => new
                {
                    PatientId = table.Column<int>(nullable: false),
                    PhoneNumberId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientPhoneNumbers", x => new { x.PatientId, x.PhoneNumberId });
                    table.ForeignKey(
                        name: "FK_PatientPhoneNumber_Patient",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientPhoneNumber_PhoneNumber",
                        column: x => x.PhoneNumberId,
                        principalTable: "PhoneNumber",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Sex",
                columns: new[] { "Id", "Type" },
                values: new object[] { 1, "Male" });

            migrationBuilder.InsertData(
                table: "Sex",
                columns: new[] { "Id", "Type" },
                values: new object[] { 2, "Female" });

            migrationBuilder.CreateIndex(
                name: "IX_Patient_SexId",
                table: "Patient",
                column: "SexId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAddress_AddressId",
                table: "PatientAddress",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPhoneNumbers_PhoneNumberId",
                table: "PatientPhoneNumbers",
                column: "PhoneNumberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientAddress");

            migrationBuilder.DropTable(
                name: "PatientPhoneNumbers");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.DropTable(
                name: "PhoneNumber");

            migrationBuilder.DropTable(
                name: "Sex");
        }
    }
}
