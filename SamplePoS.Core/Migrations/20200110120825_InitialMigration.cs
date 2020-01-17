using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SamplePoS.Core.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: true),
                    ContactNumber = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: true),
                    ContactNumber = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    VendorId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.VendorId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Time = table.Column<DateTime>(nullable: false),
                    CustomerId = table.Column<int>(nullable: true),
                    OrderDiscount = table.Column<decimal>(nullable: false),
                    SubTotal = table.Column<decimal>(nullable: false),
                    Total = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VendorId = table.Column<int>(nullable: true),
                    CategoryId = table.Column<int>(nullable: false),
                    BuyingPrice = table.Column<decimal>(nullable: false),
                    SellingPrice = table.Column<decimal>(nullable: false),
                    ProductCode = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Quantity = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(nullable: false),
                    LineDiscount = table.Column<decimal>(nullable: false),
                    Quantity = table.Column<decimal>(nullable: false),
                    OrderId = table.Column<int>(nullable: false),
                    SubTotal = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 1, "Juices" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 2, "Meals" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 3, "Starters" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Address", "ContactNumber", "Email", "Name" },
                values: new object[] { 1, null, "0772135250", null, "Mr.Raqeeb" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Address", "ContactNumber", "Email", "Name" },
                values: new object[] { 2, null, "0772135250", null, "Prince Hardware" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Address", "ContactNumber", "Email", "Name" },
                values: new object[] { 3, null, "0772135250", null, "Royal GH" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Address", "ContactNumber", "Email", "Name" },
                values: new object[] { 4, null, "0772135250", null, "Trans Asia Cellulor" });

            migrationBuilder.InsertData(
                table: "Vendors",
                columns: new[] { "VendorId", "Address", "ContactNumber", "Email", "Name" },
                values: new object[] { 1, null, "0772135250", null, "Kingsburry" });

            migrationBuilder.InsertData(
                table: "Vendors",
                columns: new[] { "VendorId", "Address", "ContactNumber", "Email", "Name" },
                values: new object[] { 2, null, "0772135250", null, "Ranjitha" });

            migrationBuilder.InsertData(
                table: "Vendors",
                columns: new[] { "VendorId", "Address", "ContactNumber", "Email", "Name" },
                values: new object[] { 3, null, "0772135250", null, "Ravi Jewellers" });

            migrationBuilder.InsertData(
                table: "Vendors",
                columns: new[] { "VendorId", "Address", "ContactNumber", "Email", "Name" },
                values: new object[] { 4, null, "0772135250", null, "Globe Glass" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "BuyingPrice", "CategoryId", "Description", "Name", "ProductCode", "Quantity", "SellingPrice", "VendorId" },
                values: new object[] { 1, 10m, 1, "What do you expect!", "Apple Juice", null, 0m, 20m, null });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "BuyingPrice", "CategoryId", "Description", "Name", "ProductCode", "Quantity", "SellingPrice", "VendorId" },
                values: new object[] { 2, 10m, 1, "What do you expect!", "Strawberry Juice", null, 0m, 30m, null });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "BuyingPrice", "CategoryId", "Description", "Name", "ProductCode", "Quantity", "SellingPrice", "VendorId" },
                values: new object[] { 3, 20m, 1, "What do you expect!", "Pineapple Juice", null, 0m, 50m, null });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "BuyingPrice", "CategoryId", "Description", "Name", "ProductCode", "Quantity", "SellingPrice", "VendorId" },
                values: new object[] { 4, 100m, 2, "What do you expect!", "Vegetable Rice and Curry", null, 0m, 180m, null });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "BuyingPrice", "CategoryId", "Description", "Name", "ProductCode", "Quantity", "SellingPrice", "VendorId" },
                values: new object[] { 5, 125m, 2, "What do you expect!", "Chicken Rice and Curry", null, 0m, 250m, null });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "BuyingPrice", "CategoryId", "Description", "Name", "ProductCode", "Quantity", "SellingPrice", "VendorId" },
                values: new object[] { 6, 140m, 2, "What do you expect!", "Beef Rice and Curry", null, 0m, 250m, null });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "BuyingPrice", "CategoryId", "Description", "Name", "ProductCode", "Quantity", "SellingPrice", "VendorId" },
                values: new object[] { 7, 110m, 2, "What do you expect!", "Fish Rice and Curry", null, 0m, 210m, null });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "BuyingPrice", "CategoryId", "Description", "Name", "ProductCode", "Quantity", "SellingPrice", "VendorId" },
                values: new object[] { 8, 250m, 3, "What do you expect!", "Fried Sea Food Dish", null, 0m, 660m, null });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "BuyingPrice", "CategoryId", "Description", "Name", "ProductCode", "Quantity", "SellingPrice", "VendorId" },
                values: new object[] { 9, 280m, 3, "What do you expect!", "Batter fried Cuttle Fish", null, 0m, 320m, null });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_VendorId",
                table: "Products",
                column: "VendorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Vendors");
        }
    }
}
