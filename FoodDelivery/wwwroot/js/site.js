// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function openFoodItemModal(name, category, price, imageUrl, description, weight, timeToReady, id) {
    // Заповніть модальне вікно даними
    document.getElementById('modalFoodItemImage').src = imageUrl;
    document.getElementById('modalFoodItemName').innerText = name;
    document.getElementById('modelFoodDescription').innerText = description;
    document.getElementById('modalFoodItemCategory').innerText = category;
    document.getElementById('modalFoodItemPrice').innerText = price + '$';
    document.getElementById('modalFoodItemWeight').innerText = weight + 'g';
    document.getElementById('modalFoodItemTimeToReady').innerText = timeToReady + ' Mins';

    var addToCartButton = document.getElementById('addToCartButton');

    // Відкрийте модальне вікно
    $('#foodItemModal').modal('show');
    // Додайте обробник подій для кнопки
    addToCartButton.addEventListener('click', function () {
        // Викликайте функцію або робіть інші необхідні дії при натисканні кнопки
        // Наприклад, викликайте функцію addToCart з параметром item.Id
        addToCart(id);
        $('#foodItemModal').modal('hide');
    });



}
function addToCart(itemId) {
    $.ajax({
        url: "/Home/AddToCart/?id=" + itemId,
        type: "POST",
        statusCode: {
            401: function(){
                window.alert('Unauthorized');
            }

        }
    });
}

function deleteCartItem(orderItemId) {
    //if (confirm('Are you sure you want to delete this item?')) {
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: '/Cart/Delete/' + orderItemId,
        type: 'DELETE',
        headers: {
            'RequestVerificationToken': token
        },
        success: function () {
            location.reload(); // or update the view accordingly
        },
        error: function () {
            alert('Error deleting item.');
        }
    });
    //}
}

function checkout() {
    //if (confirm('Are you sure you want to proceed with the checkout?')) {
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: '/Cart/Checkout',
        type: 'POST',
        headers: {
            'RequestVerificationToken': token
        },
        success: function () {
            // Redirect to the order confirmation page or update the view accordingly
            //window.location.href = '/Order/Index'; // Adjust the URL if needed
        },
        error: function () {
            alert('Error processing checkout.');
        }
    });
    //}
}
