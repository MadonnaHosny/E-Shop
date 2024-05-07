function submitForm() {
    const form = document.querySelector("#productForm");
    if (form) {
        if (form.checkValidity()) {
            form.submit();
        } else {
            form.reportValidity();
        }
    } else {
        console.error("Form element not found");
    }
}