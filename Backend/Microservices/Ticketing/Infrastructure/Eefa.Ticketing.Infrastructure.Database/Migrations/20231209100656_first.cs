using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eefa.Ticketing.Infrastructure.Database.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Ticketing");

            migrationBuilder.CreateTable(
                name: "Tickets",
                schema: "Ticketing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "عنوان تیکت"),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    PageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "صفحه ای که روی آن تیکت خورده"),
                    RoleId = table.Column<int>(type: "int", nullable: false, comment: "دپارتمان دریافت کننده"),
                    RoleTitle = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "نام دپارتمان دریافت کننده"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "وضعیت: ایجاد شده=0، پاسخ کاربر ایجاد کننده=1، پاسخ کاربر دریافت کننده=2، ارجاع داده شده=3، در دست اقدام=4، بسته شده توسط کاربر ایجاد کننده=5، بسته شده توسط کاربر پاسخ دهنده=6بسته شده توسط سیستم=7"),
                    Priority = table.Column<int>(type: "int", nullable: false, comment: "اولیت"),
                    ReceiverUserId = table.Column<int>(type: "int", nullable: true, comment: "کاربر دریافت کننده"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    OwnerRoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketDetails",
                schema: "Ticketing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1200)", maxLength: 1200, nullable: false, comment: "توضیحات تیکت"),
                    AttachmentIds = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "شناسه فایل پیوست شده"),
                    ReadDate = table.Column<DateTime>(type: "datetime2(7)", nullable: true, comment: "تاریخ مشاهده تیکت توسط دریافت کننده"),
                    RoleId = table.Column<int>(type: "int", nullable: false, comment: "دپارتمان دریافت کننده"),
                    ReaderUserId = table.Column<int>(type: "int", nullable: true, comment: "کاربر خواننده تیکت"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    OwnerRoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketDetails_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalSchema: "Ticketing",
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetailHistories",
                schema: "Ticketing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketDetailId = table.Column<int>(type: "int", nullable: false, comment: "کلید خارجی از جزییات تیکت"),
                    PrimaryRoleId = table.Column<int>(type: "int", nullable: false, comment: "دپارتمان اولیه"),
                    SecondaryRoleId = table.Column<int>(type: "int", nullable: false, comment: "دپارتمان ارجاع داده شده"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    OwnerRoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetailHistories_TicketDetails_TicketDetailId",
                        column: x => x.TicketDetailId,
                        principalSchema: "Ticketing",
                        principalTable: "TicketDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrivetMessages",
                schema: "Ticketing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketDetailId = table.Column<int>(type: "int", nullable: false, comment: "کلید خارجی از جزییات تیکت"),
                    Message = table.Column<string>(type: "nvarchar(1200)", maxLength: 1200, nullable: false, comment: "پیام"),
                    DetailHistoryId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    OwnerRoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivetMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivetMessages_DetailHistories_DetailHistoryId",
                        column: x => x.DetailHistoryId,
                        principalSchema: "Ticketing",
                        principalTable: "DetailHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrivetMessages_TicketDetails_TicketDetailId",
                        column: x => x.TicketDetailId,
                        principalSchema: "Ticketing",
                        principalTable: "TicketDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetailHistories_TicketDetailId",
                schema: "Ticketing",
                table: "DetailHistories",
                column: "TicketDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivetMessages_DetailHistoryId",
                schema: "Ticketing",
                table: "PrivetMessages",
                column: "DetailHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivetMessages_TicketDetailId",
                schema: "Ticketing",
                table: "PrivetMessages",
                column: "TicketDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketDetails_TicketId",
                schema: "Ticketing",
                table: "TicketDetails",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_RoleId",
                schema: "Ticketing",
                table: "Tickets",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrivetMessages",
                schema: "Ticketing");

            migrationBuilder.DropTable(
                name: "DetailHistories",
                schema: "Ticketing");

            migrationBuilder.DropTable(
                name: "TicketDetails",
                schema: "Ticketing");

            migrationBuilder.DropTable(
                name: "Tickets",
                schema: "Ticketing");
        }
    }
}
