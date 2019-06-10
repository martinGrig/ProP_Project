<?php
session_start();
?>
<!DOCTYPE html>
<html>
    <head>
        <title>BoardGames Profile</title>
        <link href="styles/styles.css" type="text/css" rel="stylesheet"></link>
        <script src="js/js.js"></script>
        <meta charset="UTF-8">
    </head>
    <body>
        <div class="header">
        </div>
        <div class="topnav">
            <a href="Home.php">Home</a>
            <a href="Events.php">Events</a>
            <a href="ContactUs.php">Contact us</a>
            <a href="Reviews.php">Reviews</a>
            <?php
            if (isset($_SESSION['loggedin'])){
              echo "<a href=\"LogOut.php\" style=\"float:right\">Log Out</a>";
               echo "<a href=\"Profile.php\" style=\"float:right\">Profile</a>";
             } else {
              echo "<a href=\"LogIn.php\" style=\"float:right\">Log in</a>";
             }
             ?>
        </div>
        <div class="row">
            <div class="column centre ">
              <div class="card">
                <h2>Profile</h2>

                <?php
                $name = $_SESSION['currentUser']['name'];
                $surname = $_SESSION['currentUser']['surname'];
                echo "<h3>" .  $name . " " . $surname . "</h3>";

                if (!isset($_SESSION['currentUser']['ticketNr'])) {
                  echo "<h3>No ticket purchaised yet!</h3>";
                  echo "<input type=\"button\" value=\"Buy A Ticket\" onclick=\"window.location.href='Buy.php'\">";
                } else {
                  $ticketNr = $_SESSION['currentUser']['ticketNr'];
                  $balance = $_SESSION['currentUser']['balance'];
                  echo "<h3>Ticket number: " .  $ticketNr . "</h3>";
                  echo "<h3>Balance: " .  $balance . "€</h3>";
                  echo "
                  <input type=\"button\" value=\"Top Up Balance\" onclick=\"window.location.href='TopUp.php'\">
                  <input type=\"button\" value=\"Reserve Camp Spot\" onclick=\"window.location.href='Camp.php'\">
                  ";
                }
                ?>

              </div>
            </div>
        </div>
        <div class="row">
            <div class="footer">
                <h2>
                    ADDRESS
                </h2>
                <h3>
                    <br>
                    Eindhoven, Netherlands
                </h3>
            </div>
            <div class="footer">
                <h2>
                    CONTACT
                </h2>
                <h3>
                    <br>
                    info@mysite.com<br>
                    Tel: 123-456-7890
                </h3>
            </div>
            <div class="footer">
                <h2>
                    BoardGames
                </h2>
                <h3>
                    <br>
                    © 2019 by BoardGames
                </h3>
            </div>
            <div class="footer">
                <h2>
                    WE ACCEPT
                </h2>
                <h3>
                    <img src="images/weaccept.png" width="200" height="75">
                </h3>
            </div>
        </div>
    </body>
</html>
