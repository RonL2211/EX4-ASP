const moviesApi = "https://localhost:7049/api/Movie";


$(document).ready(() => {
    const $movieContainer = $("#movie-container");
    const $movieForm = $("#add-movie-form");

    $movieForm.submit(addingMovie);
});


    addingMovie =  (e) => {
    e.preventDefault();

    const movie = {
        id: 0,
        title: $("#movie-title").val(),
        rating: parseFloat($("#movie-rating").val()),
        income: parseInt($("#movie-income").val(), 10),
        releaseYear: parseInt($("#movie-releaseYear").val(), 10),
        duration: parseInt($("#movie-duration").val(), 10),
        language: $("#movie-language").val(),
        description: $("#movie-description").val(),
        genre: $("#movie-genre").val(),
        photoUrl: $("#movie-photoUrl").val()
    };

    ajaxCall(
        "POST",
        moviesApi,
        JSON.stringify(movie),
        () => {
            alert("Movie added successfully!");
            // renderMovies(); // Refresh the list
        },
        (error) => {
            console.error("Failed to add movie:", error);
            alert("Failed to add movie. Please try again.");
        }
    );

};




function ajaxCall(method, api, data, successCB, errorCB) {
    $.ajax({
        type: method,
        url: api,
        data: data,
        cache: false,
        contentType: "application/json",
        datatype: "json",
        success: successCB,
        error: errorCB
    });
}
