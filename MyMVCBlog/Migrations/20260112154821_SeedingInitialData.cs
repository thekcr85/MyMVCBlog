using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyMVCBlog.Migrations
{
    /// <inheritdoc />
    public partial class SeedingInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Posts related to technology.", "Technology" },
                    { 2, "Posts related to health.", "Health" },
                    { 3, "Posts related to lifestyle.", "Lifestyle" }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Author", "CategoryId", "Content", "FeatureImagePath", "PublishedDate", "Title" },
                values: new object[,]
                {
                    { 1, "Jane Doe", 1, "Artificial Intelligence (AI) is becoming increasingly prevalent in our daily lives...", "images/ai-rise.jpg", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Rise of AI in Everyday Life" },
                    { 2, "John Smith", 2, "Living a healthy lifestyle doesn't have to be complicated. Here are 10 simple tips...", "images/healthy-lifestyle.jpg", new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "10 Tips for a Healthier Lifestyle" },
                    { 3, "Alice Johnson", 3, "Minimalism is more than just a design trend; it's a lifestyle choice that emphasizes simplicity...", "images/minimalist-living.jpg", new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Exploring Minimalist Living" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
