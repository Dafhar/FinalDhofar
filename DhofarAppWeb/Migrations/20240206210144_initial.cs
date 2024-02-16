using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DhofarAppWeb.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdentityNumber = table.Column<int>(type: "int", nullable: false),
                    Likes = table.Column<int>(type: "int", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JoinedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityNumbers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityNumbers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubjectTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Visitors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JoinedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deviceTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deviceTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_deviceTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Complaints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentTypeId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complaints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Complaints_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Complaints_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Complaints_DepartmentTypes_DepartmentTypeId",
                        column: x => x.DepartmentTypeId,
                        principalTable: "DepartmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentAdmins",
                columns: table => new
                {
                    DepartmentTypeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentAdmins", x => new { x.DepartmentTypeId, x.UserId });
                    table.ForeignKey(
                        name: "FK_DepartmentAdmins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartmentAdmins_DepartmentTypes_DepartmentTypeId",
                        column: x => x.DepartmentTypeId,
                        principalTable: "DepartmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubjectTypeId = table.Column<int>(type: "int", nullable: false),
                    PrimarySubject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LikeCounter = table.Column<int>(type: "int", nullable: false),
                    DisLikeCounter = table.Column<int>(type: "int", nullable: false),
                    VisitorCounter = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subjects_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subjects_SubjectTypes_SubjectTypeId",
                        column: x => x.SubjectTypeId,
                        principalTable: "SubjectTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComplaintsFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilePaths = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Filename = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComplaintId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintsFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplaintsFiles_Complaints_ComplaintId",
                        column: x => x.ComplaintId,
                        principalTable: "Complaints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommentTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpVoteCounter = table.Column<int>(type: "int", nullable: false),
                    DownVoteCounter = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentSubjects_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteSubjects",
                columns: table => new
                {
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteSubjects", x => new { x.UserId, x.SubjectId });
                    table.ForeignKey(
                        name: "FK_FavoriteSubjects_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FavoriteSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Polls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Polls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Polls_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RatingSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsLike = table.Column<bool>(type: "bit", nullable: false),
                    IsDisLike = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RatingSubjects_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RatingSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubjectFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilePaths = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Filename = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectFiles_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentReplies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReplyComment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommentSubjectId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentReplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentReplies_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentReplies_CommentSubjects_CommentSubjectId",
                        column: x => x.CommentSubjectId,
                        principalTable: "CommentSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCommentVotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentSubjectId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VoteUp = table.Column<bool>(type: "bit", nullable: false),
                    VoteDown = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCommentVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCommentVotes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserCommentVotes_CommentSubjects_CommentSubjectId",
                        column: x => x.CommentSubjectId,
                        principalTable: "CommentSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PollOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PollId = table.Column<int>(type: "int", nullable: false),
                    OptionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoteCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PollOptions_Polls_PollId",
                        column: x => x.PollId,
                        principalTable: "Polls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserVotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PollId = table.Column<int>(type: "int", nullable: false),
                    PollOptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserVotes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserVotes_PollOptions_PollOptionId",
                        column: x => x.PollOptionId,
                        principalTable: "PollOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserVotes_Polls_PollId",
                        column: x => x.PollId,
                        principalTable: "Polls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "admin", "00000000-0000-0000-0000-000000000000", "Admin", "ADMIN" },
                    { "super admin", "00000000-0000-0000-0000-000000000000", "Super Admin", "SUPER ADMIN" },
                    { "suspict", "00000000-0000-0000-0000-000000000000", "Suspict", "SUSPICT" },
                    { "user", "00000000-0000-0000-0000-000000000000", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1, "أفغانستان", "Afghanistan" },
                    { 2, "ألبانيا", "Albania" },
                    { 3, "الجزائر", "Algeria" },
                    { 4, "أندورا", "Andorra" },
                    { 5, "أنغولا", "Angola" },
                    { 6, "أنتيغوا وبربودا", "Antigua and Barbuda" },
                    { 7, "الأرجنتين", "Argentina" },
                    { 8, "أرمينيا", "Armenia" },
                    { 9, "أستراليا", "Australia" },
                    { 10, "النمسا", "Austria" },
                    { 11, "أذربيجان", "Azerbaijan" },
                    { 12, "الباهاما", "Bahamas" },
                    { 13, "البحرين", "Bahrain" },
                    { 14, "بنجلاديش", "Bangladesh" },
                    { 15, "باربادوس", "Barbados" },
                    { 16, "بيلاروس", "Belarus" },
                    { 17, "بلجيكا", "Belgium" },
                    { 18, "بليز", "Belize" },
                    { 19, "بنين", "Benin" },
                    { 20, "بوتان", "Bhutan" },
                    { 21, "بوليفيا", "Bolivia" },
                    { 22, "البوسنة والهرسك", "Bosnia and Herzegovina" },
                    { 23, "بتسوانا", "Botswana" },
                    { 24, "البرازيل", "Brazil" },
                    { 25, "بروناي", "Brunei" },
                    { 26, "بلغاريا", "Bulgaria" },
                    { 27, "بوركينا فاسو", "Burkina Faso" },
                    { 28, "بوروندي", "Burundi" },
                    { 29, "الرأس الأخضر", "Cabo Verde" },
                    { 30, "كمبوديا", "Cambodia" },
                    { 31, "الكاميرون", "Cameroon" },
                    { 32, "كندا", "Canada" },
                    { 33, "جمهورية أفريقيا الوسطى", "Central African Republic" },
                    { 34, "تشاد", "Chad" },
                    { 35, "تشيلي", "Chile" },
                    { 36, "الصين", "China" },
                    { 37, "كولومبيا", "Colombia" },
                    { 38, "جزر القمر", "Comoros" },
                    { 39, "الكونغو (برازافيل)", "Congo (Congo-Brazzaville)" },
                    { 40, "كوستاريكا", "Costa Rica" },
                    { 41, "ساحل العاج", "Cote d'Ivoire" },
                    { 42, "كرواتيا", "Croatia" },
                    { 43, "كوبا", "Cuba" },
                    { 44, "قبرص", "Cyprus" },
                    { 45, "التشيك", "Czechia" },
                    { 46, "الدنمارك", "Denmark" },
                    { 47, "جيبوتي", "Djibouti" },
                    { 48, "دومينيكا", "Dominica" },
                    { 49, "جمهورية الدومينيكان", "Dominican Republic" },
                    { 50, "الإكوادور", "Ecuador" },
                    { 51, "مصر", "Egypt" },
                    { 52, "السلفادور", "El Salvador" },
                    { 53, "غينيا الاستوائية", "Equatorial Guinea" },
                    { 54, "إريتريا", "Eritrea" },
                    { 55, "إستونيا", "Estonia" },
                    { 56, "إيسواتيني", "Eswatini" },
                    { 57, "إثيوبيا", "Ethiopia" },
                    { 58, "فيجي", "Fiji" },
                    { 59, "فنلندا", "Finland" },
                    { 60, "فرنسا", "France" },
                    { 61, "الغابون", "Gabon" },
                    { 62, "غامبيا", "Gambia" },
                    { 63, "جورجيا", "Georgia" },
                    { 64, "ألمانيا", "Germany" },
                    { 65, "غانا", "Ghana" },
                    { 66, "اليونان", "Greece" },
                    { 67, "غرينادا", "Grenada" },
                    { 68, "غواتيمالا", "Guatemala" },
                    { 69, "غينيا", "Guinea" },
                    { 70, "غينيا بيساو", "Guinea-Bissau" },
                    { 71, "غيانا", "Guyana" },
                    { 72, "هايتي", "Haiti" },
                    { 73, "هندوراس", "Honduras" },
                    { 74, "هنغاريا", "Hungary" },
                    { 75, "أيسلندا", "Iceland" },
                    { 76, "الهند", "India" },
                    { 77, "إندونيسيا", "Indonesia" },
                    { 78, "إيران", "Iran" },
                    { 79, "العراق", "Iraq" },
                    { 80, "أيرلندا", "Ireland" },
                    { 81, "إيطاليا", "Italy" },
                    { 82, "جامايكا", "Jamaica" },
                    { 83, "اليابان", "Japan" },
                    { 84, "الأردن", "Jordan" },
                    { 85, "كازاخستان", "Kazakhstan" },
                    { 86, "كينيا", "Kenya" },
                    { 87, "كيريباتي", "Kiribati" },
                    { 88, "كوريا الشمالية", "Korea, North" },
                    { 89, "كوريا الجنوبية", "Korea, South" },
                    { 90, "الكويت", "Kuwait" },
                    { 91, "قيرغيزستان", "Kyrgyzstan" },
                    { 92, "لاوس", "Laos" },
                    { 93, "لاتفيا", "Latvia" },
                    { 94, "لبنان", "Lebanon" },
                    { 95, "ليسوتو", "Lesotho" },
                    { 96, "ليبيريا", "Liberia" },
                    { 97, "ليبيا", "Libya" },
                    { 98, "ليختنشتاين", "Liechtenstein" },
                    { 99, "ليتوانيا", "Lithuania" },
                    { 100, "لوكسمبورغ", "Luxembourg" },
                    { 101, "مدغشقر", "Madagascar" },
                    { 102, "ملاوي", "Malawi" },
                    { 103, "ماليزيا", "Malaysia" },
                    { 104, "جزر المالديف", "Maldives" },
                    { 105, "مالي", "Mali" },
                    { 106, "مالطا", "Malta" },
                    { 107, "جزر مارشال", "Marshall Islands" },
                    { 108, "موريتانيا", "Mauritania" },
                    { 109, "موريشيوس", "Mauritius" },
                    { 110, "المكسيك", "Mexico" },
                    { 111, "ميكرونيزيا", "Micronesia" },
                    { 112, "مولدوفا", "Moldova" },
                    { 113, "موناكو", "Monaco" },
                    { 114, "منغوليا", "Mongolia" },
                    { 115, "الجبل الأسود", "Montenegro" },
                    { 116, "المغرب", "Morocco" },
                    { 117, "موزمبيق", "Mozambique" },
                    { 118, "ميانمار (بورما)", "Myanmar (formerly Burma)" },
                    { 119, "ناميبيا", "Namibia" },
                    { 120, "ناورو", "Nauru" },
                    { 121, "نيبال", "Nepal" },
                    { 122, "هولندا", "Netherlands" },
                    { 123, "نيوزيلندا", "New Zealand" },
                    { 124, "نيكاراغوا", "Nicaragua" },
                    { 125, "النيجر", "Niger" },
                    { 126, "نيجيريا", "Nigeria" },
                    { 127, "مقدونيا الشمالية", "North Macedonia" },
                    { 128, "النرويج", "Norway" },
                    { 129, "عُمان", "Oman" },
                    { 130, "باكستان", "Pakistan" },
                    { 131, "بالاو", "Palau" },
                    { 132, "بنما", "Panama" },
                    { 133, "بابوا غينيا الجديدة", "Papua New Guinea" },
                    { 134, "باراغواي", "Paraguay" },
                    { 135, "بيرو", "Peru" },
                    { 136, "الفلبين", "Philippines" },
                    { 137, "بولندا", "Poland" },
                    { 138, "البرتغال", "Portugal" },
                    { 139, "قطر", "Qatar" },
                    { 140, "رومانيا", "Romania" },
                    { 141, "روسيا", "Russia" },
                    { 142, "رواندا", "Rwanda" },
                    { 143, "سانت كيتس ونيفيس", "Saint Kitts and Nevis" },
                    { 144, "سانت لوسيا", "Saint Lucia" },
                    { 145, "سانت فينسنت والغرينادين", "Saint Vincent and the Grenadines" },
                    { 146, "ساموا", "Samoa" },
                    { 147, "سان مارينو", "San Marino" },
                    { 148, "ساو تومي وبرينسيبي", "Sao Tome and Principe" },
                    { 149, "المملكة العربية السعودية", "Saudi Arabia" },
                    { 150, "السنغال", "Senegal" },
                    { 151, "صربيا", "Serbia" },
                    { 152, "سيشل", "Seychelles" },
                    { 153, "سيراليون", "Sierra Leone" },
                    { 154, "سنغافورة", "Singapore" },
                    { 155, "سلوفاكيا", "Slovakia" },
                    { 156, "سلوفينيا", "Slovenia" },
                    { 157, "جزر سليمان", "Solomon Islands" },
                    { 158, "الصومال", "Somalia" },
                    { 159, "جنوب أفريقيا", "South Africa" },
                    { 160, "جنوب السودان", "South Sudan" },
                    { 161, "إسبانيا", "Spain" },
                    { 162, "سريلانكا", "Sri Lanka" },
                    { 163, "السودان", "Sudan" },
                    { 164, "سورينام", "Suriname" },
                    { 165, "السويد", "Sweden" },
                    { 166, "سويسرا", "Switzerland" },
                    { 167, "سوريا", "Syria" },
                    { 168, "تايوان", "Taiwan" },
                    { 169, "طاجيكستان", "Tajikistan" },
                    { 170, "تنزانيا", "Tanzania" },
                    { 171, "تايلاند", "Thailand" },
                    { 172, "تيمور الشرقية", "Timor-Leste" },
                    { 173, "توغو", "Togo" },
                    { 174, "تونغا", "Tonga" },
                    { 175, "ترينيداد وتوباغو", "Trinidad and Tobago" },
                    { 176, "تونس", "Tunisia" },
                    { 177, "تركيا", "Turkey" },
                    { 178, "تركمانستان", "Turkmenistan" },
                    { 179, "توفالو", "Tuvalu" },
                    { 180, "أوغندا", "Uganda" },
                    { 181, "أوكرانيا", "Ukraine" },
                    { 182, "الإمارات العربية المتحدة", "United Arab Emirates" },
                    { 183, "المملكة المتحدة", "United Kingdom" },
                    { 184, "الولايات المتحدة الأمريكية", "United States of America" },
                    { 185, "أوروغواي", "Uruguay" },
                    { 186, "أوزبكستان", "Uzbekistan" },
                    { 187, "فانواتو", "Vanuatu" },
                    { 188, "الفاتيكان", "Vatican City" },
                    { 189, "فنزويلا", "Venezuela" },
                    { 190, "فيتنام", "Vietnam" },
                    { 191, "اليمن", "Yemen" },
                    { 192, "زامبيا", "Zambia" },
                    { 193, "زيمبابوي", "Zimbabwe" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CommentReplies_CommentSubjectId",
                table: "CommentReplies",
                column: "CommentSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentReplies_UserId",
                table: "CommentReplies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentSubjects_SubjectId",
                table: "CommentSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentSubjects_UserId",
                table: "CommentSubjects",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_CategoryId",
                table: "Complaints",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_DepartmentTypeId",
                table: "Complaints",
                column: "DepartmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_UserId",
                table: "Complaints",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintsFiles_ComplaintId",
                table: "ComplaintsFiles",
                column: "ComplaintId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentAdmins_UserId",
                table: "DepartmentAdmins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_deviceTokens_UserId",
                table: "deviceTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteSubjects_SubjectId",
                table: "FavoriteSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PollOptions_PollId",
                table: "PollOptions",
                column: "PollId");

            migrationBuilder.CreateIndex(
                name: "IX_Polls_SubjectId",
                table: "Polls",
                column: "SubjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RatingSubjects_SubjectId",
                table: "RatingSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingSubjects_UserId",
                table: "RatingSubjects",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectFiles_SubjectId",
                table: "SubjectFiles",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SubjectTypeId",
                table: "Subjects",
                column: "SubjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_UserId",
                table: "Subjects",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCommentVotes_CommentSubjectId",
                table: "UserCommentVotes",
                column: "CommentSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCommentVotes_UserId",
                table: "UserCommentVotes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserVotes_PollId",
                table: "UserVotes",
                column: "PollId");

            migrationBuilder.CreateIndex(
                name: "IX_UserVotes_PollOptionId",
                table: "UserVotes",
                column: "PollOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserVotes_UserId",
                table: "UserVotes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CommentReplies");

            migrationBuilder.DropTable(
                name: "ComplaintsFiles");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "DepartmentAdmins");

            migrationBuilder.DropTable(
                name: "deviceTokens");

            migrationBuilder.DropTable(
                name: "FavoriteSubjects");

            migrationBuilder.DropTable(
                name: "IdentityNumbers");

            migrationBuilder.DropTable(
                name: "RatingSubjects");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "SubjectFiles");

            migrationBuilder.DropTable(
                name: "UserCommentVotes");

            migrationBuilder.DropTable(
                name: "UserVotes");

            migrationBuilder.DropTable(
                name: "Visitors");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Complaints");

            migrationBuilder.DropTable(
                name: "CommentSubjects");

            migrationBuilder.DropTable(
                name: "PollOptions");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "DepartmentTypes");

            migrationBuilder.DropTable(
                name: "Polls");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SubjectTypes");
        }
    }
}
