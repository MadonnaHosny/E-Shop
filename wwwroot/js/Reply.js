function toggleSellerReply(button) {
    // Get the parent element of the button, which contains the reply input and submit button
    var parentDiv = button.parentNode.querySelector('.sellerReply');

    // Toggle the visibility of the parentDiv
    if (parentDiv.style.display === 'none' || parentDiv.style.display === '') {
        parentDiv.style.display = 'block';
    } else {
        parentDiv.style.display = 'none';
    }
}

    function submitReply() {
        var enteredText = $('input[name="enteredText"]').val();
    var commentId = $('#replyForm').data('comment-id');

    $.ajax({
        url: '@Url.Action("InsertReply", "Reply")',
    type: 'POST',
    data: {enteredText: enteredText, commentId: commentId },
    success: function () {
    }
        });
}

function toggleReplies(button) {
    var repliesList = button.nextElementSibling;
    if (repliesList.style.display === "none") {
        repliesList.style.display = "block";
        button.textContent = "Hide Replies";
    } else {
        repliesList.style.display = "none";
        button.textContent = "Show Replies";
    }
}