<?php
$firstname = $_POST['firstname'];
$email = $_POST['email'];
$number = $_POST['number'];
$comment = $_POST['comment'];

$to = 'madprojects17@gmail.com'; // note the comma

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
</body>
</html>
";
//add picture with link from the server

// To send HTML mail, the Content-type header must be set
$headers[] = 'MIME-Version: 1.0';
$headers[] = 'Content-type: text/html; charset=iso-8859-1';

// Additional headers
$headers[] = 'To: Support Team <prop17team@hotmail.com>';
$headers[] = "From: $firstname <$email>";

// Mail it
mail($to, $subject, $message, implode("\r\n", $headers));
header('Location:Home.php');

?>
