// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
<script>
    var toastElements = document.querySelectorAll('.toast');
    toastElements.forEach(function (toastElement) {
        var toast = new bootstrap.Toast(toastElement);
        toast.show();
    });
</script>
