using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using Redis.OM;
using Redis.OM.Modeling;
using Redis.OM.Searching;
using RedisApp.Models;
using RedisApp.Services;
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
    IRedisCacheService _redisCacheService;
    public HomeController(ILogger<HomeController> logger, IRedisCacheService redisCacheService)
    {
        _redisCacheService = redisCacheService;
    }


    public async Task<IActionResult> Index()
    {
        return View();
    }
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _redisCacheService.GetItems<Person>());
    }
    [HttpPost]
    public async Task<IActionResult> Kaydet(PersonModel model)
    {
        //await users.InsertAsync(person);
        if (model.person.Id == null)
        {
            model.person.Id = Guid.NewGuid().ToString();
            await _redisCacheService.Cache(model.person, "Person", new TimeSpan(0, 0, model.Ttl.Value));
            return Ok("Kayıt işlemi başarılı");
        }
        else
        {
            var updateUser = await _redisCacheService.GetItem<Person>(model.person.Id);
            if (updateUser != null)
            {
                updateUser.FirstName = model.person.FirstName;
                updateUser.LastName = model.person.LastName;
                TimeSpan? timespan = null;
                if (model.Ttl.HasValue)
                {
                    timespan = new TimeSpan(0, 0, model.Ttl.Value);
                }
                var result = await _redisCacheService.UpdateItem(updateUser, "Person", timespan);
                return Ok(result ? "Güncelleme işlemi başarılı" : "Güncelleme işlemi başarısız.");
            }
            else
            {
                return Ok("Güncelleme işlemi yapılamadı. Kullanıcı bulunamadı.");
            }
        }

    }
    [HttpPost]
    public async Task<JsonResult> RemoveItem(string id)
    {
        var isRemoved = await _redisCacheService.DeleteKey("Person:" + id);
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
