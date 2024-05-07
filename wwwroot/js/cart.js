// Get all radio buttons with name "news"

var radioButtons = document.querySelectorAll('input[name="news"]');
var deliveryitem = document.querySelector('input[name="news"]:checked').value;
 document.getElementById('selectedDeliveryId').value = deliveryitem;



// Add event listener to each radio button
radioButtons.forEach(function (radio) {
    radio.addEventListener('change', function () {
        // Check if the radio button is checked
        if (this.checked) {
            // Get the delivery cost associated with the selected shipping option
            var deliveryCost = this.dataset.deliveryCost;

            // Update the selected shipping option in the <li> element
            document.getElementById("selected-shipping").innerText = '$ ' + deliveryCost;
            deliveryitem = this.value;
            document.getElementById('selectedDeliveryId').value = deliveryitem
            console.log(this.value);
            console.log(document.getElementById('selectedDeliveryId').value);
            // Update the "You Pay" total
            updateTotal(deliveryCost);
        }
    });
});


// Function to update the "You Pay" total
function updateTotal(deliveryCost) {
    var cartTotal = parseFloat(document.querySelector('.cart-total-amount span').innerText.replace("$", ""));
    
    // Calculate the total
    var total = cartTotal + parseFloat(deliveryCost);

    // Update the "You Pay" total
    document.querySelector('.last span').innerText = '$' + total.toFixed(2);
}



function checkout(count) {
    // Get the selected delivery method
    var selectedDeliveryMethod = document.querySelector('input[name="news"]:checked');

    // If a delivery method is selected, proceed to checkout
    if (count == 0) {
        alert('Your cart is empty');

    }
    else if (!selectedDeliveryMethod) {
        alert('Please choose a delivery method before proceeding to checkout.');

    }
    else
    {
        window.location.href = '/Home/index';
    }
}

// JavaScript to get the checked Id from the radio button and send it to the server-side code
//document.addEventListener('DOMContentLoaded', function () {
//    document.querySelectorAll('input[name="news"]').forEach(function (radio) {
//        radio.addEventListener('change', function () {
//            var checkedId = this.value;
//            // Send checkedId to server-side code
//            sendDataToServer(checkedId);
//        });
//    });
//});

//function sendDataToServer(checkedId) {
//    var xhr = new XMLHttpRequest();
//    xhr.onreadystatechange = function () {
//        if (xhr.readyState === XMLHttpRequest.DONE) {
//            // Handle response from server if needed
//            console.log(xhr.responseText);
//        }
//    };
//    xhr.open("POST", "/Cart/ProceedToAddress", true);
//    xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
//    xhr.send(JSON.stringify({ checkedId: checkedId }));
//}

function sendCheckedId() {
    // Get the currently checked radio button value
    var checkedId = document.querySelector('input[name="news"]:checked').value;

    // Update the value of selectedDeliveryId
    document.getElementById('selectedDeliveryId').value = checkedId;

    // Send the checkedId to the server
    fetch('@Url.Action("ProceedToAddress", "Cart")', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: 'checkedId=' + encodeURIComponent(checkedId)
    })
        .then(response => response.text())
        .then(data => {
            console.log(data);
            // Optionally handle response
        })
        .catch(error => {
            console.error('Error:', error);
        });
}

