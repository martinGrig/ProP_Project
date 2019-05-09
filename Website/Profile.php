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
                <h3>
                  <?php
                $name = $_SESSION['currentUser']['name'];
                $surname = $_SESSION['currentUser']['surname'];
                echo $name . " " . $surname;
                ?>
                </h3>
                <h3>Ticket Number</h3>
                <h3>Balance</h3>
                <form>
                    <input type="button" value="Buy A Ticket" onclick="window.location.href='Buy.php'">
                    <input type="button" value="Top Up Balance" onclick="window.location.href='TopUp.php'">
                    <input type="button" value="Reserve A Camp Spot" onclick="window.location.href='Camp.php'">
                </form>
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
