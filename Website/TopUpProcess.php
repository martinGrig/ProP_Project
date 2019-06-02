<?php
  session_start();

  $username = 'dbi410102';
  $password = 'prop17';

  $amount = $_POST['topUpAmount'];

  $con = new PDO('mysql:host=studmysql01.fhict.local;dbname=dbi410102', $username, $password);

  $sql = "UPDATE visitor SET balance = balance + ? WHERE ticketNr = ?";
  $statement = $con->prepare($sql);
  $statement->bindParam('1', $amount);
  $statement->bindParam('2', $_SESSION['currentUser']['ticketNr']);
  $result = $statement->execute();

  $_SESSION['currentUser']['balance'] = $_SESSION['currentUser']['balance'] + $amount;


  $email = $_SESSION['currentUser']['email'];
  mail($email, "Your ticket!", "MESSAGE");

  header('Location:Profile.php');
  ?>
