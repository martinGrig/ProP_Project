<?php
/*file made for login php code, to not make html in another file too complicated*/
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
    $sql = "select * from accounts where email = ? and password = ?";
    $statement = $con->prepare($sql);
    $statement->bindParam('1', $email);
    $statement->bindParam('2', $pass);
    $statement->execute();
    $result = $statement->fetchAll();
    $_SESSION['currentUser'] = $result[0];

    header("location:index.html");
  }

  //if we don't have users with such data
  else {
    header("location:LogIn.php");
  }
?>
