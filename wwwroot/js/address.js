function toggleCheckboxValue() {
    var checkbox = document.getElementById("cbox");
    if (checkbox.checked) {
        checkbox.value = 1;
    } else {
        checkbox.value = 0;
    }
}
// Get all radio buttons with name "news"
var radioButtons = document.querySelectorAll('input[name="chooseAddress"]');

// Add event listener to each radio button
radioButtons.forEach(function (radio) {
    radio.addEventListener('change', function () {
        // Check if the radio button is checked
        if (this.checked) {
            if (this.value == "ch1") {

            }
        }
    });
});

function toggleAddNewAddress(radio) {
    var newAddressFields = document.getElementById("NewAddressForm");
    var ExistingAddress = document.getElementById("ExistingAddressForm");
    var ExistingSelect = document.getElementById("existing");
    if (radio.checked && radio.value === "ch2") {
        newAddressFields.style.display = "flex";
        ExistingAddress.style.display = "none";
    } else {
        ExistingAddress.style.display = "flex";
        ExistingSelect.style.display = "block";
        newAddressFields.style.display = "none";
    }
}
