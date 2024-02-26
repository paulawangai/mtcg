using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Text;

using mtcg.Services;

namespace mtcg
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add database context
            services.AddDbContext<mtcgDbContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("DefaultConnection");
                options.UseNpgsql(connectionString);
            }, ServiceLifetime.Scoped);

            // Add distributed caching services
            services.AddDistributedMemoryCache(); // You can replace this with any other distributed cache implementation

            // Add session services
            services.AddSession(options =>
            {
                // Set session timeout and other options if needed
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Configure JWT authentication
            services.AddSingleton<JwtAuthenticationService>(); // Add JwtAuthenticationService as singleton
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Jwt:Secret"]))
                    };
                });
            services.AddControllers();

            // Add controllers and configure JSON serialization
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null; 
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdmin", policy =>
                    policy.RequireAssertion(context =>
                    {
                        var username = context.User.Identity.Name;
                        return username == "admin"; 
                    }));
            });

            services.AddScoped<IPackageService, PackageService>();
            services.AddScoped<PackageService>();
            services.AddHttpContextAccessor();
            services.AddTransient<IBattleService, BattleService>(); //ensures that a new instance of BattleService is created for each request, thus avoiding concurrency issues with the DbContext.




            // Other services and configurations
        }



        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

            // Migrate existing users
            MigrateExistingUsers(serviceProvider);

            app.UseSession();




            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<mtcgDbContext>();
                dbContext.Database.Migrate();
                MigrateExistingPackages(dbContext);
                MigrateExistingCards(dbContext);
            }

           

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Enable authentication
            app.UseAuthentication();

            app.UseRouting();

            // Add authorization middleware
            app.UseAuthorization();

            // Add endpoint routing
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Other middleware configurations
        }

        private void MigrateExistingUsers(IServiceProvider serviceProvider)
        {
            // Resolve the database context from the service provider
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<mtcgDbContext>();

            // Retrieve all existing users from the database
            var existingUsers = context.Users.ToList();

            // Update the Coins property for each user
            foreach (var user in existingUsers)
            {
                user.Coins = 20.0;
            }

            // Save the changes to the database
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                // Handle any exceptions
                Console.WriteLine("Error occurred while saving changes: " + ex.Message);
            }
        }

        private void MigrateExistingPackages(mtcgDbContext dbContext)
        {
            // Retrieve all existing packages from the database
            var existingPackages = dbContext.Packages.ToList();

            // Update the Price property for each package to 5
            foreach (var package in existingPackages)
            {
                package.Price = 5.0;
            }

            // Save changes to the database
            dbContext.SaveChanges();
        }

        private void MigrateExistingCards(mtcgDbContext dbContext)
        {
            // Retrieve all existing cards from the database
            var existingCards = dbContext.Cards.ToList();

            foreach (var card in existingCards)
            {
                // Retrieve the package ID associated with the card
                var packageId = card.PackageId;

                if (packageId != null)
                {
                    // Find the package by its ID
                    var package = dbContext.Packages.FirstOrDefault(p => p.Id == packageId);

                    if (package != null)
                    {
                        // Assign the owner ID of the package to the card
                        card.OwnerId = package.OwnerId;
                    }
                }
            }

            // Save changes to the database
            dbContext.SaveChanges();
        }



    }
}
