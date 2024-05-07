document.addEventListener('DOMContentLoaded', function () {
    // Function to handle deleting a product card with its image slider and delete button from the DOM
    function deleteProductCard(productId) {
        const productCard = document.querySelector(`[data-product-id="${productId}"]`);
        if (productCard) {
            productCard.remove(); // Remove the entire product card including the image slider and delete button
        }
    }

    // Function to handle deleting a product
    function deleteProduct(productId) {
        fetch(`/Product/DeleteProduct/${productId}`, {
            method: 'DELETE'
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                // Remove the product card, carousel, and delete button from DOM
                deleteProductCard(productId);
            })
            .catch(error => {
                console.error('There was a problem with the fetch operation:', error);
            });
    }

    // Attach event listeners to all delete buttons
    const deleteButtons = document.querySelectorAll('.delete-product');
    deleteButtons.forEach(button => {
        button.addEventListener('click', function (event) {
            event.preventDefault(); // Prevent the default behavior (e.g., page reload) of the click event
            const productId = event.target.dataset.productId; // Get the product ID from data attribute
            deleteProduct(productId); // Call the function to delete the product
        });
    });
});