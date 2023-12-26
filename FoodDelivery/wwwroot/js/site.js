// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Function to open the food item modal
function openFoodItemModal(name, category, price, imageUrl, description, weight, timeToReady, id) {
    // Fill in the modal content
    document.getElementById('modalFoodItemImage').src = imageUrl;
    document.getElementById('modalFoodItemName').innerText = name;
    document.getElementById('modelFoodDescription').innerText = description;
    document.getElementById('modalFoodItemCategory').innerText = category;
    document.getElementById('modalFoodItemPrice').innerText = price + '$';
    document.getElementById('modalFoodItemWeight').innerText = weight + 'g';
    document.getElementById('modalFoodItemTimeToReady').innerText = timeToReady + ' Mins';

    // Set data attribute on the "Add to Cart" button
    var addToCartButton = document.getElementById('addToCartButton');
    addToCartButton.dataset.itemId = id;
    // Open the modal
    $('#addToCartButton').data('item-id', id);
    $('#foodItemModal').modal('show');
}

// Attach a single event listener to the parent modal for handling "Add to Cart" clicks
$('#foodItemModal').on('click', '#addToCartButton', function () {
    // Get the item ID from the button data attribute
    var itemId = $(this).data('item-id');

    // Call the addToCart function with the item ID
    addToCart(itemId);
    // Hide the modal
    $('#foodItemModal').modal('hide');
});

// Function to add item to the cart
function addToCart(itemId) {
    $.ajax({
        url: "/Home/AddToCart/?id=" + itemId,
        type: "POST",
        statusCode: {
            401: function () {
                document.getElementsByClassName("modal-place-warning")[0].classList.add('active');
                setTimeout(function () { 
                    document.getElementsByClassName("modal-place-warning")[0].classList.remove('active');
                }, 3000)
            }
        }
    });
}

//function deleteCartItem(orderItemId) {
//    //if (confirm('Are you sure you want to delete this item?')) {
//    var token = $('input[name="__RequestVerificationToken"]').val();

//    $.ajax({
//        url: '/Cart/Delete/' + orderItemId,
//        type: 'DELETE',
//        headers: {
//            'RequestVerificationToken': token
//        },
//        success: function () {
//            location.reload(); // or update the view accordingly
//        },
//        error: function () {
//            alert('Error deleting item.');
//        }
//    });
//    //}
//}