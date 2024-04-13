$(document).ready(async function () {
    await GetData();
})
var data = [];
async function GetData() {
    await $.ajax({
        url: "/Home/GetAll",
        method: "GET",
        success: async function (e) {
            $("#redis-list").empty();
            data = e;
            for (let item of e)
                $("#redis-list").append(`<div class="row mt-2">
                <div class="col-8"><b>Ad:</b> ${item.firstName + " <b>Soyad:</b> " + item.lastName} </div>
                <div class="col-4 d-flex justify-content-end">
                    <button class='btn btn-warning btn_guncelle' data-id='${item.id}'>Güncelle</button>
                    <button class='btn btn-danger btn_sil' data-id='${item.id}'>Sil</button>
                </div>
                <hr/>
            </div>`)
        }
    })
}

$(document).on("click", ".btn_sil", async function () {
    await $.ajax({
        url: "/Home/RemoveItem",
        method: "POST",
        data: { id: $(this).data("id") },
        success: async function (e) {
            alert(e)
            GetData();
        }
    })
})
$("form").submit(async function (e) {
    e.preventDefault();
    var id = $("#id").val();
    var ad = $("#ad").val()
    var soyad = $("#soyad").val()
    var ttl = $("#ttl").val()
    await Kaydet(id, ad, soyad, ttl);
    await Temizle();
})
const Kaydet = async (id, ad, soyad, ttl) => {
    var model = {
        person: {
            Id: id,
            FirstName: ad,
            LastName: soyad,
        },
        Ttl: ttl
    }
    await $.ajax({
        url: "/Home/Kaydet",
        method: "post",
        data: { model: model },
        success: async function (e) {
            alert(e)
            await GetData();

        }
    })

}

const Temizle = async () => {
    $("form")[0].reset();
    $("#id").val(null)
}

$(document).on("click", ".btn_guncelle", async function () {
    var id = $(this).data("id");
    var selectedItem = data.find(x => x.id == id);
    $("#id").val(selectedItem.id);
    $("#ad").val(selectedItem.firstName);
    $("#soyad").val(selectedItem.lastName);

})

