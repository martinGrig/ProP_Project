<?php
session_start();
?>
<!DOCTYPE html>
<html>
    <head>
        <title>BoardGames Buy A Ticket</title>
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
                <h2>Buy A Ticket</h2>
                <h3>Check an item with your ticket</h3>
                <form name="Buy-form" action="BuyProcess.php" method="post">
                    <label class="container">T-Shirt
                    <input type="checkbox" name="t-shirt_checkbox" value="t-shirt">
                    <span class="checkmark"></span>
                    </label>

                    <label class="container">Hoodie
                    <input type="checkbox" name="hoodie_checkbox" value="hoodie">
                    <span class="checkmark"></span>
                    </label>

                    <label class="container">Hat
                    <input type="checkbox" name="hat_checkbox" value="hat">
                    <span class="checkmark"></span>
                    </label>

                    <label class="container">Key Chain
                    <input type="checkbox" name="keychain_checkbox" value="keychain">
                    <span class="checkmark"></span>
                    </label>
                    <input type="submit" value="Confirm">
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
