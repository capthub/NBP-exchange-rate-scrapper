$('body').on('click', "#btn-download", function ()
{
    let div = document.getElementById('exchangeRateContainer');
    if (div.innerText == "") { //only allow to insert the exchange rates once 
        fetchDataFromNbp();
    }
});

$('body').on('click', "#btn-save", function ()
{
    let div = document.getElementById('exchangeRateContainer');
    if (div.innerText != "") { //check if data was loaded
        postData();
    }
});

function fetchDataFromNbp() {
    // Make an AJAX request to the NbpApiController / GetApiData action
    $.ajax({
        url: "/NbpApi/GetApiData",
        type: "GET",
        success: function (data) {
            $('#exchangeRateContainer').append(data);
            $('#btn-download').html('Wyświetl aktualne kursy walut NBP <i class="tick fa-solid fa-check"></i>');
            $('#btn-download').prop("disabled", true);
            $('#btn-save').prop('disabled', false);
            $('#btn-download').addClass('success');
        },
        error: function (xhr, textStatus, err) { console.error('Error getting data', err); }
    });
}

function postData() {
    // Make an AJAX request to the DataController / PostData action   
    $('.mySpinner').removeClass('collapse');
    $.ajax({
        url: "/Data/PostData",
        type: "POST",        
        success: function (result) {             
            $('#btn-save').html(' Zapisane <i class="tick fa-solid fa-check"></i>');
            $('#btn-save').addClass('success');
            $('#btn-save').prop('disabled', true);
            $('.mySpinner').addClass('collapse');
        },
        error: function (error) { 
            console.error('Error triggering Data/PostData:', error);
        }
    });
}