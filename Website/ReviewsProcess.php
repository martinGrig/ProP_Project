<?php
  session_start();

  $username = 'dbi410102';
  $password = 'prop17';

  $review = htmlspecialchars($_POST['comment']);
  $name = "BEFOREFRONTEND";

  $con = new PDO('mysql:host=studmysql01.fhict.local;dbname=dbi410102', $username, $password);

    $sql = "INSERT INTO review (name, reviewText) VALUES (?, ?)";
    $statement = $con->prepare($sql);
    $statement->bindParam('1', $name);
    $statement->bindParam('2', $review);

    $result = $statement->execute();

    header('Location:Reviews.php');

?>
