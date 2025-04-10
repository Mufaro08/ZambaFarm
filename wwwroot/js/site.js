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

document.addEventListener("DOMContentLoaded", function () {
    function toggleFemaleOptions() {
        let gender = document.getElementById("genderSelect").value;
        let femaleOptions = document.getElementById("femaleOptions");
        if (gender === "Female") {
            femaleOptions.style.display = "block";
        } else {
            femaleOptions.style.display = "none";
            document.getElementById("isPregnantCheck").checked = false;
            document.getElementById("isNursingCheck").checked = false;
            document.getElementById("isMatingCheck").checked = false;
            document.getElementById("matingDateField").style.display = "none";
            document.getElementById("matingDateInput").value = "";
            document.getElementById("predictedDeliveryDate").value = "";
        }
    }

    function toggleMatingDateField() {
        let isMating = document.getElementById("isMatingCheck").checked;
        let matingDateField = document.getElementById("matingDateField");
        matingDateField.style.display = isMating ? "block" : "none";
        if (!isMating) {
            document.getElementById("matingDateInput").value = "";
            document.getElementById("predictedDeliveryDate").value = "";
        }
    }

    function calculateDeliveryDate() {
        let matingDateInput = document.getElementById("matingDateInput");
        let predictedDeliveryDate = document.getElementById("predictedDeliveryDate");
        if (matingDateInput.value) {
            let matingDate = new Date(matingDateInput.value);
            let deliveryDate = new Date(matingDate);
            deliveryDate.setDate(deliveryDate.getDate() + 30);
            predictedDeliveryDate.value = deliveryDate.toISOString().split('T')[0];
        } else {
            predictedDeliveryDate.value = "";
        }
    }

    document.getElementById("genderSelect").addEventListener("change", toggleFemaleOptions);
    document.getElementById("isMatingCheck").addEventListener("change", toggleMatingDateField);
    document.getElementById("matingDateInput").addEventListener("input", calculateDeliveryDate);

    toggleFemaleOptions();
    toggleMatingDateField();
});


AOS.init({
    duration: 1200, // Animation duration in milliseconds
    once: true // Animation should happen only once
});


$(document).ready(function () {
    // Smooth scrolling for links
    $('a.nav-link').on('click', function (event) {
        if (this.hash !== "") {
            event.preventDefault();
            var hash = this.hash;
            $('html, body').animate({
                scrollTop: $(hash).offset().top
            }, 800, function () {
                window.location.hash = hash;
            });
        }
    });
});