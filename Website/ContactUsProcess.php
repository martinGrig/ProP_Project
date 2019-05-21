<?php
$firstname = $_POST['firstname'];
$email = $_POST['email'];
$number = $_POST['number'];
$comment = $_POST['comment'];


// send email
mail("mitagkI@abv.bg","$firstname Contacting us","Email: $email\nTelephone number: $number\nMessage: $comment ");
header('Location:Home.php');
?>