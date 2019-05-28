<?php
$firstname = $_POST['firstname'];
$email = $_POST['email'];
$number = $_POST['number'];
$comment = $_POST['comment'];

$to = 'mitagki@abv.bg'; // note the comma

// Subject
$subject = 'Contacting us';

// Message
$message = "
<html>
<head>
<title></title>
</head>
<body>
<p>Name: $firstname</p>
<p>Email: $email</p>
<p>Telephone number: $number</p>
<p>Message: $comment</p>
<img src=\"https://www.w3schools.com/images/w3schools_green.jpg\" alt=\"W3Schools.com\"> 
</body>
</html>
";
//add picture with link from the server

// To send HTML mail, the Content-type header must be set
$headers[] = 'MIME-Version: 1.0';
$headers[] = 'Content-type: text/html; charset=iso-8859-1';

// Additional headers
$headers[] = 'To: Support Team <mitagki@abv.bg>';
$headers[] = "From: $firstname <$email>";

// Mail it
mail($to, $subject, $message, implode("\r\n", $headers));
header('Location:Home.php');

?>