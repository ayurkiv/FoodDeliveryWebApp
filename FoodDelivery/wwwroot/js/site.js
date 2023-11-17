// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function openFoodItemModal(name, category, price, imageUrl, description, weight, timeToReady, Id) {
    // Заповніть модальне вікно даними
    document.getElementById('modalFoodItemImage').src = imageUrl;
    document.getElementById('modalFoodItemName').innerText = name;
    document.getElementById('modelFoodDescription').innerText = description;
    document.getElementById('modalFoodItemCategory').innerText = category;
    document.getElementById('modalFoodItemPrice').innerText = price + '$';
    document.getElementById('modalFoodItemWeight').innerText = weight + 'g';
    document.getElementById('modalFoodItemTimeToReady').innerText = timeToReady + ' Mins';

    var addToCartButton = document.getElementById('addToCartButton');

    // Додайте обробник подій для кнопки
    addToCartButton.addEventListener('click', function () {
        // Викликайте функцію або робіть інші необхідні дії при натисканні кнопки
        // Наприклад, викликайте функцію addToCart з параметром item.Id
        addToCart(Id);
    });



    // Відкрийте модальне вікно
    $('#foodItemModal').modal('show');
}
function addToCart(itemId) {
    // Здійсніть AJAX-виклик до методу контролера для додавання товару до корзини
    $.post("/Home/AddToCart", { itemId: itemId })
}