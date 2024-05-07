function handleBrandChange(selectElement) {
    if (selectElement.value == '-1') {
        $('#otherBrandGroup').show();
    } else {
        $('#otherBrandGroup').hide();
    }
}
function handleCategoryChange(selectElement) {
    if (selectElement.value == '-1') {
        $('#otherCategoryGroup').show();
    } else {
        $('#otherCategoryGroup').hide();
    }
}


