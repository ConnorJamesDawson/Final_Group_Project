// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let showBtn = document.getElementById("btn-show-filters");
let hideBtn = document.getElementById("btn-hide-filters");
let filterForm = document.getElementById("filter-form");

showBtn.addEventListener('click', ev => {
    filterForm.style.display = "block";
    showBtn.style.display = "none";
    hideBtn.style.display = "block";
});

hideBtn.addEventListener('click', ev => {
    filterForm.style.display = "none";
    showBtn.style.display = "block";
    hideBtn.style.display = "none";
});