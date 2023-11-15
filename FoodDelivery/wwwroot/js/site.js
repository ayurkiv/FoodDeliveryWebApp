// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function openFoodItemModal(name, category, price, imageUrl, description, weight, timeToReady) {
    // Заповніть модальне вікно даними
    document.getElementById('modalFoodItemImage').src = imageUrl;
    document.getElementById('modalFoodItemName').innerText = name;
    document.getElementById('modelFoodDescription').innerText = description;
    document.getElementById('modalFoodItemCategory').innerText = category;
    document.getElementById('modalFoodItemPrice').innerText = price + '$';
    document.getElementById('modalFoodItemWeight').innerText = weight + 'g';
    document.getElementById('modalFoodItemTimeToReady').innerText = timeToReady + ' Mins';


    // Відкрийте модальне вікно
    $('#foodItemModal').modal('show');
}