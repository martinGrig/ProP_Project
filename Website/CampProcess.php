<?php
  session_start();

  $username = 'dbi410102';
  $password = 'prop17';

  $campspotName = $_POST['spots'];
  $people = $_POST['people'];
  $price = $people * 20 + 10;

  $con = new PDO('mysql:host=studmysql01.fhict.local;dbname=dbi410102', $username, $password);

  /*get the camp place from db by name - it's ID, capacity, reserved places*/

  $sql = "select * from campspot where name = ?";
  $statement = $con->prepare($sql);
  $statement->bindParam('1', $campspotName);
  $statement->execute();
  $result = $statement->fetchAll();

  if(count($result) > 0) {
    $campId = $result[0]['campSpotId'];

    //check if people can fit in this campspot
    $sql = "select * from campspot where campSpotId = ?";
    $statement = $con->prepare($sql);
    $statement->bindParam('1', $campId);
    $statement->execute();
    $result = $statement->fetchAll();

    $reserved = $result[0]['reservedPlaces'];
    $capacity = $result[0]['capacity'];
    if ($people + $reserved > $capacity) {
      //todo INDICATE THERE'RE NO SO MANY FREE PLACES
      header('Location:Camp.php');
    } else {
      //there're enough free places
      /* if user wants to pay for campspot - check if balance is more than price and than withdraw amount
      set isCampPayed to 1 for visitor*/
      if ($_POST['paynow_checkbox'] == "paynow") {
        if ($_SESSION['currentUser']['balance'] >= $price) {
          $sql = "UPDATE visitor SET balance = balance - ?, isCampPayed = '1', campSpotId = ? WHERE ticketNr = ?";
          $statement = $con->prepare($sql);
          $statement->bindParam('1', $price);
          $statement->bindParam('2', $campId);
          $statement->bindParam('3', $_SESSION['currentUser']['ticketNr']);
          $result = $statement->execute();

          $_SESSION['currentUser']['balance'] = $_SESSION['currentUser']['balance'] - $price;

          echo $sql;

        } else {
        //if user doesn't have enough money TO DO!
        header('Location:TopUp.php');
        }
      } else {
        /* if user does not want to pay now */
        $sql = "UPDATE visitor SET campSpotId = ? WHERE ticketNr = ?";
        $statement = $con->prepare($sql);
        $statement->bindParam('1', $campId);
        $statement->bindParam('2', $_SESSION['currentUser']['ticketNr']);
        $result = $statement->execute();
      }

      /* for taken place - add people in reservedPlaces */
      $sql = "UPDATE campspot SET reservedPlaces = reservedPlaces + ? WHERE campSpotId = ?";
      $statement = $con->prepare($sql);
      $statement->bindParam('1', $people);
      $statement->bindParam('2', $campId);
      $result = $statement->execute();

      header('Location:Profile.php');
    }
  }

  ?>
