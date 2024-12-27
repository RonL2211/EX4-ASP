// wishListAPI = "https://localhost:7049/api/Movie/wishlist/"  ; 
wishListAPI = "https://proj.ruppin.ac.il/bgroup1/test2/tar1/api/Movie/wishlist/"  ; 


$(document).ready(() => {


    const userId = localStorage.getItem("userId");

    if (!userId) {
        alert("You must log in to access this page.");
        window.location.href = "login.html";
        return;
    }

    let wishlistIDs = []; // Store wishlist movie IDs
    let wishlistMovies = []; // Store all fetched wishlist movies

    // Fetch user's wishlist and store IDs
    const fetchWishlist = () => {
        let userId = localStorage.getItem("userId");
        ajaxCall(
            "GET",
            wishListAPI + userId,
            null,
            (movies) => {
                wishlistIDs = movies.map(movie => movie.id); // Extract movie IDs
                localStorage.setItem("wishlistIDs", JSON.stringify(wishlistIDs));
                wishlistMovies = movies; // Store all movies in memory

                renderWishList(movies, "#wishlist-container");
                
            },
            (error) => {
                console.error("❌ Error fetching wishlist:", error);
                $("#wishlist-container").html("<p>Failed to load your wishlist. Please try again later.</p>");
            }
        );
    };

    // Render Wishlist
    const renderWishList = (movies, containerSelector) => {
        const $container = $(containerSelector);
        $container.empty();

        if (!movies || movies.length === 0) {
            $container.append("<p>No movies found.</p>");
            return;
        }

        movies.forEach(movie => {
            // Check if the movie is in the wishlist
           // const isInWishlist = wishlistIDs.includes(movie.id); // בדיקה אם הסרט נמצא ברשימה

            $container.append(`
                <div class="movie">
                    <img src="${movie.photoUrl || '../assets/default-movie.jpg'}" alt="${movie.title}" class="movie-img"
                        onerror="this.onerror=null;this.src='../assets/default-movie.jpg';">
                    <h3>${movie.title || 'No Title'}</h3>
                    <p><strong>Rating:</strong> ${movie.rating || 'N/A'}</p>
                    <p><strong>Release Year:</strong> ${movie.releaseYear || 'N/A'}</p>
                    <p><strong>Genre:</strong> ${movie.genre || 'N/A'}</p>
                    <button 
                        class="remove-wishlist-btn"
                        data-id="${movie.id}">
                        Remove from Wishlist
                    </button>
                </div>
            `);
        });
    };

    // Remove movie from wishlist
    $("#wishlist-container").on("click", ".remove-wishlist-btn", function () {
        const movieId = $(this).data("id");

        ajaxCall(
            "DELETE",
            wishListAPI + userId + '/' + movieId,
            null,
            () => {
                alert("❌ Movie removed from your wishlist!");
                fetchWishlist();
            },
            (error) => {
                console.error("❌ Error removing movie from wishlist:", error);
            }
        );
    });

    // Filter by Minimum Rating
    $("#filter-rating-btn").on("click", () => {
        const minRating = parseFloat($("#rating-filter").val());
        if (isNaN(minRating) || minRating < 0 || minRating > 10) {
            alert("❌ Please enter a valid rating between 0 and 10.");
            return;
        }

        const filteredMovies = wishlistMovies.filter(movie => movie.rating >= minRating);
        renderWishList(filteredMovies, "#wishlist-container");
    });

    // Filter by Maximum Duration
    $("#filter-duration-btn").on("click", () => {
        const maxDuration = parseFloat($("#duration-filter").val());
        if (isNaN(maxDuration) || maxDuration <= 0) {
            alert("❌ Please enter a valid duration greater than 0.");
            return;
        }

        const filteredMovies = wishlistMovies.filter(movie => movie.duration <= maxDuration);
        renderWishList(filteredMovies, "#wishlist-container");
    });
    fetchWishlist();
});
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