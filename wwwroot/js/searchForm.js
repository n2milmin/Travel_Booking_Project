$(document).ready(function () {
    $('#searchForm').submit(function (event) {
        event.preventDefault(); // Prevent default form submission

        var serviceType = $('#searchType').val();
        var searchString = $('#searchString').val();

        $.ajax({
            url: '/Home/GeneralSearch',
            method: 'GET',
            data: { searchType: searchType, searchString: searchString },
            beforeSend: function () {
                $('#searchResults').html('<div class="loader">Loading...</div>');
            },
            success: function (response) {
                $('#searchResults').html(response);
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });
});
