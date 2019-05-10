<?php
  session_start();

  $username = 'dbi410102';
  $password = 'prop17';
  $user = $_POST['username'];
  $pass = $_POST['password'];

  $review = $_POST['comment'];

  $con = new PDO('mysql:host=studmysql01.fhict.local;dbname=dbi410102', $username, $password);

//check if there're any problems (data already in database) from previous sql statements
  if ($problems > 0) {
    $_SESSION['signup_failed'] = $problems;
    header("location:Register.php");
  } else {
    //if no users with those details - register
    $sql = "INSERT INTO account (name, surname, email, password, bankAccountNr) VALUES (?, ?, ?, ?, ?)";
    $statement = $con->prepare($sql);
    $statement->bindParam('1', $firstname);
    $statement->bindParam('2', $lastname);
    $statement->bindParam('3', $email);
    $statement->bindParam('4', $pass);
    $statement->bindParam('5', $IBAN);
    $result = $statement->execute();

    header('Location:LogIn.php');
  }

?>
