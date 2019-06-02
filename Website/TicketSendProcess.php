<?php
session_start();

include('phpqrcode/qrlib.php');
include('phpqrcode/qrconfig.php');

$codeContents = $_SESSION['currentUser']['ticketNr'];

// we need to generate filename somehow,
// with md5 or with database ID used to obtains $codeContents...
$fileName = $_SESSION['currentUser']['ticketNr'] . '.png';

$filePath = 'UploadedImages/' . $fileName;

// generating
QRcode::png($codeContents, $filePath);

$to = $_SESSION['currentUser']['email'];

// Subject
$subject = 'Your ticket!';

// Message
$message = "
<html>
<head>
<title></title>
</head>
<body>
<p> Your ticket! </p>
<img src=\"" . "http://i415306.hera.fhict.nl/" . $filePath . "\" />
</body>
</html>
";
//add picture with link from the server

// To send HTML mail, the Content-type header must be set
$headers[] = 'MIME-Version: 1.0';
$headers[] = 'Content-type: text/html; charset=iso-8859-1';

// Mail it
mail($to, $subject, $message, implode("\r\n", $headers));

// echo $message;

header('Location:Profile.php');

?>
