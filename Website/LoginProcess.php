<?php
  session_start();

  $username = 'dbi410102';
  $password = 'prop17';

  $email = $_POST['email'];
  $pass = $_POST['password'];

  $con = new PDO('mysql:host=studmysql01.fhict.local;dbname=dbi410102', $username, $password);

  //get all users with data provided by post
  $sql = "select * from account where email = ? and password = ?";
  $statement = $con->prepare($sql);
  $statement->bindParam('1', $email);
  $statement->bindParam('2', $pass);
  $statement->execute();
  $result = $statement->fetchAll();

  //if we have any user with these username and pass
  if(count($result) > 0){
    $_SESSION['loggedin'] = true;

    //getting info about just logged in user
    $sql = "select * from account where email = ? and password = ?";
    $statement = $con->prepare($sql);
    $statement->bindParam('1', $email);
    $statement->bindParam('2', $pass);
    $statement->execute();
    $result = $statement->fetchAll();
    $_SESSION['currentUser'] = $result[0];

    //check if person has a ticket
    $username = 'dbi410102';
    $password = 'prop17';
    $con = new PDO('mysql:host=studmysql01.fhict.local;dbname=dbi410102', $username, $password);

    $sql = "select * from visitor where accountEmail = ?";
    $statement = $con->prepare($sql);
    $email= $_SESSION['currentUser']['email'];
    $statement->bindParam('1', $email);
    $statement->execute();
    $result = $statement->fetchAll();
    if ($result) {
      $_SESSION['currentUser']['ticketNr'] = $result[0]['ticketNr'];
      $_SESSION['currentUser']['balance'] = $result[0]['balance'];
    }

    header("location:index.html");
  }

  //if we don't have users with such data
  else {
    header("location:LogIn.php");
  }
?>
