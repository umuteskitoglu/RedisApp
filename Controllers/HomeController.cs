using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using Redis.OM;
using Redis.OM.Modeling;
using Redis.OM.Searching;
using RedisApp.Models;
using StackExchange.Redis;

namespace RedisApp.Controllers;

public class PersonModel
{
    public Person person { get; set; }
    public int? Ttl { get; set; }
}

[Document(StorageType = StorageType.Json, Prefixes = new[] { "Person" })]
public class Person
{
    [RedisIdField][Indexed] public string? Id { get; set; }

    [Searchable] public string? FirstName { get; set; }

    [Indexed] public string? LastName { get; set; }

    [Indexed] public int Age { get; set; } = 0;
    //[Indexed] public DateTime BirthDate { get; set; }


}
public class HomeController : Controller
{
    private readonly IRedisCollection<Person> users;

    private readonly ILogger<HomeController> _logger;
    private readonly IConnectionMultiplexer _redisDb;
    private readonly RedisConnectionProvider _provider;
    public HomeController(ILogger<HomeController> logger, IConnectionMultiplexer redisDb, RedisConnectionProvider provider)
    {
        _logger = logger;
        _redisDb = redisDb;
        _provider = provider;
        users = _provider.RedisCollection<Person>();
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }
    public async Task<IActionResult> GetAll()
    {
        return Ok(users.ToList());
    }
    [HttpPost]
    public async Task<IActionResult> Kaydet(PersonModel model)
    {
        var db = _redisDb.GetDatabase();
        //await users.InsertAsync(person);
        var json = db.JSON();
        if (model.person.Id == null)
        {
            model.person.Id = Guid.NewGuid().ToString();
            var key = "Person:" + model.person.Id;
            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model.person);
            await json.SetAsync(key, "$", jsonString);
            if (model.Ttl.HasValue)
            {
                db.KeyExpire(key, new TimeSpan(0, 0, model.Ttl.Value));
            }
            return Ok("Kayıt işlemi başarılı");
        }
        else
        {
            var updateUser = await users.FindByIdAsync(model.person.Id);
            if (updateUser != null)
            {
                updateUser.FirstName = model.person.FirstName;
                updateUser.LastName = model.person.LastName;
                var key = "Person:" + updateUser.Id;

                await json.SetAsync(key, "$", Newtonsoft.Json.JsonConvert.SerializeObject(updateUser));
                if (model.Ttl.HasValue)
                {
                    db.KeyExpire(key, new TimeSpan(0, 0, model.Ttl.Value));
                }
                return Ok("Güncelleme işlemi başarılı");
            }
            else
            {
                return Ok("Güncelleme işlemi yapılamadı. Kullanıcı yok.");
            }
        }

    }
    [HttpPost]
    public async Task<JsonResult> RemoveItem(string id)
    {
        var db = _redisDb.GetDatabase();
        var isRemoved = await db.KeyDeleteAsync("Person:" + id);

        return Json(isRemoved ? "Silme işlemi başarılı" : "Silme işlemi başarısız.");
    }
    public async Task<IActionResult> Privacy()
    {

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
