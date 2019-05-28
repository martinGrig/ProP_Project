<?php
  session_start();

  $username = 'dbi410102';
  $password = 'prop17';

  $review = htmlspecialchars($_POST['comment']);
  $name = $_SESSION['currentUser']['name'];
  $isAnonym = $_POST['anonymous_checkbox'];
  $ticketNr = $_SESSION['currentUser']['ticketNr'];

  if($isAnonym == "anonymous") {
    $name = "Anonymous";
  }

  if($name == "temp") {
    $name = "Anonymous";
  }

  $con = new PDO('mysql:host=studmysql01.fhict.local;dbname=dbi410102', $username, $password);

  $sql = "INSERT INTO review (name, reviewText, ticketNr) VALUES (?, ?, ?)";
  $statement = $con->prepare($sql);
  $statement->bindParam('1', $name);
  $statement->bindParam('2', $review);
  $statement->bindParam('3', $ticketNr);
  $result = $statement->execute();

  $sql = "UPDATE visitor SET hasReview = ? WHERE ticketNr = ?";
  $hasReview = 1;
  $statement = $con->prepare($sql);
  $statement->bindParam('1', $hasReview);
  $statement->bindParam('2', $_SESSION['currentUser']['ticketNr']);
  $result = $statement->execute();

  $_SESSION['currentUser']['hasReview'] = 1;

  header('Location:Reviews.php');

?>
