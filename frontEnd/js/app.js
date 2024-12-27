
// const wishListApi = "https://localhost:7049/api/Movie/wishlist/"  + localStorage.getItem("userId"); 
const wishListApi = "https://proj.ruppin.ac.il/bgroup1/test2/tar1/api/Movie/wishlist/"  + localStorage.getItem("userId"); 
// const moviesApi = "https://localhost:7049/api/Movie";
const moviesApi = "https://proj.ruppin.ac.il/bgroup1/test2/tar1/api/Movie";


$(document).ready(() => {

    const userId = localStorage.getItem("userId");

        ajaxCall(
            "GET",
            wishListApi,
            null,
            (wishlistMovies) => {
                wishlistIDs = wishlistMovies.map(movie => movie.id); // Extract movie IDs
                localStorage.setItem("wishlistIDs", JSON.stringify(wishlistIDs));                
            },
            (error) => {
                console.error("❌ Error fetching wishlist:", error);
                $("#wishlist-container").html("<p>Failed to load your wishlist. Please try again later.</p>");
            }
        );

    if (localStorage.getItem("isLoggedIn") !== "true") {
        window.location.href = "login.html";
    } else {
        $("#welcome").html("Welcome " + localStorage.getItem("userName"));

        // Display success message if exists
        const loginSuccessMessage = localStorage.getItem("loginSuccessMessage");
        if (loginSuccessMessage) {
            alert(loginSuccessMessage);
            localStorage.removeItem("loginSuccessMessage");
        }
    }

    // ✅ Navbar Menu
    const menuItems = [
        { name: "Home", link: "index.html" },
        { name: "Wish List", link: "wishlist.html" },
        { name: "Cast Form", link: "cast.html" },
        { name: "Add Movie", link: "addmovie.html" }
    ];
    if(localStorage.getItem("userName") === "Admin") menuItems.push({ name: "usersWishlists", link: "admin.html" });

    menuItems.forEach(item => {
        $("#menu").append(`<li><a href="${item.link}">${item.name}</a></li>`);
    });

    // ✅ Logout Button
    $('#logoutBtn').click(() => {
        localStorage.clear();
        window.location.href = "login.html";
    });


    // ✅ Render Movies
    const renderMovies = (movies) => {

        wishlistIDs = JSON.parse(localStorage.getItem("wishlistIDs")) || [];
        const $moviesContainer = $("#movies-container");
        $moviesContainer.empty();

        if (!movies || movies.length === 0) {
            $moviesContainer.append("<p>No movies found.</p>");
            return;
        }

        movies.forEach(movieObj => {
            const movie = movieObj.movie;
            const casts = movieObj.casts; // Extract casts
            const isInWishlist = wishlistIDs.includes(movie.id);

            $moviesContainer.append(`
                <div class="movie-card">
                    <img src="${movie.photoUrl || '../assets/default-movie.jpg'}" alt="${movie.title}" class="movie-img"
                        onerror="this.onerror=null;this.src='../assets/default-movie.jpg';">
                    <h3>${movie.title || 'No Title'}</h3>
                    <p><strong>Rating:</strong> ${movie.rating || 'N/A'}</p>
                    <p><strong>Release Year:</strong> ${movie.releaseYear || 'N/A'}</p>
                    <p><strong>Duration:</strong> ${movie.duration || 'N/A'} minutes</p>
                    <p><strong>Language:</strong> ${movie.language || 'N/A'}</p>
                    <p><strong>Genre:</strong> ${movie.genre || 'N/A'}</p>
                    <button 
                        class="wishlist-btn ${isInWishlist ? 'disabled-btn' : ''}" 
                        data-id="${movie.id}" 
                        ${isInWishlist ? 'disabled' : ''}>
                        ${isInWishlist ? 'In Wishlist' : 'Add to Wishlist'}
                    </button>
                    <button class="show-cast-btn" data-id="${movie.id}">Show Cast</button>
                    <div class="cast-container" id="cast-${movie.id}" style="display:none;"></div>
                </div>
            `);
        });
    };

    // ✅ Add to Wishlist
    $("#movies-container").on("click", ".wishlist-btn", function () {
        const movieId = $(this).data("id");
        const $button = $(this);

        ajaxCall(
            "POST",
            wishListApi +'/' + movieId,
            null,
            () => {
                alert("✅ Movie added to wishlist!");
                wishlistIDs.push(movieId);
                localStorage.setItem("wishlistIDs", JSON.stringify(wishlistIDs));
                $button.prop("disabled", true).addClass("disabled-btn").text("In Wishlist");
            },
            (error) => {
                console.error("❌ Error adding to wishlist:", error);
            }
        );
    });

    // ✅ Show Cast Details
    $("#movies-container").on("click", ".show-cast-btn", function () {
        const movieId = $(this).data("id");
        const $castContainer = $(`#cast-${movieId}`);

        if ($castContainer.is(":visible")) {
            $castContainer.hide();
            $(this).text("Show Cast");
        } else {
            $castContainer.show();
            $(this).text("Hide Cast");

            // Check if casts are already loaded
            if ($castContainer.is(":empty")) {
                const movie = movies.find(m => m.movie.id === movieId); // Find the specific movie
                if (movie && movie.casts.length > 0) {
                    movie.casts.forEach(cast => {
                        $castContainer.append(`
                            <p><strong>${cast.name}</strong> as ${cast.role}</p>
                        `);
                    });
                } else {
                    $castContainer.append("<p>No cast information available.</p>");
                }
            }
        }
    });

    // ✅ Fetch and Render Movies
    let movies = []; // To keep the movies data in scope
    const fetchAndRenderMovies = () => {
        $.ajax({
            url: moviesApi,
            method: 'GET',
            dataType: 'json',
            success: (fetchedMovies) => {
                movies = fetchedMovies; // Store in global variable
                console.log("✅ Movies Data from API:", movies);
                renderMovies(movies);
            },
            error: (xhr, status, error) => {
                console.error("❌ Error fetching movies:", error);
                $("#movies-container").append("<p>Failed to load movies. Please try again later.</p>");
            }
        });
    };

    fetchAndRenderMovies();
});



// ✅ Generic AJAX Function
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
