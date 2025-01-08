// const moviesApi = "https://localhost:7049/api/Movie";
const moviesApi = "https://proj.ruppin.ac.il/bgroup1/test2/tar1/api/Movie";
// const castsApi = "https://localhost:7049/api/Cast";
const castsApi = "https://proj.ruppin.ac.il/bgroup1/test2/tar1/api/Cast";
// const usersApi = "https://localhost:7049/api/User";
const usersApi = "https://proj.ruppin.ac.il/bgroup1/test2/tar1/api/User";

$(document).ready(() => {

    localStorage.clear();
    // Toggle visibility of login and register sections
    $("#show-register-btn").click(() => {
        $("#login-section").hide();
        $("#register-section").fadeIn();
    });

    $("#show-login-btn").click(() => {
        $("#register-section").hide();
        $("#login-section").fadeIn();
    });

    // Display feedback messages
    function showFeedback(elementId, message, success = true) {
        const element = $("#" + elementId);
        element.css("color", success ? "green" : "red").text(message);
    }

    // Function to register a new user
    function registerUser() {
        const userName = $("#register-username").val().trim();
        const email = $("#register-email").val().trim();
        const password = $("#register-password").val();

        if (!userName || !email || !password) {
            showFeedback("register-feedback", "All fields are required.", false);
            return;
        }

        const userData = JSON.stringify({ id: 0, userName: userName, Email: email, Password: password });

        ajaxCall(
            "POST",
            `${usersApi}/Register`,
            userData,
            function () {
                showFeedback("register-feedback", "Registration successful! You can now log in.");
            },
            function (xhr) {
                const error = xhr.responseText || "Registration failed.";
                showFeedback("register-feedback", `Registration failed: ${error}`, false);
            }
        );
    }

    // Function to log in a user
    function loginUser() {
        const email = $("#login-email").val().trim();
        const password = $("#login-password").val();

        if (!email || !password) {
            showFeedback("login-feedback", "Both email and password are required.", false);
            return;
        }

        const loginData = JSON.stringify({ id: 0, username: "", Email: email, Password: password });

        ajaxCall(
            "POST",
            `${usersApi}/Login`,
            loginData,
            function (response) {
                if (response.userName && response.id) {
                    // Save login data to local storage
                    localStorage.setItem("isLoggedIn", true);
                    localStorage.setItem("userName", response.userName);
                    localStorage.setItem("userId", response.id);

                    // Add success message to LocalStorage
                    localStorage.setItem("loginSuccessMessage", "Login successful! Welcome, " + response.userName + ".");

                    window.location.href = "index.html";
                } else {
                    showFeedback("login-feedback", "Login succeeded but user details not found.", false);
                }
            },
            function (xhr) {
                const error = xhr.responseText || "Login failed: Please register first.";
                showFeedback("login-feedback", error, false);
            }
        );
    }

    // Event listeners for the buttons
    $("#register-btn").click(registerUser);
    $("#login-btn").click(loginUser);

    // Check login status on other pages
    const currentPageId = $("body").attr("id");
    const isLoggedIn = localStorage.getItem("isLoggedIn");

    if (currentPageId !== "login-page" && !isLoggedIn) {
        // Clear local storage and redirect to login page
        localStorage.clear();
        alert("You must log in to access this page.");
        window.location.href = "login.html";
    }
});

function ajaxCall(method, api, data, successCB, errorCB) {
    $.ajax({
        type: method,
        url: api,
        data: data,
        cache: false,
        contentType: "application/json",
        dataType: "json",
        success: successCB,
        error: errorCB
    });
}
// Function to fetch favorite movies
function fetchFavoriteMovies() {
    const userId = localStorage.getItem("userId");

    $.ajax({
        url: moviesApi,
        method: "GET",
        success: function (movies) {
            const container = $("#movies-container");
            container.empty(); // Clear previous content

            movies.forEach(movie => {
                const movieCard = `
                    <div class="movie-card">
                        <h3>${movie.title}</h3>
                        <p>${movie.genre}</p>
                        <p>Release Year: ${movie.releaseYear}</p>
                        <button onclick="removeFavorite(${movie.id})">Remove</button>
                    </div>
                `;
                container.append(movieCard);
            });
        },
        error: function () {
            console.error("Failed to fetch favorites.");
        }
    });
}

function removeFavorite(movieId) {
    const userId = localStorage.getItem("userId");

    $.ajax({
        url: moviesApi,
        method: "DELETE",
        success: function () {
            alert("Movie removed from favorites.");
            fetchFavoriteMovies(); // Refresh favorites
        },
        error: function () {
            console.error("Failed to remove movie.");
        }
    });
}

// Call the function when the page loads
if ($("body").attr("id") === "favorites-page") {
    fetchFavoriteMovies();
}


// Function to add a cast member to a movie
$("#add-cast-form").on("submit", function (event) {
    event.preventDefault();

    const movieId = $("#movie-id").val();
    const castId = $("#cast-id").val();

    $.ajax({
        url: castsApi,
        method: "POST",
        success: function () {
            alert("Cast added to movie successfully.");
        },
        error: function () {
            console.error("Failed to add cast.");
        }
    });
});


// function to show movie cast
$("#view-cast-form").on("submit", function (event) {
    event.preventDefault();

    const movieId = $("#cast-movie-id").val();

    $.ajax({
        url: castsApi,
        method: "GET",
        success: function (castList) {
            const $container = $("#cast-container");
            $container.empty(); // Clear the container before adding new cast cards

            castList.forEach((cast) => {
                const castCard = `
                    <div class="cast-card">
                        <h3>${cast.name}</h3>
                        <p>Role: ${cast.role}</p>
                        <p>Country: ${cast.country}</p>
                    </div>
                `;
                $container.append(castCard);
            });
        },
        error: function () {
            console.error("Failed to fetch cast.");
        }
    });
});


// Admin page to show all users
function fetchUsers() {
    $.ajax({
        url: usersApi,
        method: "GET",
        success: function (users) {
            const $container = $("#users-container");
            $container.empty(); // Clear the container

            users.forEach((user) => {
                const userCard = `
                    <div class="user-card">
                        <h3>${user.userName}</h3>
                        <button class="view-favorites-btn" data-user-id="${user.id}">View Favorites</button>
                    </div>
                `;
                $container.append(userCard);
            });

            // Attach click events to the buttons
            $(".view-favorites-btn").on("click", function () {
                const userId = $(this).data("user-id");
                viewUserFavorites(userId);
            });
        },
        error: function () {
            console.error("Failed to fetch users.");
        }
    });
}

function viewUserFavorites(userId) {
    $.ajax({
        url: usersApi,
        method: "GET",
        success: function (favorites) {
            const $container = $("#user-favorites-container");
            $container.empty(); // Clear the container

            favorites.forEach((movie) => {
                const movieCard = `
                    <div class="movie-card">
                        <h3>${movie.title}</h3>
                        <p>${movie.genre}</p>
                    </div>
                `;
                $container.append(movieCard);
            });
        },
        error: function () {
            console.error("Failed to fetch user favorites.");
        }
    });
}

// Call fetchUsers when admin page loads
if ($("body").attr("id") === "admin-page") {
    fetchUsers();
}

