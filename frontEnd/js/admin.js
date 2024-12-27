
const usersApi = "https://localhost:7049/api/User";
const moviesApi = "https://localhost:7049/api/Movie";


$(document).ready(() => {
    // ✅ בדיקת התחברות כמנהל
    const isLoggedIn = localStorage.getItem("isLoggedIn");
    const userRole = localStorage.getItem("userName");

    if (isLoggedIn !== "true" || userRole !== "Admin") {
        alert("Access Denied: Admins Only!");
        localStorage.clear();
        window.location.href = "login.html";
    } else {
        $("#welcome").text(`Welcome, Admin`);
    }

    // ✅ ניווט חזרה לעמוד הבית
    $('#homePage').click(() => {
        window.location.href = "index.html";
    });

    // ✅ שליפת כל המשתמשים
    const fetchUsers = () => {
        ajaxCall(
            "GET",
            usersApi,
            null,
            (users) => {
                renderUsers(users);
            },
            (error) => {
                console.error("❌ Failed to fetch users:", error);
                $("#users-container").html("<p>Failed to load users. Please try again later.</p>");
            }
        );
    };

    const renderUsers = (users) => {
        const $usersContainer = $("#users-container");
        $usersContainer.empty();

        if (!users || users.length === 0) {
            $usersContainer.append("<p>No users found.</p>");
            return;
        }

        users.forEach(user => {
            $usersContainer.append(`
                <div class="user-card" data-id="${user.id}">
                    <h3>${user.userName}</h3>
                    <p><strong>Email:</strong> ${user.email}</p>
                    <button class="view-wishlist-btn">📋 View Wishlist</button>
                    <div class="wishlist-section" style="display:none;"></div>
                </div>
            `);
        });
    };

    // ✅ שליפת רשימת חלומות של משתמש
    const fetchUserWishlist = (userId, $wishlistSection) => {
        ajaxCall(
            "GET",
            moviesApi + `/wishlist/${userId}`,
            null,
            (wishlist) => {
                renderWishlist(wishlist, $wishlistSection);
            },
            (error) => {
                console.error("❌ Failed to fetch wishlist:", error);
                $wishlistSection.html("<p>this user have no movies in the wishlist.</p>");
            }
        );
    };

    const renderWishlist = (wishlist, $wishlistSection) => {
        $wishlistSection.empty();

        if (!wishlist || wishlist.length === 0) {
            $wishlistSection.append("<p>This user has no items in their wishlist.</p>");
            return;
        }

        wishlist.forEach(movie => {
            $wishlistSection.append(`
                <div class="movie-card">
                    <img src="${movie.photoUrl || '../assets/default-movie.jpg'}" alt="${movie.title}" class="movie-img">
                    <h4>${movie.title}</h4>
                    <p><strong>Rating:</strong> ${movie.rating || 'N/A'}</p>
                    <p><strong>Genre:</strong> ${movie.genre || 'N/A'}</p>
                </div>
            `);
        });
    };

    // ✅ הצגת רשימת חלומות של משתמש בלחיצה
    $("#users-container").on("click", ".view-wishlist-btn", function () {
        const userId = $(this).closest(".user-card").data("id");
        const $wishlistSection = $(this).closest(".user-card").find(".wishlist-section");

        if ($wishlistSection.is(":visible")) {
            $wishlistSection.slideUp();
        } else {
            $wishlistSection.slideDown();
            fetchUserWishlist(userId, $wishlistSection);
        }
    });

    // ✅ אתחול של שליפת משתמשים
    fetchUsers();
});

// ✅ פונקציית AJAX גנרית
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
