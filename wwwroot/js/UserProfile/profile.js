document.addEventListener("DOMContentLoaded", function () {
    let input = document.querySelector("#phone");
    let phoneInput = window.intlTelInput(input, {});


    setTimeout(function () {
        let [code, num] = document.querySelector("#phone").value.split("+")[1].split(" ");
        document.querySelector("#phone").value = num;

        let countryCode = phoneInput.getSelectedCountryData().dialCode;

        let phoneNumber = input.value;

        let phoneNumberWithCountryCode = "+" + countryCode + " " + phoneNumber;

        console.log(phoneNumberWithCountryCode);

        let hiddenInput = document.querySelector("#phoneNumberCountryCode");
        hiddenInput.value = phoneNumberWithCountryCode;
    }, 1000);
    document.querySelector("#phone").addEventListener('input', function (event) {

        let countryCode = phoneInput.getSelectedCountryData().dialCode;

        let phoneNumber = input.value;

        let phoneNumberWithCountryCode = "+" + countryCode + " " + phoneNumber;

        console.log(phoneNumberWithCountryCode);

        let hiddenInput = document.querySelector("#phoneNumberCountryCode");
        hiddenInput.value = phoneNumberWithCountryCode;


    });
});