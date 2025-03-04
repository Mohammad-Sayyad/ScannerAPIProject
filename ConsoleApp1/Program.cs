////using Microsoft.EntityFrameworkCore;
////using Microsoft.Extensions.DependencyInjection;
////using Microsoft.Extensions.Hosting;
////using System;
////using System.Collections.Generic;
////using System.IO;
////using System.Linq;
////using System.Text.RegularExpressions;
////using System.Threading.Tasks;

////namespace ApiScannerConsole
////{
////    // مدل‌های دیتابیس
////    public class MenuPageApi
////    {
////        public int Id { get; set; }
////        public string ApiUrl { get; set; }
////        public int MenuPageId { get; set; }
////    }

////    // DbContext
////    public class ApplicationDbContext : DbContext
////    {
////        public DbSet<MenuPageApi> MenuPageApis { get; set; }

////        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
////    }

////    // سرویس اسکنر API
////    public class JsApiScannerService
////    {
////        private readonly ApplicationDbContext _context;
////        private readonly Regex _apiPattern;

////        public JsApiScannerService(ApplicationDbContext context)
////        {
////            _context = context;
////            _apiPattern = new Regex(@"api/[^\""?']+", RegexOptions.IgnoreCase);
////        }

////        public async Task ScanAndSaveAllControllersAsync(string rootPath)
////        {
////            if (!Directory.Exists(rootPath))
////            {
////                Console.WriteLine($"مسیر مشخص‌شده '{rootPath}' وجود ندارد!");
////                return;
////            }

////            var jsFiles = Directory.GetFiles(rootPath, "*controller.js", SearchOption.AllDirectories);

////            foreach (var jsFile in jsFiles)
////            {
////                string fileContent = await File.ReadAllTextAsync(jsFile);
////                string controllerName = Path.GetFileNameWithoutExtension(jsFile);
////                string folderPath = Path.GetDirectoryName(jsFile);
////                string folderName = new DirectoryInfo(folderPath).Name;

////                // استخراج API ها
////                var apiUrls = ExtractApiEndpoints(fileContent);
////                foreach (var api in apiUrls)
////                {
////                    if (!_context.MenuPageApis.Any(a => a.ApiUrl == api))
////                    {
////                        _context.MenuPageApis.Add(new MenuPageApi
////                        {
////                            ApiUrl = api,
////                        });
////                        await _context.SaveChangesAsync();
////                    }
////                }
////            }
////        }

////        private List<string> ExtractApiEndpoints(string fileContent)
////        {
////            var apiUrls = new List<string>();
////            var matches = _apiPattern.Matches(fileContent);

////            foreach (Match match in matches)
////            {
////                apiUrls.Add(match.Value.Trim('"', '\''));
////            }

////            return apiUrls.Distinct().ToList();
////        }
////    }

////    // برنامه اصلی
////    class Program
////    {
////        static async Task Main(string[] args)
////        {
////            var host = Host.CreateDefaultBuilder(args)
////                .ConfigureServices((context, services) =>
////                {
////                    var connectionString = "Data Source=DESKTOP-912MTT6;Initial Catalog=ApiScanner;Integrated Security=True;TrustServerCertificate=true";
////                    services.AddDbContext<ApplicationDbContext>(options =>
////                        options.UseSqlServer(connectionString));

////                    services.AddScoped<JsApiScannerService>();
////                })
////                .Build();

////            var service = host.Services.GetRequiredService<JsApiScannerService>();
////            string rootPath = @"C:\Users\User\source\repos\sida2\sida-cross-platform\Pajoohesh.School.Web\wwwroot\Sida\App\views";
////            await service.ScanAndSaveAllControllersAsync(rootPath);
////        }
////    }
////}


////////////////////////////////////////////////////////////
/////// Actions and API Requests and Redirects of them 
































//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using System.Web;

//namespace ApiScannerConsole
//{
//    // مدل‌های دیتابیس
//    public class MenuPageData
//    {
//        public int Id { get; set; }
//        public string FolderName { get; set; }
//        public string ActionName { get; set; }
//        public List<string> ApiUrls { get; set; }
//        public List<string> RedirectUrls { get; set; }
//    }

//    public class ApplicationDbContext : DbContext
//    {
//        public DbSet<MenuPageData> MenuPageDatas { get; set; }

//        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
//    }


//    // سرویس اسکنر API
//    public class JsApiScannerService
//    {
//        private readonly ApplicationDbContext _context;

//        public JsApiScannerService(ApplicationDbContext context)
//        {
//            _context = context;
//        }



//        public async Task ScanAndSaveAllControllersAsync(string rootPath)
//        {
//            if (!Directory.Exists(rootPath))
//            {
//                Console.WriteLine($"مسیر مشخص‌شده '{rootPath}' وجود ندارد!");
//                return;
//            }

//            var jsFiles = Directory.GetFiles(rootPath, "*controller.js", SearchOption.AllDirectories);

//            foreach (var jsFile in jsFiles)
//            {
//                string fileContent = await File.ReadAllTextAsync(jsFile);
//                string controllerName = Path.GetFileNameWithoutExtension(jsFile);
//                string folderPath = Path.GetDirectoryName(jsFile);
//                string folderName = new DirectoryInfo(folderPath).Name;

//                var existingPage = await _context.MenuPageDatas
//                    .FirstOrDefaultAsync(p => p.ActionName == controllerName && p.FolderName == folderName);

//                if (existingPage == null)
//                {
//                    existingPage = new MenuPageData
//                    {
//                        FolderName = folderName,
//                        ActionName = controllerName,
//                        ApiUrls = new List<string>(),
//                        RedirectUrls = new List<string>()
//                    };
//                    _context.MenuPageDatas.Add(existingPage);
//                    await _context.SaveChangesAsync();
//                }

//                var apiUrls = ExtractApiEndpoints(fileContent);
//                foreach (var api in apiUrls)
//                {
//                    if (!existingPage.ApiUrls.Contains(api))
//                    {
//                        existingPage.ApiUrls.Add(api);
//                        await _context.SaveChangesAsync();
//                    }
//                }

//                var redirects = ExtractRedirects(fileContent);
//                foreach (var redirect in redirects)
//                {
//                    if (!existingPage.RedirectUrls.Contains(redirect))
//                    {
//                        existingPage.RedirectUrls.Add(redirect);
//                        await _context.SaveChangesAsync();
//                    }
//                }

//                Console.WriteLine($"فولدر: {folderName}");
//                Console.WriteLine($"اکشن: {controllerName}");
//                Console.WriteLine("APIهای موجود:");
//                foreach (var api in existingPage.ApiUrls)
//                {
//                    Console.WriteLine($"  - {api}");
//                }

//                Console.WriteLine("ری دایرکت ها:");
//                foreach (var redirect in existingPage.RedirectUrls)
//                {
//                    Console.WriteLine($"  - {redirect}");
//                }
//            }
//        }

//        public async Task AddManualEntryAsync()
//        {
//            Console.WriteLine("Enter folder name:");
//            string folderName = Console.ReadLine();

//            Console.WriteLine("Enter action name:");
//            string actionName = Console.ReadLine();

//            var existingPage = await _context.MenuPageDatas
//                .FirstOrDefaultAsync(p => p.ActionName == actionName && p.FolderName == folderName);

//            if (existingPage == null)
//            {
//                existingPage = new MenuPageData
//                {
//                    FolderName = folderName,
//                    ActionName = actionName,
//                    ApiUrls = new List<string>(),
//                    RedirectUrls = new List<string>()
//                };
//                _context.MenuPageDatas.Add(existingPage);
//                await _context.SaveChangesAsync();
//            }

//            Console.WriteLine("Enter the API (press Enter to finish):");
//            while (true)
//            {
//                string apiUrl = Console.ReadLine();
//                if (string.IsNullOrEmpty(apiUrl)) break;

//                if (!existingPage.ApiUrls.Contains(apiUrl))
//                {
//                    existingPage.ApiUrls.Add(apiUrl);
//                    await _context.SaveChangesAsync();
//                }
//            }

//            Console.WriteLine("Enter the redirect (press Enter to finish):");
//            while (true)
//            {
//                string redirectUrl = Console.ReadLine();
//                if (string.IsNullOrEmpty(redirectUrl)) break;

//                if (!existingPage.RedirectUrls.Contains(redirectUrl))
//                {
//                    existingPage.RedirectUrls.Add(redirectUrl);
//                    await _context.SaveChangesAsync();
//                }
//            }

//            Console.WriteLine("Information added successfully!");
//        }

//        private List<string> ExtractApiEndpoints(string fileContent)
//        {
//            var apiUrls = new List<string>();

//            var regexPatterns = new List<string>
//        {
//            @"['""](\/api\/[\w\/-]+)['""]",
//            @"['""](\/api\/[^'""?]+\?[^'""]+)['""]",
//            @"`(\/api\/[^`]+)`",
//            @"\$scope\.\w+API\s*=\s*['""]([^'""]+)['""]",
//            @"['""](api\/[^'""?]+|\/api\/[^'""?]+)['""]",
//            @"url:\s*['""](api\/[^'""?]+|\/api\/[^'""?]+)['""]",
//        };
//            foreach (var pattern in regexPatterns)
//            {
//                var matches = Regex.Matches(fileContent, pattern, RegexOptions.IgnoreCase);
//                foreach (Match match in matches)
//                {
//                    var apiUrl = match.Groups[1].Value.Trim();
//                    if (!string.IsNullOrEmpty(apiUrl))
//                    {
//                        apiUrls.Add(apiUrl);
//                    }
//                }
//            }

//            return apiUrls.Distinct().ToList();
//        }
//        private List<string> ExtractRedirects(string fileContent)
//        {
//            var redirects = new List<string>();

//            var regexPattern = @"\$state\.go\(\s*['""`]([^'"",`\s\)]+)['""`]";

//            var matches = Regex.Matches(fileContent, regexPattern, RegexOptions.IgnoreCase);
//            foreach (Match match in matches)
//            {
//                var redirectUrl = match.Groups[1].Value.Trim();
//                if (!string.IsNullOrEmpty(redirectUrl))
//                {
//                    redirects.Add(redirectUrl);
//                }
//            }

//            return redirects.Distinct().ToList();
//        }



//    }


//    // برنامه اصلی
//    class Program
//    {
//        static async Task Main(string[] args)
//        {
//            // تنظیمات مربوط به سرویس و دیتابیس
//            var host = Host.CreateDefaultBuilder(args)
//                .ConfigureServices((context, services) =>
//                {
//                    // تنظیمات دیتابیس و سرویس
//                    var connectionString = "Data Source=DESKTOP-912MTT6;Initial Catalog=ApiScanner;Integrated Security=True;TrustServerCertificate=true";
//                    services.AddDbContext<ApplicationDbContext>(options =>
//                        options.UseSqlServer(connectionString));

//                    services.AddScoped<JsApiScannerService>();
//                })
//                .Build();

//            // به دست آوردن سرویس و شروع اسکن
//            var service = host.Services.GetRequiredService<JsApiScannerService>();
//            string rootPath = @"F:\new\sida-cross-platform-main\Pajoohesh.School.Web\wwwroot\Sida\App\views";
//            await service.ScanAndSaveAllControllersAsync(rootPath);
//        }
//    }
//}

