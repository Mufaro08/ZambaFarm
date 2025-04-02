// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

    var toastElements = document.querySelectorAll('.toast');
    toastElements.forEach(function (toastElement) {
        var toast = new bootstrap.Toast(toastElement);
        toast.show();
    });


   
        // Show or hide the 'Number of Babies Being Nursed' field based on the 'Is Nursing' checkbox
        document.getElementById("isNursingCheckbox").addEventListener("change", function () {
            const numberOfBabiesDiv = document.getElementById("numberOfBabiesDiv");
        if (this.checked) {
            numberOfBabiesDiv.style.display = "block";
            } else {
            numberOfBabiesDiv.style.display = "none";
            }
        });
   