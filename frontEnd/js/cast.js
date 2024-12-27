
const castsApi = "https://localhost:7049/api/Cast";
const moviesApi = "https://localhost:7049/api/Movie";


$(document).ready(() => {
    const $castContainer = $("#cast-container");
    const $castForm = $("#add-cast-form");

    // ✅ Render Cast Members
    const renderCast = (castMembers) => {
        $castContainer.empty(); // Clear previous members

        if (!castMembers || castMembers.length === 0) {
            $castContainer.append("<p>No cast members found.</p>");
            return;
        }

        castMembers.forEach(member => {
            $castContainer.append(`
                <div class="cast-member" data-id="${member.id}">
                    <img src="${member.photoUrl}" alt="${member.name}" class="cast-photo">
                    <h3>${member.name}</h3>
                    <p><strong>Role:</strong> ${member.role}</p>
                    <p><strong>Date of Birth:</strong> ${member.date}</p>
                    <p><strong>Country:</strong> ${member.country}</p>
                    <button class="assign-movie-btn">Assign to Movie</button>
                    <div class="assign-movie-section" style="display:none; margin-top:10px;">
                        <select class="movie-dropdown"></select>
                        <button class="confirm-assign-btn">Confirm</button>
                    </div>
                </div>
            `);
        });
    };

    // ✅ Fetch Movies for Dropdown
    const fetchMoviesForDropdown = ($dropdown) => {
        $.ajax({
            url: moviesApi,
            method: "GET",
            dataType: "json",
            success: (movies) => {
                $dropdown.empty();
                $dropdown.append(`<option value="" disabled selected>Select a Movie</option>`);

                movies.forEach(movie => {
                    $dropdown.append(`<option value="${movie.movie.id}">${movie.movie.title}</option>`);
                });
            },
            error: (error) => {
                console.error("Failed to fetch movies:", error);
                alert("Failed to load movies for assignment.");
            }
        });
    };

    // ✅ Show Movie Dropdown when "Assign to Movie" is clicked
    $castContainer.on("click", ".assign-movie-btn", function () {
        const $castCard = $(this).closest(".cast-member");
        const $assignSection = $castCard.find(".assign-movie-section");
        const $dropdown = $assignSection.find(".movie-dropdown");

        if ($assignSection.is(":visible")) {
            $assignSection.hide();
        } else {
            $assignSection.show();
            fetchMoviesForDropdown($dropdown);
        }
    });

    // ✅ Handle Movie Assignment
    $castContainer.on("click", ".confirm-assign-btn", function () {
        const $castCard = $(this).closest(".cast-member");
        const castId = $castCard.data("id");
        const movieId = $castCard.find(".movie-dropdown").val();

        if (!movieId) {
            alert("Please select a movie to assign the cast member.");
            return;
        }
        $.ajax({
            url: castsApi + `/link/${movieId}/${castId}`,
            method: "POST",
            success: () => {
                alert("✅ Cast member assigned to movie successfully!");
                $castCard.find(".assign-movie-section").hide();
            },
            error: (error) => {
                console.error("❌ Failed to assign cast member:", error);
                alert("The cast is already in the list . Please try again.");
            }
        });
    });

    // ✅ Handle Cast Form Submission
    $castForm.on("submit", (e) => {
        e.preventDefault();

        const castMember = {
            id: $("#cast-id").val(),
            name: $("#cast-name").val(),
            role: $("#cast-role").val(),
            date: $("#cast-date").val(),
            country: $("#cast-country").val(),
            photoUrl: $("#cast-photoUrl").val()
        };

        $.ajax({
            url: castsApi,
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(castMember),
            success: () => {
                alert("✅ Cast member added successfully!");
                $castForm.trigger("reset");
                fetchAndRenderCast(); // Refresh the list
            },
            error: (error) => {
                console.error("❌ Failed to add cast member:", error);
                alert("Failed to add cast member. Please try again.");
            }
        });
    });

    // ✅ Fetch and Render Cast Members
    const fetchAndRenderCast = () => {
        $.ajax({
            url: castsApi,
            method: "GET",
            dataType: "json",
            success: (castMembers) => {
                renderCast(castMembers);
            },
            error: (error) => {
                console.error("❌ Failed to fetch cast members:", error);
                $castContainer.html("<p>Failed to load cast members. Please try again later.</p>");
            }
        });
    };

    fetchAndRenderCast();
});
