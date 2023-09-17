$('body').on('click', "#btn-download", function () {
    let div = document.getElementById('exchangeRateContainer');
    if (div.innerText == ""){ //only allow to insert the exchange rates once
        fetchDataFromNbp();
    }
});
function fetchDataFromNbp() {
    $.get("/NbpApi/GetApiData", function (data) {
        $('#exchangeRateContainer').append(data);
        $('#btn-download').addClass('disabled');
    });
}