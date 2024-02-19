using DhofarAppWeb.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DhofarApp.Data.Seeder;
using System.Reflection.Emit;

namespace DhofarAppWeb.Data
{
    public class AppDbContext: IdentityDbContext<User>

    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Complaint> Complaints { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<SubCategory> SubCategories { get; set; }

        public DbSet<ComplaintsFile> ComplaintsFiles { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<SubjectType> SubjectTypes { get; set; }

        public DbSet<SubjectFiles> SubjectFiles { get; set; }

        public DbSet<GeneralSubjectsType> GeneralSubjectsTypes { get; set; }

        public DbSet<RatingSubject> RatingSubjects { get; set; }

        public DbSet<CommentSubject> CommentSubjects { get; set; }

        public DbSet<FavoriteSubject> FavoriteSubjects { get; set; }

        public DbSet<UserCommentVote> UserCommentVotes { get; set; }

        public DbSet<CommentReplies> CommentReplies { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<IdentityNumber> IdentityNumbers { get; set; }

        public DbSet<DeviceToken> deviceTokens { get; set; }

        public DbSet<Poll> Polls { get; set; }

        public DbSet<PollOption> PollOptions { get; set; }

        public DbSet<UserVote> UserVotes { get; set; }

        public DbSet<DepartmentType> DepartmentTypes { get; set; }

        public DbSet<DepartmentAdmin> DepartmentAdmins { get; set; }

        public DbSet<Visitor> Visitors { get; set; }

        public DbSet<Colors> Colors { get; set; }

        public DbSet<OnBoardScreen> OnBoardScreens { get; set; }

        public DbSet<SubjectTypeSubject> SubjectTypeSubjects { get; set; }




        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            new CountriesSeeder(builder);


            ////// auto generate the customId 
            //builder.Entity<User>()
            //    .Property(u => u.CustomId)
            //    .ValueGeneratedOnAdd()
            //    .UseIdentityColumn();

            // setup the keys for the favorite Subject table
            builder.Entity<FavoriteSubject>()
        .HasKey(fs => new { fs.UserId, fs.SubjectId });

            builder.Entity<FavoriteSubject>()
                .HasOne(fs => fs.User)
                .WithMany(u => u.FavoriteSubjects)
                .HasForeignKey(fs => fs.UserId)
                .IsRequired()  // Make UserId a required field
                .OnDelete(DeleteBehavior.Restrict);// Specify NO ACTION

            builder.Entity<SubjectTypeSubject>()
             .HasKey(subTypeSub => new {
                 subTypeSub.SubjectId,
                 subTypeSub.SubjectTypeId
             });

            builder.Entity<SubjectTypeSubject>()
                .HasOne(subTypeSub => subTypeSub.Subject)
                .WithMany(sub => sub.SubjectTypeSubjects)
                .HasForeignKey(subTypeSub => subTypeSub.SubjectId);

            builder.Entity<SubjectTypeSubject>()
                .HasOne(subTypeSub => subTypeSub.SubjectType)
                .WithMany(subType => subType.SubjectTypeSubjects)
                .HasForeignKey(subTypeSub => subTypeSub.SubjectTypeId);

            builder.Entity<FavoriteSubject>()
                .HasOne(fs => fs.Subject)
                .WithMany(s => s.FavoriteSubjects)
                .HasForeignKey(fs => fs.SubjectId)
                .IsRequired()  // Make SubjectId a required field
                .OnDelete(DeleteBehavior.Cascade); // Specify NO ACTION

            // Set the length of UserId to match the length in AspNetUsers table
            builder.Entity<FavoriteSubject>()
                .Property(fs => fs.UserId)
                .HasMaxLength(450); // Adjust the length as needed

            // setup the keys for the comment suvbject table 
            builder.Entity<CommentSubject>()
                .HasKey(cs => cs.Id);

            builder.Entity<CommentSubject>()
                .HasOne(cs => cs.User)
                .WithMany(u => u.CommentSubjects)
                .HasForeignKey(cs => cs.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CommentSubject>()
                .HasOne(cs => cs.Subject)
                .WithMany(s => s.CommentSubjects)
                .HasForeignKey(cs => cs.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // setup the comment Vote for user keys to denied the cycels 
            builder.Entity<UserCommentVote>()
                .HasKey(uc => uc.Id);

            builder.Entity<UserCommentVote>()
                .HasOne(cs => cs.User)
                .WithMany(u => u.userCommentVotes)
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserCommentVote>()
                .HasOne(us => us.CommentSubject)
                .WithMany(u => u.UserCommentVotes)
                .HasForeignKey(us => us.CommentSubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Setup The Comment's replies Keys and relations

            builder.Entity<CommentReplies>()
                .HasKey(cr => cr.Id);

            builder.Entity<CommentReplies>()
                .HasOne(cr => cr.User)
                .WithMany(cr => cr.CommentReplies)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CommentReplies>()
                .HasOne(cr => cr.CommentSubject)
                .WithMany(cr => cr.CommentReplies)
                .OnDelete(DeleteBehavior.Cascade);


            // setup the keys for the Rating subject table 
            builder.Entity<RatingSubject>()
                .HasKey(cs => cs.Id);

            builder.Entity<RatingSubject>()
                .HasOne(cs => cs.User)
                .WithMany(u => u.RatingSubjects)
                .HasForeignKey(cs => cs.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RatingSubject>()
                .HasOne(cs => cs.Subject)
                .WithMany(s => s.RatingSubjects)
                .HasForeignKey(cs => cs.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relation between the Subject and the Poll 

            builder.Entity<Poll>()
           .HasOne(p => p.Subject)
           .WithOne(s => s.Poll)
           .HasForeignKey<Poll>(p => p.SubjectId);

            builder.Entity<PollOption>()
                .HasOne(po => po.Poll)
                .WithMany(p => p.Options)
                .HasForeignKey(po => po.PollId);

            builder.Entity<UserVote>()
    .HasKey(uv => uv.Id);

            builder.Entity<UserVote>()
                .HasOne(uv => uv.User)
                .WithMany(u => u.UserVotes)
                .HasForeignKey(uv => uv.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserVote>()
                .HasOne(uv => uv.Poll)
                .WithMany(p => p.UserVotes)
                .HasForeignKey(uv => uv.PollId)
                .OnDelete(DeleteBehavior.Restrict); // Change to Restrict

            builder.Entity<UserVote>()
                .HasOne(uv => uv.PollOption)
                .WithMany(po => po.UserVotes)
                .HasForeignKey(uv => uv.PollOptionId)
                .OnDelete(DeleteBehavior.Cascade); // Change to Cascade


            // deparmnet with admin 
            builder.Entity<DepartmentAdmin>()
                .HasKey(da => new { da.DepartmentTypeId, da.UserId });

            builder.Entity<DepartmentAdmin>()
                .HasOne(da => da.DepartmentType)
                .WithMany(dt => dt.DepartmentAdmins)
                .HasForeignKey(da => da.DepartmentTypeId);

            builder.Entity<DepartmentAdmin>()
                .HasOne(da => da.User)
                .WithMany(u => u.DepartmentAdmins)
                .HasForeignKey(u => u.UserId);

            builder.Entity<UserColors>()
               .HasKey(uc => new { uc.UserId, uc.ColorsId });

            builder.Entity<Colors>().HasData(
                new Colors { Id = 1, HexColor = "#FF0000" },
                new Colors { Id = 2, HexColor = "#FFA500" },
                new Colors { Id = 3, HexColor = "#0000FF" },
                new Colors { Id = 4, HexColor = "#008000" }
                );

            builder.Entity<Category>().HasData(
              new Category { Id = 1, Name_En = "Category 1", Name_Ar = "الفئة 1" },
              new Category { Id = 2, Name_En = "Category 2", Name_Ar = "الفئة 2" }
          // Add more categories as needed
          );

            builder.Entity<DepartmentType>().HasData(
                new DepartmentType { Id = 1, Name_En = "DepartmentType 1", Name_Ar = "نوع القسم 1" },
                new DepartmentType { Id = 2, Name_En = "DepartmentType 2", Name_Ar = "نوع القسم 2" }
                // Add more department types as needed
            );

            builder.Entity<SubCategory>().HasData(
                new SubCategory { Id = 1, Name_En = "Subcategory 1", Name_Ar = "الفئة الفرعية 1", CategoryId = 1 },
                new SubCategory { Id = 2, Name_En = "Subcategory 2", Name_Ar = "الفئة الفرعية 2", CategoryId = 2 }
                // Add more subcategories as needed
            );

            builder.Entity<SubjectType>().HasData(
                new SubjectType { Id = 1, Name_Ar = "نقطة نقاش", Name_En = "suggest" },
                new SubjectType { Id = 2, Name_Ar = "افكار مبتكرة", Name_En = "Ideas" }
                // Add more subject types as needed
            );
            builder.Entity<GeneralSubjectsType>().HasData(
             new GeneralSubjectsType { Id = 1, Title_En = "Public", Title_Ar = "عام", Name_Ar = "الأفكار والمقترحات العامة", Name_En = "General ideas and suggestions" },
             new GeneralSubjectsType { Id = 2, Title_En = "Public", Title_Ar = "عام", Name_Ar = "تطوير الخدمات الالكترونية", Name_En = "Development of electronic services" },
             new GeneralSubjectsType { Id = 3, Title_En = "Public", Title_Ar = "عام", Name_Ar = "حلول وأفكار تصريف مياه الأمطار", Name_En = "Solutions and ideas for rainwater drainage" },
             new GeneralSubjectsType { Id = 4, Title_En = "Public", Title_Ar = "عام", Name_Ar = "تطوير السوق المركزي", Name_En = "Development of the central market" },
             new GeneralSubjectsType { Id = 5, Title_En = "Private", Title_Ar = "خاص", Name_Ar = "مقترح لفعاليات خريف ظفار ٢٠٢٤", Name_En = "Proposal for Dhofar Fall 2024 events" }
         // Add more subject types as needed
         );




            builder.Entity<OnBoardScreen>()
                .HasData(
                new OnBoardScreen { Id = 1, ImageUrl = ".uploads/b8f808a9-5cb4-43c9-a294-3d8abfefa36f_onboard_1 (1).png", Title = "شاركنا أفكارك", Description = "يتيح تطبيق ضع بصمتك التابع لبلدية ظفار وسيلة فعّالة لمشاركة الأفكار بين المواطنين والسلطات المحلية بسهولة. مما يسهم في خلق بيئة تشجع " },
                new OnBoardScreen { Id = 2, ImageUrl = "uploads/a3cfa779-1119-47f3-b801-5084b664a3e2_onboard_2.png", Title = "قدم شكوى أو بلاغ", Description = "قم بالإبلاغ عن المشكلات وحلها بكفاءة من خلال تقديم طلب في التطبيق واستمتع بتجربة سلسة في التعبير عن مخاوفك. ويمكنك تتبع طلبك بسهولة عن بعد." }
                );






            seedRole(builder, "Super Admin", "Create", "Update", "Delete", "Read");
            seedRole(builder, "Admin", "Create", "Update", "Delete", "Read");
            seedRole(builder, "User", "Create", "Update", "Delete", "Read");
            seedRole(builder, "Suspict", "Read");


        }
        int nextId = 1;
        private void seedRole(ModelBuilder modelBuilder, string roleName, params string[] permissions)
        {
            var role = new IdentityRole
            {
                Id = roleName.ToLower(),
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
                ConcurrencyStamp = Guid.Empty.ToString()
            };
            var roleClaim = permissions.Select(permissions =>
            new IdentityRoleClaim<string>
            {
                Id = nextId++,
                RoleId = role.Id,
                ClaimType = "permissions",
                ClaimValue = permissions
            }).ToArray();
            modelBuilder.Entity<IdentityRole>().HasData(role);

        }

    }
}
