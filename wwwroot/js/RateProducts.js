/// works
document.addEventListener("DOMContentLoaded", function () {
    let stars = document.querySelectorAll('.star-button');
    let productId = 123; // dummy

    let oldRating = 0;
    let newRating = 0;
    let pageLoadedFirst = 1;

    function updateStars(savedRating) {
        console.log(newRating);

        console.log(oldRating);
        if (oldRating == newRating) {
            for (let i = 0; i < stars.length; i++) {
                if (i < savedRating) {
                    stars[i].classList.toggle('fas'); // Remove solid
                    stars[i].classList.toggle('far'); // Add empty
                }
            }
        }
        else {

            for (let i = 0; i < stars.length; i++) {
                if (i < savedRating) {
                    stars[i].classList.add('fas');
                    stars[i].classList.remove('far');
                } else {
                    stars[i].classList.remove('fas');
                    stars[i].classList.add('far');
                }
            }
        }
    }

    function fetchSavedRating(productId) {
        fetch('/Product/GetProductRating?Id=' + productId)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                let savedRating = parseInt(data.rating);
                //    console.log(savedRating)
                oldRating = savedRating;
                updateStars(savedRating);
            })
            .catch(error => {
                console.error('There was a problem with the fetch operation:', error);
            });
    }

    fetchSavedRating(productId);
    stars.forEach(function (star, index) {
        star.addEventListener('click', function (event) {
            event.preventDefault(); // Prevent reload page

            if (pageLoadedFirst) {
                pageLoadedFirst = 0;
                newRating = index + 1;
            }
            else {
                oldRating = newRating;
                newRating = index + 1;
            }

            let rating = index + 1;

            document.getElementById('NumOfStars').value = rating;
            console.log("Rating updated: " + rating);

            updateStars(rating);


            let formData = new FormData(document.getElementById('rateProductForm'));
            fetch('/Product/RateProduct', {
                method: 'POST',
                body: formData
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    return response.text();
                })
                .then(data => {
                    document.getElementById('rating-section').innerHTML = data;
                    console.log(data);
                })
                .catch(error => {
                    console.error('There was a problem with the fetch operation:', error);
                });
        });
    });
});
