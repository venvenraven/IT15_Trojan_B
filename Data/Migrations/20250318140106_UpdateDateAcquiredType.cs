﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IT15_Trojan_B.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDateAcquiredType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateAcquired",
                table: "Materials",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateAcquired",
                table: "Materials",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }
    }
}
