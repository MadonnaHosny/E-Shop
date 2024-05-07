document.addEventListener("DOMContentLoaded", _ => {
    document.getElementById("verifyBtn")?.addEventListener("click", _ => {
        let spliredRoute = window.location.href.split('/');
        let Id = spliredRoute[spliredRoute.length - 1];

        fetch(`/admins/VerifySeller/${Id}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            }

        })
        .then(response => response.json())
        .then(data => {
            if (data) {
                document.getElementById("success").style.display = "block";
                document.getElementById("error").style.display = "none";


                setTimeout(_ => document.getElementById("success").style.display = "none", 2000);
            }
            else {
                document.getElementById("success").style.display = "none";
                document.getElementById("error").style.display = "block";
                setTimeout(_ => document.getElementById("error").style.display = "none", 2000);
            }
        })
        .catch(error => console.error(error));
    });
});