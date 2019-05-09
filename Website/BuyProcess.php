<?php
  session_start();

  $username = 'dbi410102';
  $password = 'prop17';

  $tshirt = $_POST['t-shirt_checkbox'];
  $hoodie = $_POST['hoodie_checkbox'];
  $hat = $_POST['hat_checkbox'];
  $keychain = $_POST['keychain_checkbox'];

  $con = new PDO('mysql:host=studmysql01.fhict.local;dbname=dbi410102', $username, $password);

  $email = $_SESSION['currentUser']['email'];

  $sql = "INSERT INTO visitor (accountEmail) VALUES (?)";
  $statement = $con->prepare($sql);
  $statement->bindParam('1', $email);
  $result = $statement->execute();

  $sql = "select * from visitor where accountEmail = ?";
  $statement = $con->prepare($sql);
  $statement->bindParam('1', $email);
  $statement->execute();
  $result = $statement->fetchAll();

  if ($result) {
    $_SESSION['currentUser']['ticketNr'] = $result[0]['ticketNr'];
    $_SESSION['currentUser']['balance'] = $result[0]['balance'];
  }

  header("location:Profile.php");

?>
